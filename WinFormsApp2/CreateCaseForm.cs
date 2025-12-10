using AgentActivitiesTracker;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

[System.ComponentModel.DesignerCategory("")]
public class CreateCaseForm : Form
{
    private Label lblTitle;
    private TextBox txtTitle;

    private Label lblDescription;
    private TextBox txtDescription;

    private Label lblPriority;
    private ComboBox cmbPriority;

    private Label lblOrigin;
    private ComboBox cmbOrigin;

    private Label lblClient;
    private ComboBox cmbClient;

    private Button btnCreateCase;
    private Button btnAddClient;
    private Button btnBack;
    private Button btnQuit;

    private readonly IMongoCollection<BsonDocument> casesCollection;
    private readonly IMongoCollection<BsonDocument> clientsCollection;
    private readonly IMongoCollection<BsonDocument> employeesCollection;

    public CreateCaseForm()
    {
        var db = AppState.Db;

        casesCollection = db.GetCollection<BsonDocument>("cases");
        clientsCollection = db.GetCollection<BsonDocument>("clients");
        employeesCollection = db.GetCollection<BsonDocument>("employees");

        BuildUI();
        LoadClients();
    }

    private void BuildUI()
    {
        Text = "Create New Case";
        StartPosition = FormStartPosition.CenterScreen;
        MaximumSize = new Size(750, 500);
        MinimumSize = new Size(750, 500);

        lblTitle = new Label { Text = "Case Title:", Left = 40, Top = 50, AutoSize = true };
        txtTitle = new TextBox { Left = 200, Top = 45, Width = 300 };

        lblDescription = new Label { Text = "Description:", Left = 40, Top = 100, AutoSize = true };
        txtDescription = new TextBox { Left = 200, Top = 95, Width = 500, Height = 80, Multiline = true };

        lblPriority = new Label { Text = "Priority:", Left = 40, Top = 200, AutoSize = true };
        cmbPriority = new ComboBox
        {
            Left = 200,
            Top = 195,
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Items = { "Low", "Medium", "High", "Critical" }
        };

        lblOrigin = new Label { Text = "Case Origin:", Left = 40, Top = 250, AutoSize = true };
        cmbOrigin = new ComboBox
        {
            Left = 200,
            Top = 245,
            Width = 200,
            DropDownStyle = ComboBoxStyle.DropDownList,
            Items = { "Phone Call", "Email", "Walk-In", "Portal" }
        };

        lblClient = new Label { Text = "Client:", Left = 40, Top = 300, AutoSize = true };
        cmbClient = new ComboBox
        {
            Left = 200,
            Top = 295,
            Width = 300,
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        btnAddClient = new Button
        {
            Text = "Add Client",
            Left = 520,
            Top = 295,
            Width = 120,
            Height = 35
        };
        btnAddClient.Click += BtnAddClient_Click;

        btnCreateCase = new Button
        {
            Text = "Create Case",
            Left = 320,
            Top = 380,
            Width = 120,
            Height = 35
        };
        btnCreateCase.Click += BtnCreateCase_Click;

        btnBack = new Button
        {
            Text = "Back",
            Left = 40,
            Top = 380,
            Width = 120,
            Height = 35
        };
        btnBack.Click += BtnBack_Click;

        btnQuit = new Button
        {
            Text = "Quit",
            Left = 180,
            Top = 380,
            Width = 120,
            Height = 35
        };
        btnQuit.Click += (s, e) => Application.Exit();

        Controls.AddRange(new Control[]
        {
            lblTitle, txtTitle,
            lblDescription, txtDescription,
            lblPriority, cmbPriority,
            lblOrigin, cmbOrigin,
            lblClient, cmbClient,
            btnAddClient,
            btnCreateCase,
            btnBack, btnQuit
        });
    }
    public void RefreshClients(string newClientId)
    {
        LoadClients();

        for (int i = 0; i < cmbClient.Items.Count; i++)
        {
            if (cmbClient.Items[i].ToString().StartsWith(newClientId))
            {
                cmbClient.SelectedIndex = i;
                break;
            }
        }
    }

    private void LoadClients()
    {
        cmbClient.Items.Clear();

        var list = clientsCollection.Find(new BsonDocument()).ToList();

        foreach (var c in list)
        {
            string id = c.GetValue("client_id", "").ToString();
            string first = c.GetValue("first_name", "").ToString();
            string last = c.GetValue("last_name", "").ToString();
            cmbClient.Items.Add($"{id} - {first} {last}");
        }
    }

    private void BtnAddClient_Click(object sender, EventArgs e)
    {
        AppState.Navigation.Push(this);

        var form = new CreateClient();
        this.Hide();
        form.Show();
    }

    private void BtnBack_Click(object sender, EventArgs e)
    {
        if (AppState.Navigation.Count > 0)
        {
            var prev = AppState.Navigation.Pop();
            this.Hide();
            prev.Show();
        }
        else
        {
            Application.Exit();
        }
    }

    private string GetRandomSupervisorId()
    {
        var supervisors = employeesCollection
            .Find(Builders<BsonDocument>.Filter.Eq("role", "supervisor"))
            .ToList();

        if (supervisors.Count == 0)
            throw new Exception("No supervisors available.");

        var rnd = new Random();
        return supervisors[rnd.Next(supervisors.Count)]["employee_id"].AsString;
    }

    private string GenerateCaseId()
    {
        var pipeline = new[]
        {
            new BsonDocument("$project", new BsonDocument
            {
                { "num", new BsonDocument("$toInt", new BsonDocument("$substr", new BsonArray { "$case_id", 1, -1 })) }
            }),
            new BsonDocument("$sort", new BsonDocument("num", -1)),
            new BsonDocument("$limit", 1)
        };

        var last = casesCollection.Aggregate<BsonDocument>(pipeline).FirstOrDefault();
        int next = (last != null && last.Contains("num")) ? last["num"].ToInt32() + 1 : 1;

        return "K" + next.ToString("000");
    }

    private async void BtnCreateCase_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
            string.IsNullOrWhiteSpace(txtDescription.Text) ||
            cmbPriority.SelectedIndex == -1 ||
            cmbOrigin.SelectedIndex == -1 ||
            cmbClient.SelectedIndex == -1)
        {
            MessageBox.Show("Please fill in all fields before saving.",
                        "Missing Information",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
            return;
        }

        string selectedClient = cmbClient.SelectedItem.ToString();
        string clientId = selectedClient.Split('-')[0].Trim();

        string supervisorId = GetRandomSupervisorId();

        var doc = new BsonDocument
        {
            { "case_id", GenerateCaseId() },
            { "title", txtTitle.Text.Trim() },
            { "description", txtDescription.Text.Trim() },
            { "priority", cmbPriority.Text },
            { "status", "open" },
            { "case_origin", cmbOrigin.Text },
            { "creation_date", DateTime.UtcNow },
            { "tags", new BsonArray() },
            { "client_id", clientId },
            { "agent_id", AppState.CurrentUser.employee_id },
            { "supervisor_id", supervisorId }
        };

        await casesCollection.InsertOneAsync(doc);

        MessageBox.Show("Case Created Successfully!");

        if (AppState.Navigation.Count > 0)
        {
            var prev = AppState.Navigation.Pop();
            this.Hide();
            prev.Show();
        }
        else
        {
            Application.Exit();
        }
    }
}
