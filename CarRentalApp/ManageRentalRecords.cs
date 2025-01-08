using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class ManageRentalRecords : Form
    {
        // declare db connection
        private readonly CarRentalEntities _db;
        public ManageRentalRecords()
        {
            InitializeComponent();
            // establish db connection
            _db = new CarRentalEntities();
        }


        private void btnAddRecord_Click(object sender, EventArgs e)
        {
            //if (!Utils.FormIsOpen("AddEditRentalRecord"))
            //{
            var addRentalRecord = new AddEditRentalRecord()
            {
                MdiParent = this.MdiParent
            };

            addRentalRecord.Show();
            //}
        }

        private void btnEditRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["id"].Value;

                // query database for record
                var record = _db.car_rentals.FirstOrDefault(q => q.id == id);

                //// launch AddEditVehicle form with data
                AddEditRentalRecord addEditRecord = new AddEditRentalRecord(record,this);
                addEditRecord.MdiParent = this.MdiParent;
                addEditRecord.Show();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnDeleteRecord_Click(object sender, EventArgs e)
        {
            try
            {
                // get Id of selected row
                var id = (int)gvRecordList.SelectedRows[0].Cells["id"].Value;

                // query database for record
                var record = _db.car_types.FirstOrDefault(q => q.id == id);

                //delete vehicle from table
                _db.car_types.Remove(record);
                _db.SaveChanges();

                PopulateGrid();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void ManageRentalRecords_Load(object sender, EventArgs e)
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

            var records = _db.car_rentals
                .Select(q => new
                {
                    Customer = q.customer_name,
                    DateOut = q.date_rented,
                    DateIn = q.date_returned,
                    Cost = q.cost,
                    CarMake = q.car_types.make,
                    CarModel = q.car_types.model,
                    q.id
                })
                .ToList();
            gvRecordList.DataSource = records;
            gvRecordList.Columns["DateOut"].HeaderText = "Date Out";
            gvRecordList.Columns["DateIn"].HeaderText = "Date In";
            gvRecordList.Columns["CarMake"].HeaderText = "Make";
            gvRecordList.Columns["CarModel"].HeaderText = "Model";

            //Hide the column for ID. Changed from the hard coded column value to the name, 
            // to make it more dynamic. 
            gvRecordList.Columns["id"].Visible = false;
        }
    }
}
