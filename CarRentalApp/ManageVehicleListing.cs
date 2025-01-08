using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageVehicleListing : Form
    {
        // declare db connection
        private readonly CarRentalEntities _db;
        public ManageVehicleListing()
        {
            InitializeComponent();
            // establish db connection
            _db = new CarRentalEntities();
        }


        private void ManageVehicleListing_Load(object sender, EventArgs e)
        {
            try
            {
                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error: {ex.Message}");
            }

        }

        public void PopulateGrid()
        {
            // display data in gridview
            var cars = _db.car_types
                .Select(q => new
                {
                    Make = q.make,
                    Model = q.model,
                    VIN = q.vin,
                    Year = q.year,
                    LicensePlateNumber = q.license_plate_number,
                    q.id
                })
                .ToList();
            gvVehicleList.DataSource = cars;
            gvVehicleList.Columns[0].HeaderText = "MAKE";
            gvVehicleList.Columns[1].HeaderText = "MODEL";
            gvVehicleList.Columns[2].HeaderText = "VIN";
            gvVehicleList.Columns[3].HeaderText = "YEAR";
            gvVehicleList.Columns[4].HeaderText = "LICENSE PLATE NUMBER";
            //Hide the column for ID. Changed from the hard coded column value to the name, 
            // to make it more dynamic. 
            gvVehicleList.Columns["id"].Visible = false;
        }

        private void btnAddCar_Click(object sender, EventArgs e)
        {
            try
            {
                AddEditVehicle addEditVehicle = new AddEditVehicle(this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error: {ex.Message}");
            }


        }

        private void btnEditCar_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["id"].Value;

                // query database for record
                var car = _db.car_types.FirstOrDefault(q => q.id == id);

                // launch AddEditVehicle form with data
                AddEditVehicle addEditVehicle = new AddEditVehicle(car,this);
                addEditVehicle.MdiParent = this.MdiParent;
                addEditVehicle.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error: {ex.Message}");
            }


        }

        private void btnDeleteCar_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvVehicleList.SelectedRows[0].Cells["Id"].Value;

                //query database for record
                var car = _db.car_types.FirstOrDefault(q => q.id == id);

                DialogResult dr = MessageBox.Show("Are You Sure You Want To Delete This Record?",
                    "Delete", MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning);

                if (dr == DialogResult.Yes)
                {
                    //delete vehicle from table
                    _db.car_types.Remove(car);
                    _db.SaveChanges();
                }
                PopulateGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnRefresh_Click_1(object sender, EventArgs e)
        {
            //Simple Refresh Option
            PopulateGrid();
        }

        private void gvVehicleList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            gvVehicleList.SelectedCells.ToString();
        }
    }
}
