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
using КурсовойПроект.dialogs;

namespace КурсовойПроект
{
    public partial class addRemBook : Form
    {
        SqlConnection conn;
        int librarian_code;
        public addRemBook(SqlConnection conSting, int num)
        {
            conn = conSting;
            librarian_code = num;
            InitializeComponent();
        }

        private void addRemBook_Load(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void LoadTable()
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "getLocalLibraryBooks";
            cmd.Parameters.AddWithValue("workerID", librarian_code);
            try
            {
                DataSet ds = new DataSet();
                DataTable table = new DataTable();
                table.Load(cmd.ExecuteReader());
                BindingSource bS = new BindingSource();
                bS.DataSource = table;

                dataGridView1.DataSource = bS;
                bindingNavigator1.BindingSource = bS;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "connection error");
            }
        }
        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
            addBook ab = new addBook(conn, librarian_code);
            ab.Owner = this;
            if (DialogResult.OK == ab.ShowDialog())
            {
                LoadTable();
            }
        }

        private void bindingNavigatorDeleteItem_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            if (dataGridView1.CurrentCell != null)
            {
                try
                {
                    cmd.Connection = conn;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "removeBook";
                    cmd.Parameters.AddWithValue("@librian_id", librarian_code);
                    cmd.Parameters.AddWithValue("@book_id", dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value);
                    cmd.ExecuteNonQuery();
                    LoadTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка операции" + ex.Message, "Er", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                toolStripStatusLabel1.Text = "выберите книгу";
            }
        }
    }
}
