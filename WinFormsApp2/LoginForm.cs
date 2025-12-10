using Agent_Activities_Tracker;
using AgentActivitiesTracker;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Drawing;
using System.Windows.Forms;

[System.ComponentModel.DesignerCategory("")]
public class LoginForm : Form
{
    private Label lblWelcome;
    private Label lblLogin;
    private Label lblEmail;
    private TextBox txtEmail;
    private Label lblPassword;
    private TextBox txtPassword;
    private Button btnLogin;
    private Button btnQuit;

    private readonly Database _db;

    public LoginForm()
    {
        _db = AppState.Db ?? new Database();
        BuildUI();
    }

    private void BuildUI()
    {
        Text = "Activities Tracker - Login";
        Width = 420;
        Height = 300;
        MinimumSize = new Size(420, 300);
        MaximumSize = new Size(420, 300);
        StartPosition = FormStartPosition.CenterScreen;

        lblWelcome = new Label
        {
            Text = "Welcome",
            Left = 20,
            Top = 10,
            AutoSize = true,
            Font = new Font("Segoe UI", 14F)
        };

        lblLogin = new Label
        {
            Text = "Please login to continue",
            Left = 20,
            Top = 50,
            AutoSize = true
        };

        lblEmail = new Label { Text = "Email:", Left = 20, Top = 90, AutoSize = true };
        txtEmail = new TextBox { Left = 110, Top = 86, Width = 260, Name = "txtEmail" };

        lblPassword = new Label { Text = "Password:", Left = 20, Top = 130, AutoSize = true };
        txtPassword = new TextBox { Left = 110, Top = 126, Width = 260, UseSystemPasswordChar = true, Name = "txtPassword" };

        btnLogin = new Button { Text = "Login", Left = 110, Top = 170, Width = 120, Height = 45 };
        btnLogin.Click += BtnLogin_Click;

        btnQuit = new Button { Text = "Quit", Left = 250, Top = 170, Width = 120, Height = 45 };
        btnQuit.Click += (s, e) => Application.Exit();

        Controls.AddRange(new Control[] {
            lblWelcome, lblLogin,
            lblEmail, txtEmail,
            lblPassword, txtPassword,
            btnLogin, btnQuit
        });
    }

    private void BtnLogin_Click(object sender, EventArgs e)
    {
        string email = txtEmail.Text.Trim();
        string password = txtPassword.Text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("Please enter both email and password.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("email", email)),
                new BsonDocument("$project", new BsonDocument
                {
                    { "employee_id", 1 },
                    { "first_name", 1 },
                    { "last_name", 1 },
                    { "phone_number", 1 },
                    { "email", 1 },
                    { "role", 1 },
                    { "password", 1 },
                    { "passwordMatches", new BsonDocument("$eq", new BsonArray { "$password", password }) }
                }),
                new BsonDocument("$match", new BsonDocument("passwordMatches", true))
            };

            var employeesCollection = _db.GetCollection<BsonDocument>("employees");
            var result = employeesCollection.Aggregate<BsonDocument>(pipeline).FirstOrDefault();

            if (result == null)
            {
                MessageBox.Show("Email or password is incorrect.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var employee = BsonSerializer.Deserialize<AgentActivitiesTracker.Employee>(result);

            AppState.CurrentUser = employee;

            // Push login to navigation stack
            AppState.Navigation.Push(this);

            if (employee.role.ToLower() == "supervisor")
            {
                AppState.Navigation.Push(this);
                var sup = new ShowSupervisorForm();
                this.Hide();
                sup.Show();
                return;
            }
            else
            {
                AppState.Navigation.Push(this);
                var agent = new ShowAgentForm();
                this.Hide();
                agent.Show();
                return;
            }



        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred while logging in:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
