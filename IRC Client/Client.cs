﻿using System;
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

            do
            {
                
                if (sock.Poll(-1, SelectMode.SelectRead))
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
                            bit = Encoding.ASCII.GetString(bytesRecieved);

                            outputString += bit;

                            //TODO: Change to write to correct room
                            if (bit.Contains("\r\n"))
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
                                    System.Diagnostics.Debug.Write("{");
                                    System.Diagnostics.Debug.Write("{"+server+"}"+"{"+status+"}"+"{"+data+"}");
                                    System.Diagnostics.Debug.Write("}\n");

                                }

                                //System.Diagnostics.Debug.WriteLine("split length: ");
                                //System.Diagnostics.Debug.WriteLine(split.Length);
                                //System.Diagnostics.Debug.WriteLine(outputString);
                                this.writeToRoom("server", outputString);
                                //"\r\n" + somethin that is the start of the next line
                                outputString = split[split.Length - 1];
                                bit = "1";
                                bytes = 0;
                                Array.Clear(bytesRecieved, 0, bytesRecieved.Length);

                            }
                        }
                    
                }
            } while (true);
        }

        private void doStuff(String server, String status, String data)
        {
            char[] delimiterChars = {' '};
            string[] statuss = status.Split(delimiterChars);
            int value;
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

                    System.Diagnostics.Debug.Write("Printing nick list: ");

                    foreach (string nick in this.nicknames[key])
                    {
                        System.Diagnostics.Debug.Write(nick);
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
                    break;
                case "QUIT":
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

            foreach (string nick in this.nicknames[room])
            {
                if (lstNicks.InvokeRequired)
                {
                    this.Invoke(new Action<String>(addItemToNicklist), new object[] { nick });
                } else {
                    addItemToNicklist(nick);
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
            this.txtChatMain.AppendText("Connecting...\r\n");
            var selected = this.lstServers.SelectedIndex;
            var serverInfo = this.serverGroups[selected].Split(':');
            string serverName = serverInfo[1];
            string nick = serverInfo[0];
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
                        box.AppendText(line);
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

            //change to the right nicknames
            String newTab = getTabName();

            updateNicklist(newTab);



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

    }
}
