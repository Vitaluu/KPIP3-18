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
    public partial class addAuthors : Form
    {
        SqlConnection conn;
        public addAuthors(SqlConnection c)
        {
            conn = c;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.CommandText = "addNewAuthor";
                cmd.Parameters.AddWithValue("@authorName", textBox1.Text);
                cmd.Parameters.AddWithValue("@birthdayYear", textBox2.Text);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка записи " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            this.Close();
        }
    }
}
