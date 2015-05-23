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
    public partial class NewRoom : Form
    {
        public NewRoom()
        {
            InitializeComponent();
        }

        private void btnOkRoom_Click(object sender, EventArgs e)
        {
            string room = this.txtRoom.Text;
            Properties.Settings.Default["newRoom"] = room;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btnCancelRoom_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
