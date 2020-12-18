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
    public partial class addNewAbonement : Form
    {
        int LibCode;
        SqlConnection conn;
        public addNewAbonement(SqlConnection c, int libCode)
        {
            InitializeComponent();
            conn = c;
            LibCode = libCode;
        }

        private void addNewAbonement_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from readers";
            DataTable dt = new DataTable();
            BindingSource bs = new BindingSource();
            try
            {
                dt.Load(cmd.ExecuteReader());
                bs.DataSource = dt;
                dataGridView1.DataSource = bs;
                textBox2.DataBindings.Clear();
                textBox2.DataBindings.Add("Text", bs, "r_id");

            }
            catch (Exception)
            {
                throw;
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            addReader ar = new addReader(conn);
            if (DialogResult.OK == ar.ShowDialog())
            {
                addNewAbonement_Load(sender, e);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = conn;
            cmd.CommandText = "createAbonement";
            cmd.Parameters.AddWithValue("@r_id", textBox2.Text);
            cmd.Parameters.AddWithValue("@lib_id", LibCode);
            try
            {
                cmd.ExecuteNonQuery();
                Close();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
