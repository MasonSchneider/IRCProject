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
        bool registered = false;

        public clientForm()
        {
            InitializeComponent();
            populateServers();
            this.FormClosed += clientForm_FormClosed;
        }

        private void listenOnSocket()
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
            if (this.listener.IsAlive)
            {
                this.listener.Abort();
            }

            if (sock != null)
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
            host = Dns.GetHostEntry(serverName);

            IPAddress address = host.AddressList[0];
            IPEndPoint end = new IPEndPoint(address, Convert.ToInt32(port));

            sock = new Socket(end.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            sock.Connect(end);

            if (!sock.Connected)
            {
                MessageBox.Show("Failed to connect!", "Failed");
            }

            this.lblServerName.Text = serverName;
            this.listener = new Thread(new ThreadStart(this.listenOnSocket));
            this.listener.Start();
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
                    string[] lines = line.Split('\n');
                    foreach (var l in lines)
                    {
                        if (l.Equals(""))
                        {
                            continue;
                        }
                        box.AppendText("[" + DateTime.Now.ToString("HH:mm") + "] " + l + "\n");
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
            this.registered = false;
        }

    }
}
