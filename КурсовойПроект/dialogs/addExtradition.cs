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
    public partial class addExtradition : Form
    {
        SqlConnection conn;
        int libCode;
        public addExtradition(SqlConnection cn, int l)
        {
            InitializeComponent();
            conn = cn;
            libCode = l;
        }

        private void addExtradition_Load(object sender, EventArgs e)
        {
            LoadForm();
        }

        public void LoadForm()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText =
                @"select a_id as id,
                CONCAT(surname,' ', name, ' ', middleName) as FIO
                from abonement
	                join readers
	                  on reader_id = r_id
                where getdate() between date_start and date_end
                order by a_id";
            DataTable dt = new DataTable();
            BindingSource bs1 = new BindingSource();
            try
            {
                dt.Load(cmd.ExecuteReader());
                bs1.DataSource = dt;
                dataGridView1.DataSource = bs1;
                textBox3.DataBindings.Clear();
                textBox6.DataBindings.Clear();
                textBox6.DataBindings.Add("Text", bs1, "id");
                textBox3.DataBindings.Add("Text", bs1, "FIO");
            }
            catch (Exception)
            {
                throw;
            }
            SqlCommand cmd1 = new SqlCommand();
            cmd1.Connection = conn;
            cmd1.CommandText = "loadBookInfo";
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@l_id", libCode);
            DataTable dt1 = new DataTable();
            BindingSource bs2 = new BindingSource();
            try
            {
                dt1.Load(cmd1.ExecuteReader());
                bs2.DataSource = dt1;
                dataGridView2.DataSource = bs2;
                textBox4.DataBindings.Clear();
                textBox5.DataBindings.Clear();
                textBox4.DataBindings.Add("Text", bs2, "book_id");
                textBox5.DataBindings.Add("Text", bs2, "book_name");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "extradBook";
            cmd.Parameters.AddWithValue("@bookID", textBox4.Text);
            cmd.Parameters.AddWithValue("@lib_id", libCode);
            cmd.Parameters.AddWithValue("@abonement", textBox6.Text);
            cmd.Parameters.AddWithValue("@quantity", numericUpDown1.Value);

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

        private void button4_Click(object sender, EventArgs e)
        {
            addNewAbonement ana = new addNewAbonement(conn, libCode);
            if (ana.ShowDialog() == DialogResult.OK)
            {
                LoadForm();
            }
        }
    }
}
