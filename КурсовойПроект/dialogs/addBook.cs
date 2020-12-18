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
    public partial class addBook : Form
    {
        SqlConnection conn;
        int LibCode;
        BindingSource authorsSource;
        DataTable AuthSelected;
        BindingSource AuthorsTableBinding = new BindingSource();
        public addBook(SqlConnection conn, int libCode)
        {
            this.conn = conn;
            LibCode = libCode;
            InitializeComponent();
        }

        private void addBook_Load(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "select * from authors";
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());
            AuthorsTableBinding.DataSource = dt;
            dataGridView1.DataSource = AuthorsTableBinding;

            // заполнение данными первого комбобокса
            cmd.CommandText = @"select stor_id
                                from storages
	                                join reading_room
	                                  on reading_room.lib_id = storages.lib_id
	                                join librarians
	                                  on room_id = r_id
                                where l_id = " + LibCode;
            DataTable combo1 = new DataTable();
            combo1.Load(cmd.ExecuteReader());
            comboBox1.Items.Clear();
            for (int i = 0; i < combo1.Rows.Count; i++)
            {
                comboBox1.Items.Add(combo1.Rows[i].ItemArray[0].ToString());
            }

            // заполнение данными второго комбобокса
            cmd.CommandText = @"select *
                                from book_types
                                order by type_id";
            DataTable combo2 = new DataTable();
            combo2.Load(cmd.ExecuteReader());
            BindingSource bs = new BindingSource();
            bs.DataSource = combo2;
            comboBox2.DataSource = bs;
            comboBox2.DisplayMember = "type_name";
            comboBox2.ValueMember = "type_id";

            // for listbox
            authorsSource = new BindingSource();
            AuthSelected = new DataTable();
            AuthSelected.Columns.Add("id");
            AuthSelected.Columns.Add("fio");
            AuthSelected.Columns.Add("year");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                authorsSource.RemoveAt(listBox1.SelectedIndex);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            
            for (int i = 0; i < dataGridView1.SelectedCells.Count; i++)
            {
                dataGridView1.Rows[dataGridView1.SelectedCells[i].RowIndex].Selected = true;
            }
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Selected)
                {
                    if (CheckContain(dataGridView1.Rows[i].Cells[0].Value.ToString()))
                    {
                        DataRow dr = AuthSelected.NewRow();
                        dr[0] = dataGridView1.Rows[i].Cells[0].Value;
                        dr[1] = dataGridView1.Rows[i].Cells[1].Value;
                        dr[2] = dataGridView1.Rows[i].Cells[2].Value;
                        AuthSelected.Rows.Add(dr);
                    }
                }
            }
            authorsSource.DataSource = AuthSelected;
            listBox1.DataSource = authorsSource;
            listBox1.ValueMember = "id";
            listBox1.DisplayMember = "fio";
        }

        private bool CheckContain(String id)
        {   
            if (AuthSelected.Rows.Count>0)
            {
                for (int i = 0; i < AuthSelected.Rows.Count; i++)
                {
                    if (id.Equals(AuthSelected.Rows[i].ItemArray[0].ToString()))
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AuthorsTableBinding.Filter = dataGridView1.Columns[0].HeaderText + " = " + "'" + textBox1.Text + "'";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AuthorsTableBinding.Filter = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            addAuthors addA = new addAuthors(conn);
            if (DialogResult.OK == addA.ShowDialog())
            {
                addBook_Load(sender, e);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.CommandText = "removeAuthor";
                cmd.Parameters.AddWithValue("@id", dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[0].Value);
                if (DialogResult.Yes == MessageBox.Show("Вы действительно хотите удалить автора \n" + dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells[1].Value + "?", "Подвверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                {
                    cmd.ExecuteNonQuery();
                    addBook_Load(sender, e);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка операции" + ex.Message, "Er", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = conn;
            cmd.CommandType = CommandType.StoredProcedure;
            try
            {
                cmd.CommandText = "addNewBook";
                cmd.Parameters.AddWithValue("@librianID", LibCode);
                cmd.Parameters.AddWithValue("@storage_id", comboBox1.SelectedItem);
                cmd.Parameters.AddWithValue("@stand", numericUpDown1.Value);
                cmd.Parameters.AddWithValue("@shelf", numericUpDown2.Value);
                cmd.Parameters.AddWithValue("@bookName", textBox4.Text);
                cmd.Parameters.AddWithValue("@deadLine", numericUpDown3.Value);
                cmd.Parameters.AddWithValue("@bookTypeID", comboBox2.SelectedValue);
                cmd.Parameters.AddWithValue("@readOnly", checkBox1.Checked ? '1' : '0');
                cmd.Parameters.AddWithValue("@BookID", 0);
                cmd.Parameters["@BookID"].Direction = ParameterDirection.Output;
                cmd.ExecuteNonQuery();
                int bId = int.Parse(cmd.Parameters["@BookID"].Value.ToString());
                SqlCommand cmd1 = new SqlCommand();
                cmd1.Connection = conn;
                cmd1.CommandType = CommandType.StoredProcedure;
                cmd1.CommandText = "addBookAuthor";
                cmd1.Parameters.AddWithValue("@b_id", bId);
                cmd1.Parameters.AddWithValue("@a_id", 0);
                for (int i = 0; i < listBox1.Items.Count; i++)
                {
                    listBox1.SetSelected(i, true);
                    cmd1.Parameters["@a_id"].Value = listBox1.SelectedValue;
                    cmd1.ExecuteNonQuery();
                }
                this.Close();
            }
            catch (Exception ex)
            {
               MessageBox.Show("Ошибка операции" + ex.Message, "Er", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
