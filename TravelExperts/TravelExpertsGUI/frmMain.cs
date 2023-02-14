using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TravelExpertsData;
using static TravelExpertsData.DB_Utils;
using static TravelExpertsGUI.frmAddEditPackages;
using static TravelExpertsGUI.frmProd_SuppAddModify;

namespace TravelExpertsGUI
{
    public partial class frmMain : Form
    {
        private string? selectedTable = null;
        private int? selectedRecordID = null;
        private Package? selectedPackage = null;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (selectedTable == "Packages")
            {
                frmAddEditPackages secondForm = new frmAddEditPackages();
                frmAddEditPackages.isAdd = true;
                frmAddEditPackages.package = null;

                DialogResult result = secondForm.ShowDialog(); // display second form modal

                if (result == DialogResult.OK) // second form accepted new data
                {
                    // take customer from the second form and add to the database
                    selectedPackage = frmAddEditPackages.package;
                    try
                    {
                        using (TravelExpertsContext db = new TravelExpertsContext())
                        {
                            if (selectedPackage != null)
                            {
                                db.Packages.Add(selectedPackage);
                                db.SaveChanges();
                            }
                            DisplayPackage(); // display added package
                        }
                    }
                    catch (DbUpdateException ex)
                    {
                        string errorMessage = "";
                        var sqlException = (SqlException)ex.InnerException;
                        foreach (SqlError error in sqlException.Errors)
                        {
                            errorMessage += "ERROR CODE:  " + error.Number +
                                            " " + error.Message + "\n";
                        }
                        MessageBox.Show(errorMessage);
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show("Error while adding package" + ex.Message,
                            ex.GetType().ToString());
                    }
                } 
            }
            else if (selectedTable == "Products-Suppliers")
            {
                frmProd_SuppAddModify.isAdd = true;
                frmProd_SuppAddModify form = new frmProd_SuppAddModify();
                form.ShowDialog();
            }
            else if (selectedTable == "Products")
            {
                frmAddEditProduct.isAdd = true;
                frmAddEditProduct form = new frmAddEditProduct();
                form.ShowDialog();
            }
            else if (selectedTable == "Suppliers")
            {
                frmAddEditSupplier.isAdd = true;
                frmAddEditSupplier form = new frmAddEditSupplier();
                form.ShowDialog();
            }
        }

        private void DisplayPackage()
        {
            //throw new NotImplementedException();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //ClearControls();
        }

        //private void ClearControls()
        //{

        //}

        private void cmbTables_SelectedIndexChanged(object sender, EventArgs e)
        {
            while (cmbTables.SelectedItem.ToString()!=null)
            {
                selectedTable = cmbTables.SelectedItem.ToString();
                string? selection = cmbTables.SelectedItem.ToString();
                switch (selection)
                {
                    case "Products":
                        dgvMain.DataSource = GetAllProducts();
                        break;
                    case "Packages":
                        dgvMain.DataSource = GetAllPackages();
                        break;
                    case "Products-Suppliers":
                        dgvMain.DataSource = GetAllProductsSupplier();
                        ProductsSupplierFormat();
                        break;
                    case "Suppliers":
                        dgvMain.DataSource = GetAllSuppliers();
                        break;
                }
                break;
            }
            dgvMain.Refresh();
        }

        private void ProductsSupplierFormat()
        {
            dgvMain.AlternatingRowsDefaultCellStyle.BackColor = Color.PaleGoldenrod;

            dgvMain.Columns[0].Width = 132; // format ProdSupID column
            dgvMain.Columns[2].HeaderText = "Supplier Name";
            dgvMain.Columns[2].Width = 255; // format Supname column
            dgvMain.Columns[3].Width = 90; // format ProdID column
            dgvMain.Columns[4].HeaderText = "Product Name";
            dgvMain.Columns[4].Width = 140; // format the ProdName column
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (selectedTable == "Packages")
            {
                frmAddEditPackages.isAdd = false;
                frmAddEditPackages form = new frmAddEditPackages();
                form.ShowDialog();
            }
            else if (selectedTable == "Products-Suppliers")
            {
                frmProd_SuppAddModify.isAdd = false;
                frmProd_SuppAddModify form = new frmProd_SuppAddModify();
                form.ShowDialog();
            }
            else if (selectedTable == "Products")
            {
                frmAddEditProduct.isAdd = false;
                frmAddEditProduct form = new frmAddEditProduct();
                form.ShowDialog();
            }
            else if (selectedTable == "Suppliers")
            {
                frmAddEditSupplier.isAdd = false;
                frmAddEditSupplier form = new frmAddEditSupplier();
                form.ShowDialog();
            }
        }

        private object RecordSelector()
        {
            var selType = dgvMain.CurrentRow.DataBoundItem.GetType();
            if (selType == typeof(Product)) 
            {
                Product currentObject = (Product)dgvMain.CurrentRow.DataBoundItem;
            }
            else if (selType == typeof(Supplier))
            {
                Supplier currentObject = (Supplier)dgvMain.CurrentRow.DataBoundItem;
            }
            else if (selType == typeof(Package)) {
                Package currentObject = (Package)dgvMain.CurrentRow.DataBoundItem;
            }
            else if (selType == typeof(ProductSupplierInfo)) 
            {

            }
        }
    }//class

    public class SelectedRecord
    {
        public Product? Product { get; set;}
        public Supplier? Supplier { get; set; }
        public Package? Package { get; set; }

}