using AgentActivitiesTracker;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgentActivitiesTracker
{
    [System.ComponentModel.DesignerCategory("")]
    public class CreateClient : Form
    {
        public string CreatedClientId { get; private set; } = null;

        private Label lblFirstName;
        private Label lblLastName;
        private Label lblPhone;
        private Label lblEmail;
        private Label lblDOB;
        private Label lblSex;
        private Label lblStreet;
        private Label lblCity;
        private Label lblCountry;

        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtPhone;
        private TextBox txtEmail;
        private DateTimePicker dtDOB;
        private ComboBox cmbSex;
        private TextBox txtStreet;
        private TextBox txtCity;
        private TextBox txtCountry;

        private Button btnSave;
        private Button btnBack;
        private Button btnQuit;

        public CreateClient()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            StartPosition = FormStartPosition.CenterScreen;

            lblFirstName = new Label();
            lblLastName = new Label();
            lblPhone = new Label();
            lblEmail = new Label();
            lblDOB = new Label();
            lblSex = new Label();
            lblStreet = new Label();
            lblCity = new Label();
            lblCountry = new Label();

            txtFirstName = new TextBox();
            txtLastName = new TextBox();
            txtPhone = new TextBox();
            txtEmail = new TextBox();

            dtDOB = new DateTimePicker();
            cmbSex = new ComboBox();

            txtStreet = new TextBox();
            txtCity = new TextBox();
            txtCountry = new TextBox();

            btnSave = new Button();
            btnBack = new Button();
            btnQuit = new Button();

            SuspendLayout();

            lblFirstName.AutoSize = true;
            lblFirstName.Location = new Point(30, 25);
            lblFirstName.Size = new Size(93, 25);
            lblFirstName.Text = "First Name";

            txtFirstName.Location = new Point(180, 22);
            txtFirstName.Size = new Size(300, 31);

            lblLastName.AutoSize = true;
            lblLastName.Location = new Point(30, 75);
            lblLastName.Text = "Last Name";

            txtLastName.Location = new Point(180, 72);
            txtLastName.Size = new Size(300, 31);

            lblPhone.AutoSize = true;
            lblPhone.Location = new Point(30, 125);
            lblPhone.Text = "Phone Number";

            txtPhone.Location = new Point(180, 122);
            txtPhone.Size = new Size(300, 31);

            lblEmail.AutoSize = true;
            lblEmail.Location = new Point(30, 175);
            lblEmail.Text = "Email";

            txtEmail.Location = new Point(180, 172);
            txtEmail.Size = new Size(300, 31);

            lblDOB.AutoSize = true;
            lblDOB.Location = new Point(30, 225);
            lblDOB.Text = "Date of Birth";

            dtDOB.Location = new Point(180, 220);
            dtDOB.Size = new Size(300, 31);
            dtDOB.Format = DateTimePickerFormat.Short;

            lblSex.AutoSize = true;
            lblSex.Location = new Point(30, 275);
            lblSex.Text = "Sex";

            cmbSex.Location = new Point(180, 272);
            cmbSex.Size = new Size(180, 33);
            cmbSex.Items.AddRange(new string[] { "M", "F" });

            lblStreet.AutoSize = true;
            lblStreet.Location = new Point(30, 325);
            lblStreet.Text = "Street";

            txtStreet.Location = new Point(180, 322);
            txtStreet.Size = new Size(300, 31);

            lblCity.AutoSize = true;
            lblCity.Location = new Point(30, 375);
            lblCity.Text = "City";

            txtCity.Location = new Point(180, 372);
            txtCity.Size = new Size(300, 31);

            lblCountry.AutoSize = true;
            lblCountry.Location = new Point(30, 425);
            lblCountry.Text = "Country";

            txtCountry.Location = new Point(180, 422);
            txtCountry.Size = new Size(300, 31);

            btnSave.Location = new Point(180, 480);
            btnSave.Size = new Size(200, 45);
            btnSave.Text = "Save Client";
            btnSave.Click += new EventHandler(this.btnSaveClient_Click);

            btnBack.Location = new Point(30, 480);
            btnBack.Size = new Size(120, 45);
            btnBack.Text = "Back";
            btnBack.Click += BtnBack_Click;

            btnQuit.Location = new Point(400, 480);
            btnQuit.Size = new Size(120, 45);
            btnQuit.Text = "Quit";
            btnQuit.Click += (s, e) => Application.Exit();

            ClientSize = new Size(550, 550);
            Controls.AddRange(new Control[] {
                lblFirstName, txtFirstName, lblLastName, txtLastName, lblPhone, txtPhone,
                lblEmail, txtEmail, lblDOB, dtDOB, lblSex, cmbSex, lblStreet, txtStreet,
                lblCity, txtCity, lblCountry, txtCountry, btnSave, btnBack, btnQuit
            });
            Name = "CreateClient";
            Text = "Add New Client";
            ResumeLayout(false);
            PerformLayout();
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            if (AppState.Navigation.Count > 0)
            {
                var prev = AppState.Navigation.Pop();
                this.Hide();
                prev.Show();
            }
            else Application.Exit();
        }

        private void btnSaveClient_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(cmbSex.Text) ||
                string.IsNullOrWhiteSpace(txtStreet.Text) ||
                string.IsNullOrWhiteSpace(txtCity.Text) ||
                string.IsNullOrWhiteSpace(txtCountry.Text))
                {
                    MessageBox.Show("Please fill in all fields before saving.",
                        "Missing Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                var db = AppState.Db;
                var clientCollection = db.GetCollection<BsonDocument>("clients");

                var lastClient = clientCollection.Find(FilterDefinition<BsonDocument>.Empty)
                    .Sort(Builders<BsonDocument>.Sort.Descending("client_id"))
                    .Limit(1)
                    .FirstOrDefault();

                int nextNumber = 1;
                if (lastClient != null)
                {
                    var id = lastClient.GetValue("client_id").AsString;
                    if (id.StartsWith("C") && int.TryParse(id.Substring(1), out int num))
                        nextNumber = num + 1;
                }

                string newClientId = "C" + nextNumber.ToString("D3");

                var newClient = new BsonDocument
                {
                    { "client_id", newClientId },
                    { "first_name", txtFirstName.Text },
                    { "last_name", txtLastName.Text },
                    { "phone_number", txtPhone.Text },
                    { "date_of_birth", dtDOB.Value },
                    { "sex", cmbSex.Text },
                    { "email", txtEmail.Text },
                    {
                        "primary_address", new BsonDocument
                        {
                            { "street", txtStreet.Text },
                            { "city", txtCity.Text },
                            { "country", txtCountry.Text }
                        }
                    }
                };

                clientCollection.InsertOne(newClient);

                CreatedClientId = newClientId;

                MessageBox.Show($"Client created successfully! ID = {newClientId}",
                    "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (AppState.Navigation.Count > 0)
                {
                    var prev = AppState.Navigation.Pop();
                    if (prev is CreateCaseForm cform)
                    {
                        this.Hide();
                        cform.RefreshClients(newClientId);
                        cform.Show();
                    }
                    else
                    {
                        this.Hide();
                        prev.Show();
                    }
                }
                else
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error creating client:\nMake sure your inputs fit the validation schema.");
            }
        }
    }
}
