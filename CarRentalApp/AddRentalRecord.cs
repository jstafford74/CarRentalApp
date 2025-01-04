using System;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class AddRentalRecord : Form
    {
        private readonly car_rentalEntities car_RentalEntities;
        public AddRentalRecord()
        {
            InitializeComponent();
            car_RentalEntities = new car_rentalEntities();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                string customerName = tbCustomerName.Text;
                string dateIn = dtpDateRented.Value.ToShortDateString();
                string dateOut = dtpDateReturned.Value.ToShortDateString();
                double cost = Convert.ToDouble(tbCost.Text);
                var carType = cbCarType.Text;
                var isValid = true;
                var errorMessage = "Errors: \n\r";

                if (string.IsNullOrWhiteSpace(customerName) || string.IsNullOrWhiteSpace(carType))
                {
                    isValid = false;
                    errorMessage += "Please enter a valid name and car type.\n\r";
                }

                if (dtpDateRented.Value > dtpDateReturned.Value)
                {
                    isValid = false;
                    errorMessage += "Invalid date selection.\n\r";
                }

                if (isValid)
                {
                    // Save data then display the message
                    var car_RentalRecord = new car_rentals()
                    {
                        customer_name = customerName,
                        date_rented = dtpDateRented.Value,
                        date_returned = dtpDateReturned.Value,
                        cost = (decimal) cost,
                        car_type_id = (int) cbCarType.SelectedValue
                    };

                    car_RentalEntities.car_rentals.Add(car_RentalRecord);
                    car_RentalEntities.SaveChanges();

                    MessageBox.Show($"Customer Name: {customerName}\n\r" +
                    $"Date Rented: {dateIn}\n\r" +
                    $"Date Returned: {dateOut}\n\r" +
                    $"Total Cost: {cost}\n\r" +
                    $"Car Type: {carType}\n\r" +
                    $"Thank You For Your Business!");
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
            var cars = car_RentalEntities.car_types.ToList();
            cbCarType.DisplayMember = "name";
            cbCarType.ValueMember = "id";
            cbCarType.DataSource = cars;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }


}

