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

namespace RestaurantApp
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Runtime.Remoting.Channels;

    public partial class Form1 : Form
    {
        int selectedCustomerId = 0;
        public Form1()
        {
            InitializeComponent();
        }
        private void LoadCustomer()
        {

            string cs = "Server=.;Database=RestaurantDB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand(
                "select CustomersId,FullName from Customers where IsActive=1", conn))
            {
                conn.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(dr);
                cmbCustomers.DisplayMember = "FullName";
                cmbCustomers.ValueMember = "CustomersId";
                cmbCustomers.DataSource = dt;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string cs = "Server=.;Database=RestaurantDB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(cs))
            {
                conn.Open();
                MessageBox.Show("connected Successfully");
                conn.Close();
            }
            LoadCustomer();
        }


        private void btnInsert_Click(object sender, EventArgs e)
        {

            string cs = "Server=.;DataBase=RestaurantDB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("InsertCustomer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FullName", txtName.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Addresss", txtAddresss.Text);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("customer is added");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            string cs = "Server=.;DataBase=RestaurantDB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("dbo.SearchCustomer", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Search", txtSearch.Text);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                selectedCustomerId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["CustomersId"].Value);
                txtName.Text = dataGridView1.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                txtPhone.Text = dataGridView1.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
                txtAddresss.Text = dataGridView1.Rows[e.RowIndex].Cells["Addresss"].Value.ToString();

            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string cs = "Server=.;DataBase=RestaurantDB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("UpdateCustomer", conn))
            {
                cmd.Parameters.AddWithValue("@CustomersId", selectedCustomerId);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@FullName", txtName.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Addresss", txtAddresss.Text);
                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("customer is Updated");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void cmbCustomers_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnSaveOrder_Click(object sender, EventArgs e)
        {
            if (cmbCustomers.SelectedValue == null)
            {
                MessageBox.Show("chose the Customer");
            }
            if (!decimal.TryParse(txtTotalAmount.Text, out decimal total))
            {
                MessageBox.Show("price is not valueable");
                return;
            }

            string cs = "Server=.;DataBase=RestaurantDB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("dbo.InsertOrder", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("CustomersId", cmbCustomers.SelectedValue);
                cmd.Parameters.AddWithValue("TotalAmount", total);
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("orders submit successfully");
                txtTotalAmount.Clear();
            }
        }

        private void btnReport_Click(object sender, EventArgs e)
        {

            string cs = "Server=.;DataBase=RestaurantDB;Trusted_Connection=True;";
            using (SqlConnection conn = new SqlConnection(cs))
            using (SqlCommand cmd = new SqlCommand("dbo.SalesReport", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
               conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            }
        }
    }
}
