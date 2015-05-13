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

namespace IRC_Client
{
    public partial class clientForm : Form
    {
        string[] serverGroups;
        volatile Socket sock = null;
        IPHostEntry host = null;
        Thread listener;

        public clientForm()
        {
            InitializeComponent();
            populateServers();
            this.FormClosed += clientForm_FormClosed;
        }

        private void listenOnMainSocket()
        {
            int bytes = 0;
            Byte[] bytesRecieved = new Byte[256];
            do
            {
                lock (sock)
                {
                    bytes = sock.Receive(bytesRecieved, bytesRecieved.Length, 0);
                }
                if (bytes > 0)
                {                    
                    this.writeToRoom("server", Encoding.ASCII.GetString(bytesRecieved, 0, bytes));
                }
            } while (true);
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

        }

        private void btnDisconnect_Click(object sender, EventArgs e)
        {
            this.listener.Abort();
        }

        private void menuAddServer_Click(object sender, EventArgs e)
        {
            this.btnAddServer_Click(null, null);
        }

    }
}
