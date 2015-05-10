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
        public NewServer()
        {
            InitializeComponent();
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
            if (curr.Length > 0)
            {
                curr += ",";
            }
            string newServer = string.Join(":", nick, server, port, pass, rooms, real, user);
            if (!curr.Contains(nick + ":" + server)) {
                curr += newServer;
            } else {
                MessageBox.Show("This server and nickname is already set!", "Already exists");
                return;
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
