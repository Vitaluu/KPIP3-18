using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace КурсовойПроект.dialogs
{
    public partial class addReader : Form
    {
        SqlConnection conn;
        public addReader(SqlConnection c)
        {
            InitializeComponent();
            conn = c;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if ((textBox1.Text.Trim() != "" ||
                textBox2.Text.Trim() != "" ||
                textBox3.Text.Trim() != "" ||
                textBox4.Text.Trim() != "") &&
                textBox4.Text.Trim().Length == 10)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "addReader";
                cmd.Parameters.AddWithValue("@sur", textBox1.Text);
                cmd.Parameters.AddWithValue("@name", textBox2.Text);
                cmd.Parameters.AddWithValue("@mid", textBox3.Text);
                cmd.Parameters.AddWithValue("@tel", int.Parse(textBox4.Text));
                try
                {
                    cmd.ExecuteNonQuery();
                    this.Close();
                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
    }
}
