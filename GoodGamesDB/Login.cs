using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;

// The Login-Window will be implemented at a later stage of development
namespace GoodGamesDB
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Btn_Login_Click(object sender, EventArgs e)
        {
            this.Hide();
            Index f2 = new Index("pwd");
            f2.ShowDialog();
            Index.LoginStatus = true;
        }
    }
}
