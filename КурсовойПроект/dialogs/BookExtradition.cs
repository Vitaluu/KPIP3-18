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
    public partial class BookExtradition : Form
    {
        int LibCode;
        SqlConnection conn;
        BindingSource bs;
        public BookExtradition(SqlConnection sqlConnection, int l_code)
        {
            LibCode = l_code;
            conn = sqlConnection;
            InitializeComponent();
        }

        private void BookExtradition_Load(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void LoadTable()
        {
            bs = new BindingSource();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = 
      @"select ex_id,
		concat(librarians.middleName,' ', librarians.name, ' ', librarians.surname) as [Выдал],
		concat(readers.middleName,' ', readers.name, ' ', readers.surname) as [Выдано],
		extraditions.date_start,
		quantity,
		is_active,
		books.book_id,
		book_name
        from extraditions
	        join librarians
	          on librarians.l_id = extraditions.l_id
	        join abonement
	          on abonement.a_id = extraditions.a_id
	        join readers
	          on readers.r_id = reader_id
	        join books 
	          on books.book_id = extraditions.book_id
        where is_active = 'y'";
            try
            {
                dt.Load(cmd.ExecuteReader());
                bs.DataSource = dt;
                bindingNavigator1.BindingSource = bs;
                dataGridView1.DataSource = bs;
                textBox1.DataBindings.Clear();
                textBox1.DataBindings.Add("Text", bs, "ex_id");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            addExtradition ae = new addExtradition(conn, LibCode);
            if (ae.ShowDialog() == DialogResult.OK)
            {
                LoadTable();
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "returnBook";
            cmd.Parameters.AddWithValue("@ex_id", textBox1.Text);
            try
            {
                if (MessageBox.Show("Подтверждение удаления", "Подтверждение", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    cmd.ExecuteNonQuery();
                    LoadTable();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
