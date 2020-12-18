using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using КурсовойПроект.dialogs;

namespace КурсовойПроект
{
    public partial class MainForm : Form
    {
        SqlConnection conString;
        int LibCode;
        String LibName;
        public MainForm(SqlConnection connectString, String name)
        {
            try
            {
                Regex reg = new Regex("_");
                String[] s = reg.Split(name);
                conString = connectString;
                LibCode = int.Parse(s[0]);
                LibName = s[1];
                int i = 1;
                while (i < LibName.Length)
                {
                    if (LibName[i] > 'А' && LibName[i] < 'Я')
                    {
                        LibName = LibName.Substring(0, i) + " " + LibName.Substring(i, LibName.Length - i);
                        i++;
                    }
                    i++;
                }
            }
            catch (Exception)
            {
                throw;
            }

            InitializeComponent();
        }
        private void MainForm_Load(object sender, EventArgs e)
        {
            name.Text = LibName;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addRemBook arb = new addRemBook(conString, LibCode);
            arb.Show();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            BookExtradition bs = new BookExtradition(conString, LibCode);
            bs.Show();
        }
    }
}
