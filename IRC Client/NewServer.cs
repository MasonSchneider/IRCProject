using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IRC_Client
{
    public partial class NewServer : Form
    {
        private bool editing = false;
        private String editServer = null;
        public NewServer()
        {
            this.editing = false;
            InitializeComponent();
        }
        public NewServer(string serverName, string nick, string port, string pass, string rooms, string real, string user)
        {
            this.editing = true;
            InitializeComponent();
            this.txtNick.Text = nick;
            this.txtPass.Text = pass;
            this.txtPort.Text = port;
            this.txtReal.Text = real;
            this.txtRooms.Text = rooms;
            this.txtServer.Text = serverName;
            this.txtUsername.Text = user;
            this.editServer = string.Join(":", nick, serverName, port, pass, rooms, real, user);

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string nick = txtNick.Text;
            string server = txtServer.Text;
            string port = txtPort.Text;
            string pass = txtPass.Text;
            string rooms = txtRooms.Text;
            string real = txtReal.Text;
            string user = txtUsername.Text;

            if (nick.Length == 0 || server.Length == 0 || port.Length == 0 || real.Length == 0 || user.Length == 0)
            {
                MessageBox.Show("Nickname, Server, Port, Real Name, and Username are required.", "Fields left blank");
                return;
            }

            if (pass.Length == 0)
            {
                pass = "**";
            }

            string curr = (string)Properties.Settings.Default["servers"];

            string newServer = string.Join(":", nick, server, port, pass, rooms, real, user);
            if (this.editing)
            {
                System.Diagnostics.Debug.WriteLine("Editing: "+newServer);
                curr = curr.Replace(this.editServer, newServer);
            }
            else
            {
                if (curr.Length > 0)
                {
                    curr += ",";
                }
                if (!curr.Contains(nick + ":" + server))
                {
                    curr += newServer;
                }
                else
                {
                    MessageBox.Show("This server and nickname is already set!", "Already exists");
                    return;
                }
            }
            Properties.Settings.Default["servers"] = curr;
            Properties.Settings.Default.Save();

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
