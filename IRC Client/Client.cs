using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Text.RegularExpressions;

namespace IRC_Client
{
    public partial class clientForm : Form
    {
        enum type{
            MSG,
            JOIN
        }
        string[] serverGroups;
        volatile Socket sock = null;
        IPHostEntry host = null;
        Thread listener;
        Dictionary<String, List<String>> nicknames = new Dictionary<String, List<String>>();
        string myNick = "notsetyet";
        


        public clientForm()
        {
            nicknames["server"] = new List<String>();
            nicknames["server"].Add("Click a channel to view users");
            InitializeComponent();
            populateServers();
            this.FormClosed += clientForm_FormClosed;
        }

        private void listenOnMainSocket()
        {
            int bytes = 0;
            Byte[] bytesRecieved = new Byte[256];

            sock.ReceiveTimeout = 1000;
            sock.SendTimeout = 1000;

            String outputString = "";
            String bit = "1";
            bytes = 0;
            string[] newlineDelimiter = {"\r\n"};
            char[] spaceDelimiter = {' '};
            char[] periodSpaceDelimiter = { '.', ' ' };
            char[] exclamationDelimiter = { '!' };
            char[] colonDelimiter = { ':' };

            do
            {

                if (sock.Poll(1000, SelectMode.SelectRead))
                {
                    lock (sock)
                    {
                        try
                        {
                            //GETS THE STUFF FROM THE SERVER

                            bytes = sock.Receive(bytesRecieved, bytesRecieved.Length, 0);
                        }

                        catch (System.Net.Sockets.SocketException e)
                        {

                            System.Diagnostics.Debug.WriteLine("Error: listenOnMainSocket Socket Exception.");
                        }
                    }



                    if (bytes > 0)
                    {
                        bit = Encoding.ASCII.GetString(bytesRecieved, 0, bytes);

                        //add on the remaining tail portion from last received string
                        outputString += bit;

                    }

                }

                if (outputString.Contains("\r\n"))
                {

                    //split on "\r\n"
                    var split = outputString.Split(newlineDelimiter, 2, StringSplitOptions.None);

                    outputString = split[1];

                    string nextLine = split[0];
                    System.Diagnostics.Debug.WriteLine("Received: {" +nextLine + "}");

                    //first handle PINGs
                    split = nextLine.Split(spaceDelimiter, 2, StringSplitOptions.None);

                    if (split[0].Equals("PING"))
                    {
                        //send PONG
                        Byte[] bytesSent = Encoding.ASCII.GetBytes("PONG");
                        sock.Send(bytesSent, bytesSent.Length, 0);
                        System.Diagnostics.Debug.WriteLine("Sent: {" + "PONG" + "}");
                    }
                    else
                    {

                        string source = split[0];
                        string message = split[1];
                        //check if its a server message or a personal message
                        split = source.Split(periodSpaceDelimiter, 2, StringSplitOptions.None);

                        if (split[0].Substring(1).Equals(this.myNick) || split[0].Equals(":NickServ!NickServ@services") || split[0].Equals(":ChanServ!ChanServ@services"))
                        {
                            //treat like a server.  its this weird particular messages {:lilfrosty MODE lilfrosty :+i}, {:NickServ!NickServ@services. NOTICE lilfrosty :lilfrosty is not a registered nickname.}

                            //handle like a server message, just print it out to the console
                            this.writeToRoom("server", message.Split(spaceDelimiter, 3, StringSplitOptions.None)[2] + "\n");
                        }
                        else if (split.Length == 2 && split[1] == "freenode.net")
                        {
                            //it's a message from the server
                            //the first bit will be the code
                            split = message.Split(spaceDelimiter, 5, StringSplitOptions.None);
                            string typeCode = split[0];

                            //TODO: handle server messages.  they will have that server code thingy
                            if (typeCode.Equals("353"))
                            {
                                //get key
                                string key = split[3].Substring(1);
                                if (key.Equals("#chat"))
                                {
                                    key = "general"; //peform the #chat -> general swap
                                }

                                System.Diagnostics.Debug.WriteLine("key = " + key);

                                if (!this.nicknames.ContainsKey(key))
                                {
                                    this.nicknames.Add(key, new List<String>());
                                }

                                string data = split[4].Substring(1);
                                string[] nicks = data.Split(spaceDelimiter);
                                foreach (string nick in nicks)
                                {
                                    this.nicknames[key].Add(nick);
                                }
                            }
                            else if (typeCode.Equals("366"))
                            {

                                //get key
                                string key = split[2].Substring(1);
                                if (key.Equals("#chat"))
                                {
                                    key = "general"; //peform the #chat -> general swap
                                }

                                string tab = getTabName();
                                if (key.Equals(tab))
                                {
                                    updateNicklist(key);
                                }
                            }
                            else if (typeCode.Equals("332"))
                            {
                                //message of the day

                                //get key
                                string key = split[2].Substring(1);
                                if (key.Equals("#chat"))
                                {
                                    key = "general"; //peform the #chat -> general swap
                                }
                                this.writeToRoom(key, message.Split(spaceDelimiter, 4, StringSplitOptions.None)[3].Substring(1) + "\n");
                            }
                            else {
                                //just log it to the console
                                this.writeToRoom("server", message.Split(spaceDelimiter, 3, StringSplitOptions.None)[2] + "\n");
                            }

                        }
                        else
                        {
                            //it's a message from another user
                            split = source.Split(exclamationDelimiter, 2, StringSplitOptions.None);
                            string userNick = split[0].Substring(1);

                            split = message.Split(spaceDelimiter, 3, StringSplitOptions.None);
                            string typeCode = split[0]; // PRIVMSG, JOIN, or QUIT
                            string chatRoom = split[1].Substring(1); //takes off the hashtag
                            if (chatRoom.Equals("#chat"))
                            {
                                chatRoom = "general"; //peform the #chat -> general swap
                            }
                            //message = split[2]

                            if (typeCode.Equals("PRIVMSG"))
                            {
                                //it is a message, so it's safe to use split[2].substring(1) as the message contents

                                this.writeToRoom(chatRoom, userNick + "> " + split[2].Substring(1) + "\n");
                            }
                            else if (typeCode.Equals("JOIN"))
                            {
                                if (userNick != this.myNick) { //ignore your own notification
                                    //System.Diagnostics.Debug.WriteLine("Message from: " + userNick + " of type: " + typeCode + " in chat room: " + chatRoom);
                                    this.nicknames[chatRoom].Add(userNick);
                                    string tab = getTabName();
                                    if (chatRoom.Equals(tab))
                                    {
                                        updateNicklist(chatRoom);
                                    }
                                }
                            }
                            else if (typeCode.Equals("QUIT"))
                            {

                                if (userNick != this.myNick) //ignore your own notification
                                {
                                    foreach (string room in this.nicknames.Keys)
                                    {
                                        this.nicknames[room].Remove(userNick); // removes him from all rooms
                                    }

                                    string tab = getTabName();
                                    if (chatRoom.Equals(tab))
                                    {
                                        updateNicklist(chatRoom);
                                    }
                                }
                            }
                            else
                            {
                                System.Diagnostics.Debug.WriteLine("Message from: " + userNick + " of type: " + typeCode + " in chat room: " + chatRoom);
                                this.writeToRoom("server", "Undisplayed message, fell through the parsing: {" + source + " " + message + "}");
                            }

                        }
                    }
                }

                    //-----------------------------------------------------------------------------------------------------------------------------------
    
                        if (false)
                        {
                            bit = Encoding.ASCII.GetString(bytesRecieved);

                            outputString += bit;


                            System.Diagnostics.Debug.WriteLine(bit);

                            //TODO: Change to write to correct room
                            if (outputString.Contains("\r\n"))
                            {



                                Regex inputs = new Regex(":(.*?) (.*?):(.*?)\r\n", RegexOptions.IgnoreCase);
                                var split = inputs.Split(outputString);
                                
                                System.Diagnostics.Debug.WriteLine("--------------------------");
                                //First one is garbage we want the next 3
                                for (var i=1; i<split.Length;i+=4 )
                                {
                                    var server = split[i];
                                    var status = split[i + 1];
                                    var data = split[i + 2];

                                    doStuff(server, status, data);
                                    //System.Diagnostics.Debug.Write("{");
                                    //System.Diagnostics.Debug.WriteLine("{"+server+"}"+"{"+status+"}"+"{"+data+"}");
                                    //System.Diagnostics.Debug.Write("}\n");

                                }

                                //System.Diagnostics.Debug.WriteLine("split length: ");
                                //System.Diagnostics.Debug.WriteLine(split.Length);

                                //"\r\n" + somethin that is the start of the next line
                                outputString = split[split.Length - 1];
                                bit = "1";
                                bytes = 0;
                                Array.Clear(bytesRecieved, 0, bytesRecieved.Length);

                            }
                        }
                    
                
            } while (true);
        }

        private void doStuff(String server, String status, String data)
        {
            char[] delimiterChars = {' '};
            string[] statuss = status.Split(delimiterChars);
            int value;


            System.Diagnostics.Debug.WriteLine("{" + server + "}" + "{" + status + "}" + "{" + data + "}");

            switch (statuss[0])
            {
                case "353":
                    //Nicklist
                    string key  = "";
                    if (statuss[3] == "##chat")
                    {
                        key = "general";
                    }
                    else{
                        key = statuss[3].Substring(1);
                    }
                    if (!this.nicknames.ContainsKey(key))
                    {
                        this.nicknames.Add(key, new List<String>());
                    }

                    string[] nicks = data.Split(delimiterChars);
                    foreach (string nick in nicks)
                    {
                        this.nicknames[key].Add(nick);
                    }

                    break;
                case "366":
                    key  = "";
                    
                    if (statuss[2] == "##chat")
                    {
                        key = "general";
                    }
                    else{
                        key = statuss[2].Substring(1);
                    }
                    

                    string tab = getTabName();
                    if(key.Equals(tab)){
                        updateNicklist(key);
                    }
                    break;
                case "332":
                    //MOTD
                    if (statuss[2] == "##chat")
                    {
                        this.writeToRoom("general", data+"\n");
                    }
                    else
                    {
                        this.writeToRoom(statuss[2].Substring(1), data + "\n");
                    }

                    break;
                case "PRIVMSG":
                    string[] names = server.Split(new string[] { "!" }, StringSplitOptions.None);
                    if (statuss[1] == "##chat")
                    {
                        this.writeToRoom("general", " "+ names[0] + "> " + data + "\n");
                    }
                    else
                    {
                        this.writeToRoom(statuss[1].Substring(1), " " + names[0] + "> " + data + "\n");
                    }
                    break;
                case "JOIN":
                    //Need to add the person on join but we don't know the channel they are joining to
                    names = server.Split(new string[] { "!" }, StringSplitOptions.None);
                    System.Diagnostics.Debug.WriteLine("We just got a JOIN");
                    break;
                case "QUIT":
                    //Need to add the person on join but we don't know the channel they are joining to
                    names = server.Split(new string[] { "!" }, StringSplitOptions.None);

                    break;
                default:
                    this.writeToRoom("server", server + ": " + status + ": " + data + "\n");
                    //print to server
                    break;

            }


        }

        public string  getTabName()
         {
            if (tabChats.InvokeRequired)
            {
                string res = "";
                var action = new Action<TabControl>(c => res = c.SelectedTab.Text);
                tabChats.Invoke(action, tabChats);
                return res;
            }
            string varText = tabChats.SelectedTab.Text;
            return varText;
        }

        

        private void addItemToNicklist(String s)
        {
            lstNicks.Items.Add(s);
        }

        private void clearNicks(String s)
        {
            lstNicks.Items.Clear();
        }

        private void updateNicklist(String room)
        {

            if (lstNicks.InvokeRequired)
            {
                this.Invoke(new Action<String>(clearNicks), new object[] { "aa"});
            }
            else
            {
                clearNicks("aa");
            }
            if (this.nicknames.ContainsKey(room))
            {
                foreach (string nick in this.nicknames[room])
                {
                    if (lstNicks.InvokeRequired)
                    {
                        this.Invoke(new Action<String>(addItemToNicklist), new object[] { nick });
                    }
                    else
                    {
                        addItemToNicklist(nick);
                    }

                }
            }
            

            
        }
        void clientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.listener != null && this.listener.IsAlive)
            {
                this.listener.Abort();
            }

            if (sock != null && sock.Connected)
            {
                sock.Close();
            }
        }

        private void btnAddServer_Click(object sender, EventArgs e)
        {
            NewServer srvDialog = new NewServer();
            srvDialog.Show();
            this.Enabled = false;
            srvDialog.FormClosed += addClosed;
            
        }

        private void addClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
            populateServers();
        }

        private void populateServers()
        {
            string servers = (string)Properties.Settings.Default["servers"];

            if (servers.Equals(""))
            {
                this.lstServers.Items.Clear();
                return;
            }
            this.serverGroups = servers.Split(',');
            this.lstServers.Items.Clear();
            foreach (var server in this.serverGroups)
            {
                string[] parts = server.Split(':');
                this.lstServers.Items.Add(parts[1]);
            }
            if (this.lstServers.Items.Count > 0)
            {
                this.lstServers.SelectedIndex = 0;
            }
        }

        private void removeServer(int index)
        {
            List<string> removed = this.serverGroups.ToList<string>();
            removed.RemoveAt(index);
            string newServers = String.Join(",", removed);
            Properties.Settings.Default["servers"] = newServers;
            Properties.Settings.Default.Save();
            this.populateServers();
        }

        private void btnConnectServer_Click(object sender, EventArgs e)
        {
            if (this.listener != null && this.listener.IsAlive)
            {
                this.listener.Abort();
            }
            if (this.sock != null && this.sock.Connected)
            {
                this.sock.Close();
            }
            this.txtChatMain.Clear();
            var selected = this.lstServers.SelectedIndex;
            var serverInfo = this.serverGroups[selected].Split(':');
            string serverName = serverInfo[1];
            this.txtChatMain.AppendText("Connecting to " + serverName + "...\r\n");
            string nick = serverInfo[0];
            this.myNick = nick;
            string port = serverInfo[2];
            string pass = serverInfo[3];
            string rooms = serverInfo[4];
            string real = serverInfo[5];
            string user = serverInfo[6];
            this.lblServerName.Text = serverName;

            this.sock = startSocket(serverName, port);

            if (!this.sock.Connected)
            {
                MessageBox.Show("Failed to connect to server.");
            }

            this.loginWithSocket(this.sock, nick, user, pass, real);

            this.listener = new Thread(new ThreadStart(this.listenOnMainSocket));
            this.listener.Start();
        }

        private Socket startSocket(string serverName, string port)
        {
            host = Dns.GetHostEntry(serverName);

            //this.txtChatMain.AppendText("Redirected to " + host.HostName + "...\r\n");

            IPAddress address = host.AddressList[0];
            IPEndPoint end = new IPEndPoint(address, Convert.ToInt32(port));

            Socket temp = new Socket(end.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            temp.Connect(end);

            return temp;
        }

        private void loginWithSocket(Socket sock, string nick, string user, string pass, string real)
        {
            if (pass.Length == 0)
	        {
		        pass = "**";
        	}

            Byte[] bytesSent = Encoding.ASCII.GetBytes("PASS " + pass + "\r\n");
            sock.Send(bytesSent, bytesSent.Length, 0);

            bytesSent = Encoding.ASCII.GetBytes("NICK " + nick + "\r\n");
            sock.Send(bytesSent, bytesSent.Length, 0);

            bytesSent = Encoding.ASCII.GetBytes("USER " + String.Join(" ", user, "0 *", real) + "\r\n");
            sock.Send(bytesSent, bytesSent.Length, 0);

        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            this.removeServer(this.lstServers.SelectedIndex);
        }

        private void writeToRoom(string room, string line)
        {
            foreach (TabPage page in this.tabChats.TabPages)
            {
                if (page.Text.Equals(room))
                {
                    TextBox box = (TextBox)page.Controls[0];
                    if (InvokeRequired)
                    {
                        this.Invoke(new Action<string, string>(writeToRoom), new object[] { room, line });
                        continue;
                    }
                    string marker = "[" + DateTime.Now.ToString("HH:mm") + "] ";
                    for (int i = 0; i < line.Length-1; i++)
                    {
                        if (line[i] == '\n')
                        {
                            line = line.Substring(0, i + 1) + marker + line.Substring(i+1);
                            i += marker.Length;
                        }
                    }
                    if (box.Text.EndsWith("\n"))
                    {                        
                        box.AppendText(marker + line);
                    }
                    else
                    {
                        box.AppendText(marker + line);
                    }
                }
            }
        }

        private void makeRoomTab(string room)
        {
            TabPage roomTab = new TabPage(room);
            roomTab.UseVisualStyleBackColor = true;
            TextBox box = new TextBox();
            box.Height = 474;
            box.Width = 729;
            box.ScrollBars = ScrollBars.Vertical;
            box.ReadOnly = true;
            box.Multiline = true;
            box.Location = new Point(6, 6);
            roomTab.Controls.Add(box);
            this.tabChats.Controls.Add(roomTab);
        }

        private void btnJoinRoom_Click(object sender, EventArgs e)
        {
            NewRoom roomDialog = new NewRoom();
            roomDialog.Show();
            this.Enabled = false;
            roomDialog.FormClosed += addRoomClosed;
        }
        private void addRoomClosed(object sender, FormClosedEventArgs e)
        {
            this.Enabled = true;
            string room = (string)Properties.Settings.Default["newRoom"];
            makeRoomTab(room);
            sendMsg(room, type.JOIN);
        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            this.listener.Abort();
        }

        private void menuAddServer_Click(object sender, EventArgs e)
        {
            this.btnAddServer_Click(null, null);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            var selected = this.lstServers.SelectedIndex;
            var serverInfo = this.serverGroups[selected].Split(':');
            string serverName = serverInfo[1];
            string nick = serverInfo[0];
            string port = serverInfo[2];
            string pass = serverInfo[3];
            string rooms = serverInfo[4];
            string real = serverInfo[5];
            string user = serverInfo[6];
            NewServer srvDialog = new NewServer(serverName, nick, port, pass, rooms, real, user);
            srvDialog.Show();
            this.Enabled = false;
            srvDialog.FormClosed += addClosed;
            this.lstServers.Items.Clear();
            populateServers();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (this.sock == null || !this.sock.Connected)
            {
                return;
            }
            string msg = txtMsg.Text.ToString();
            if (msg.Length > 0)
            {
                sendMsg(msg,type.MSG);
                msg += "\n";
                writeToRoom(this.tabChats.SelectedTab.Text,this.myNick + "> " + msg);
                txtMsg.Text = "";
            }
            
        }
        private void sendMsg(string msg, type t)
        {
            Byte[] bytesSent = null;
            switch(t){
                case(type.JOIN):
                    bytesSent = Encoding.ASCII.GetBytes("JOIN #"+msg+"\r\n");
                    break;
                case(type.MSG):
                    bytesSent = Encoding.ASCII.GetBytes("PRIVMSG #"+this.tabChats.SelectedTab.Text+" :"+msg+"\r\n");
                    break;
            }
            lock(sock)
            {
                sock.Send(bytesSent,bytesSent.Length,0);
            }
        }

        private void tabPageMain_Click(object sender, EventArgs e)
        {

        }

        private void clientForm_Load(object sender, EventArgs e)
        {

        }

        private void txtChatMain_TextChanged(object sender, EventArgs e)
        {

        }

        private void tabChats_SelectedIndexChanged(object sender, EventArgs e)
        {
           


        }

        private void tabChats_TabIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void tabChats_Selected(object sender, TabControlEventArgs e)
        {


        }

        private void s(object sender, EventArgs e)
        {

        }

        private void btnLeave_Click_1(object sender, EventArgs e)
        {
            string closingTab = getTabName();
            this.nicknames.Remove(closingTab);

            //TODO: This is where we need to graphically remove the tab and all that.

            int i = this.tabChats.SelectedIndex;
            if (i > 0)
            {
                this.tabChats.TabPages.RemoveAt(i);
            }



        }

        private void tabPageMain_Click_1(object sender, EventArgs e)
        {

        }

        private void tabChats_SelectedIndexChanged_1(object sender, EventArgs e)
        {

            //change to the right nicknames
            String newTab = getTabName();
            updateNicklist(newTab);

        }

    }
}
