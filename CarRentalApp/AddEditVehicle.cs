using System;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddEditVehicle : Form
    {
        private bool isEditMode;
        private ManageVehicleListing _manageVehicleListing;
        // declare db connection
        private readonly CarRentalEntities _db;
        public AddEditVehicle(ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Add New Vehicle";
            Text = "Add New Vehicle";
            isEditMode = false;
            _manageVehicleListing = new ManageVehicleListing();
            _db = new CarRentalEntities();
        }

        public AddEditVehicle(car_types carToEdit, ManageVehicleListing manageVehicleListing = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Vehicle";
            Text = "Edit Vehicle";
            _manageVehicleListing = manageVehicleListing;
            if (carToEdit == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFields(carToEdit);
            }


        }
        private void PopulateFields(car_types car)
        {

            lblId.Text = car.id.ToString();
            tbMake.Text = car.make;
            tbModel.Text = car.model;
            tbVin.Text = car.vin;
            tbYear.Text = car.year.ToString();
            tbLicensePlateNumber.Text = car.license_plate_number;

        }

        private void btnSaveChanges_Click(object sender, EventArgs e)
        {
            try {
                //Added Validation for make and model
                if (string.IsNullOrWhiteSpace(tbMake.Text) ||
                        string.IsNullOrWhiteSpace(tbModel.Text))
                {
                    MessageBox.Show("Please ensure that you provide a make and a model");
                }
                else
                {
                    if (isEditMode)
                    {
                        // Update Vehicle
                        var id = int.Parse(lblId.Text);
                        var car = _db.car_types.FirstOrDefault(q => q.id == id);
                        car.model = tbModel.Text;
                        car.make = tbMake.Text;
                        car.vin = tbVin.Text;
                        car.year = int.Parse(tbYear.Text);
                        car.license_plate_number = tbLicensePlateNumber.Text;
                    }
                    else
                    {
                        // Add New Vehicle
                        var newCar = new car_types
                        {
                            model = tbModel.Text,
                            make = tbMake.Text,
                            vin = tbVin.Text,
                            year = int.Parse(tbYear.Text),
                            license_plate_number = tbLicensePlateNumber.Text
                        };

                        _db.car_types.Add(newCar);
                    
                    }

                    _db.SaveChanges();
                    _manageVehicleListing.PopulateGrid();
                    MessageBox.Show("Operation Completed. Refresh Grid To See Changes");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnCxlChanges_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
