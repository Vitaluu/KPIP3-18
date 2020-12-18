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

namespace КурсовойПроект
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            er.Visible = false;
            string s = Properties.Settings.Default.Connection;
            var builder = new SqlConnectionStringBuilder(s);
            builder.UserID = textBox1.Text;
            builder.Password = textBox2.Text;
            SqlConnection conn = new SqlConnection(builder.ToString());
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "getNamer";
            cmd.Parameters.AddWithValue("@name", "");
            cmd.Parameters["@name"].Direction = ParameterDirection.Output;
            cmd.Parameters["@name"].Size = 40;
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                MainForm mf = new MainForm(conn, cmd.Parameters["@name"].Value.ToString());
                mf.Owner = this;
                mf.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                er.Visible = true;
                MessageBox.Show(ex.Message);
            }
        }

        private void StartForm_Load(object sender, EventArgs e)
        {

        }
    }
}
