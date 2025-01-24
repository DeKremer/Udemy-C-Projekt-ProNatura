using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProNatur_BiomarktGMBH
{
    public partial class ProductsScreen : Form
    {
        private SqlConnection databaseConnection = new SqlConnection(@"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename=C:\Users\caspe\OneDrive\Dokumente\Pro- Natura Biomarkt GmbH.mdf;Integrated Security = True; Connect Timeout = 30");
        private int lastSelectetProduktKey;

       
        public ProductsScreen()
        {
            InitializeComponent();

            ShowProdukts();
        }



        private void btnProductSave_Click(object sender, EventArgs e)
        {

            if(textBoxProductName.Text == "" || textBoxProductBrand.Text == "" || textBoxProductPrice.Text == "" || comboBoxProductCategory.Text == "")
            {
                MessageBox.Show("Bitte füllen Sie alle Felder aus!");

                return;
            }
            // save product name in database
            string productName = textBoxProductName.Text;
            string productBrand = textBoxProductBrand.Text;
            string productCategory = comboBoxProductCategory.Text;
            string productPrice = (textBoxProductPrice.Text);

            string query = string.Format("INSERT INTO Products values('{0}','{1}','{2}','{3}')", productName, productBrand, productCategory, productPrice);
            ExecuteQuery(query);
           
            ClearAllFields();

            ShowProdukts();
        }

        private void btnProductEdit_Click(object sender, EventArgs e)
        {
            if (lastSelectetProduktKey == 0)
            {
                MessageBox.Show("Bitte wählen Sie ein Produkt aus!");
                return;
            }
            string productName = textBoxProductName.Text;
            string productBrand = textBoxProductBrand.Text;
            string productCategory = comboBoxProductCategory.Text;
            string productPrice = (textBoxProductPrice.Text);

            string query =string.Format("UPDATE Products SET Name='{0}',Brand='{1}', Category='{2}',Price='{3}' WHERE id={4}"
                , productName, productBrand, productCategory, productPrice, lastSelectetProduktKey  );

            ExecuteQuery(query);
            ShowProdukts();
        }

        private void btnProductClear_Click(object sender, EventArgs e)
        {
            ClearAllFields();
        }

        private void btnProductDelete_Click(object sender, EventArgs e)
        {
            if(lastSelectetProduktKey == 0)
            {
                MessageBox.Show("Bitte wählen Sie ein Produkt aus!");
                return;
            }
            string query = string.Format("DELETE FROM Products WHERE id={0};",lastSelectetProduktKey);
            ExecuteQuery(query);

            ClearAllFields();
            ShowProdukts();
        }
        private void ExecuteQuery(string query)
        {
            databaseConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(query, databaseConnection);
            sqlCommand.ExecuteNonQuery();
            databaseConnection.Close();
        }
        private void ClearAllFields()
        {
            textBoxProductName.Text = " ";
            textBoxProductBrand.Text = " ";
            textBoxProductPrice.Text = " ";
            comboBoxProductCategory.Text = " ";
            comboBoxProductCategory.SelectedItem = null;
        }
        private void ShowProdukts()
        {
            // Start
            databaseConnection.Open();

            string query = "SELECT * FROM Products";
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, databaseConnection);

            DataSet dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet);

            productsDGV.DataSource = dataSet.Tables[0];

            productsDGV.Columns[0].Visible = false;


            databaseConnection.Close();
        }

        private void productsDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBoxProductName.Text = productsDGV.SelectedRows[0].Cells[1].Value.ToString();
            textBoxProductBrand.Text = productsDGV.SelectedRows[0].Cells[2].Value.ToString();
            comboBoxProductCategory.Text = productsDGV.SelectedRows[0].Cells[3].Value.ToString();
            textBoxProductPrice.Text = productsDGV.SelectedRows[0].Cells[4].Value.ToString();

            lastSelectetProduktKey = Convert.ToInt32(productsDGV.SelectedRows[0].Cells[0].Value);

        }
    }
}
