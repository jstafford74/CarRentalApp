using System;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddEditRentalRecord : Form
    {
        private bool isEditMode;
        private ManageRentalRecords _manageRentalRecords;
        private readonly CarRentalEntities _db;
        public AddEditRentalRecord()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
            lblTitle.Text = "Add New Rental Record";
            Text = "Add New Rental Record";
            isEditMode = false;
            _manageRentalRecords = new ManageRentalRecords();
        }

        public AddEditRentalRecord(car_rentals record, ManageRentalRecords manageRentalRecords = null)
        {
            InitializeComponent();
            lblTitle.Text = "Edit Rental Record";
            Text = "Edit Rental Record";
            _manageRentalRecords = manageRentalRecords;

            if (record == null)
            {
                MessageBox.Show("Please ensure that you selected a valid record to edit.");
                Close();
            }
            else
            {
                isEditMode = true;
                _db = new CarRentalEntities();
                PopulateFields(record);

            }
        }

        private void PopulateFields(car_rentals record)
        {
            tbCustomerName.Text = record.customer_name;
            dtpDateRented.Value = (DateTime)record.date_rented;
            dtpDateReturned.Value = (DateTime)record.date_returned;
            tbCost.Text = record.cost.ToString();
            lblRecordId.Text = record.id.ToString();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                var dateIn = dtpDateReturned.Value;
                var dateOut = dtpDateRented.Value;
                double cost = Convert.ToDouble(tbCost.Text);

                var carType = cbCarType.Text;
                var isValid = true;
                var errorMessage = "Errors: \n\r";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "Please enter a valid name and car type.\n\r";
                }

                if (dateOut > dateIn)
                {
                    isValid = false;
                    errorMessage += "Invalid date selection.\n\r";
                }

                if (isValid)
                {
                    var rentalRecord = new car_rentals();

                    if (isEditMode)
                    {
                        var id = int.Parse(lblRecordId.Text);
                        rentalRecord = _db.car_rentals.FirstOrDefault(q => q.id == id);
                    }


                    rentalRecord.customer_name = customerName;
                    rentalRecord.date_rented = dtpDateRented.Value;
                    rentalRecord.date_returned = dtpDateReturned.Value;
                    rentalRecord.cost = (decimal)cost;
                    rentalRecord.car_type_id = (int)cbCarType.SelectedValue;

                    if (!isEditMode)
                    {

                        _db.car_rentals.Add(rentalRecord);
                    }
                    _db.SaveChanges();
                    _manageRentalRecords.PopulateGrid();

                    MessageBox.Show($"Customer Name: {customerName}\n\r" +
                    $"Date Rented: {dateIn}\n\r" +
                    $"Date Returned: {dateOut}\n\r" +
                    $"Total Cost: {cost}\n\r" +
                    $"Car Type: {carType}\n\r" +
                    $"Thank You For Your Business!");
                    Close();

                }
                else
                {
                    MessageBox.Show(errorMessage);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred:\n\r" +
                    $"{ex.Message}");

            }

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var cars = _db.car_types
                .Select(q => new
                {
                    Id = q.id,
                    Name = q.make.Trim() + " " + q.model.Trim()
                }).ToList();

            cbCarType.DataSource = cars;

            cbCarType.DisplayMember = "Name";
            cbCarType.ValueMember = "Id";

        }


    }


}

