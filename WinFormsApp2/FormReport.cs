using AgentActivitiesTracker;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Agent_Activities_Tracker
{
    public partial class FormReport : Form
    {
        private readonly BsonDocument _caseDoc;
        private readonly System.Collections.Generic.List<BsonDocument> _actions;

        private readonly IMongoCollection<BsonDocument> clientsCollection;
        private readonly IMongoCollection<BsonDocument> employeesCollection;

        private Label lblHeader;
        private Label lblClient;
        private Label lblAgent;
        private Label lblSupervisor;
        private Label lblPriority;
        private Label lblOrigin;
        private Label lblTags;
        private Label lblStatus;
        private Label lblCreated;
        private Label lblClosed;
        private Label lblDescription;
        private DataGridView dgvActions;
        private Label lblSummary;

        private Button btnBack;
        private Button btnQuit;

        public FormReport(BsonDocument caseDoc, System.Collections.Generic.List<BsonDocument> actions)
        {
            _caseDoc = caseDoc;
            _actions = actions;

            var db = AppState.Db;
            clientsCollection = db.GetCollection<BsonDocument>("clients");
            employeesCollection = db.GetCollection<BsonDocument>("employees");

            InitializeComponent();
            LoadReport();
        }

        private void InitializeComponent()
        {
            WindowState = FormWindowState.Maximized;
            Text = "Case Report";
            Font = new Font("Segoe UI", 11);
            BackColor = Color.White;

            var root = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                AutoSize = true,
                Padding = new Padding(25),
            };
            Controls.Add(root);

            var headerBar = new FlowLayoutPanel { Dock = DockStyle.Top, FlowDirection = FlowDirection.LeftToRight, Height = 40 };
            btnBack = new Button { Text = "Back", Width = 100, Height = 40 };
            btnQuit = new Button { Text = "Quit", Width = 100 , Height = 40 };
            btnBack.Click += BtnBack_Click;
            btnQuit.Click += (s, e) => Application.Exit();
            headerBar.Controls.Add(btnBack);
            headerBar.Controls.Add(btnQuit);
            root.Controls.Add(headerBar);

            lblHeader = new Label()
            {
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                Dock = DockStyle.Top,
                AutoSize = true,
                Padding = new Padding(0, 0, 0, 20),
                TextAlign = ContentAlignment.MiddleCenter
            };
            root.Controls.Add(lblHeader);

            var descLabel = new Label() { Text = "Description:", Font = new Font("Segoe UI", 14, FontStyle.Bold), AutoSize = true, Padding = new Padding(0, 20, 0, 5) };
            root.Controls.Add(descLabel);

            lblDescription = new Label() { AutoSize = true, MaximumSize = new Size(1600, 0), Font = new Font("Segoe UI", 11), Padding = new Padding(0, 5, 0, 20) };
            root.Controls.Add(lblDescription);

            var infoGrid = new TableLayoutPanel() { ColumnCount = 2, AutoSize = true, Padding = new Padding(5), BackColor = Color.FromArgb(245, 245, 245) };
            infoGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            infoGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
            root.Controls.Add(infoGrid);

            lblClient = new Label() { AutoSize = true };
            lblAgent = new Label() { AutoSize = true };
            lblSupervisor = new Label() { AutoSize = true };
            infoGrid.Controls.Add(lblClient, 0, 0);
            infoGrid.Controls.Add(lblAgent, 0, 1);
            infoGrid.Controls.Add(lblSupervisor, 0, 2);

            lblPriority = new Label() { AutoSize = true };
            lblOrigin = new Label() { AutoSize = true };
            lblTags = new Label() { AutoSize = true };
            lblStatus = new Label() { AutoSize = true };
            lblCreated = new Label() { AutoSize = true };
            lblClosed = new Label() { AutoSize = true };

            infoGrid.Controls.Add(lblPriority, 1, 0);
            infoGrid.Controls.Add(lblOrigin, 1, 1);
            infoGrid.Controls.Add(lblTags, 1, 2);
            infoGrid.Controls.Add(lblStatus, 1, 3);
            infoGrid.Controls.Add(lblCreated, 1, 4);
            infoGrid.Controls.Add(lblClosed, 1, 5);

            root.Controls.Add(new Label { Text = "Actions", Font = new Font("Segoe UI", 16, FontStyle.Bold), AutoSize = true, Padding = new Padding(0, 25, 0, 10) });

            dgvActions = new DataGridView { Height = 300, Width = 1600, ReadOnly = true, AllowUserToAddRows = false, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells, AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells, BackgroundColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            dgvActions.EnableHeadersVisualStyles = false;
            dgvActions.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(230, 230, 230);
            dgvActions.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            dgvActions.RowsDefaultCellStyle.BackColor = Color.White;
            dgvActions.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

            root.Controls.Add(dgvActions);

            lblSummary = new Label() { Font = new Font("Segoe UI", 13, FontStyle.Bold), AutoSize = true, Padding = new Padding(0, 25, 0, 10) };
            root.Controls.Add(lblSummary);
        }

        private void LoadReport()
        {
            lblHeader.Text = _caseDoc.GetValue("title", "").ToString();
            lblDescription.Text = _caseDoc.GetValue("description", "").ToString();

            lblPriority.Text = $"Priority:    {_caseDoc.GetValue("priority", "")}";
            lblOrigin.Text = $"Origin:      {_caseDoc.GetValue("case_origin", "")}";
            lblStatus.Text = $"Status:      {_caseDoc.GetValue("status", "")}";

            if (_caseDoc.Contains("creation_date"))
            {
                lblCreated.Text = "Created:     " + _caseDoc["creation_date"].ToUniversalTime().ToString("yyyy-MM-dd HH:mm");
            }

            if (_caseDoc.Contains("closed_date"))
            {
                lblClosed.Text = "Closed:      " + _caseDoc["closed_date"].ToUniversalTime().ToString("yyyy-MM-dd HH:mm");
            }
            else
            {
                lblClosed.Text = "Closed:      -";
            }

            if (_caseDoc.Contains("tags"))
            {
                var tags = string.Join(", ", _caseDoc["tags"].AsBsonArray.Select(t => t.ToString()));
                lblTags.Text = $"Tags:        {tags}";
            }

            lblClient.Text = "Client:      " + ResolveName(_caseDoc.GetValue("client_id", "").ToString());
            lblAgent.Text = "Agent:       " + ResolveName(_caseDoc.GetValue("agent_id", "").ToString());
            lblSupervisor.Text = "Supervisor:  " + ResolveName(_caseDoc.GetValue("supervisor_id", "").ToString());

            var db = AppState.Db;
            var actionsCollection = db.GetCollection<BsonDocument>("actions");
            var caseId = _caseDoc.GetValue("case_id", "").ToString();

            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("case_id", caseId)),
                new BsonDocument("$group", new BsonDocument
                {
                    { "_id", BsonNull.Value },
                    { "total", new BsonDocument("$sum", 1) },
                    { "reviewed", new BsonDocument("$sum", new BsonDocument("$cond", new BsonArray { "$is_reviewed", 1, 0 })) },
                    { "duration", new BsonDocument("$sum", "$duration_minutes") }
                })
            };

            var summary = actionsCollection.Aggregate<BsonDocument>(pipeline).FirstOrDefault();

            int total = summary?["total"].AsInt32 ?? 0;
            int reviewed = summary?["reviewed"].AsInt32 ?? 0;
            int duration = summary?["duration"].AsInt32 ?? 0;

            var actions = actionsCollection.Find(new BsonDocument("case_id", caseId)).ToList();
            dgvActions.DataSource = actions.Select(a => new
            {
                Type = a.GetValue("action_type", "").ToString(),
                Description = a.GetValue("description", "").ToString(),
                Timestamp = a.Contains("timestamp")
                                ? a["timestamp"].ToUniversalTime().ToString("yyyy-MM-dd HH:mm")
                                : "",
                Duration = a.GetValue("duration_minutes", "").ToString(),
                Reviewed = a.GetValue("is_reviewed", "").ToString()
            }).ToList();

            lblSummary.Text = $"Total actions: {total}    |    Reviewed: {reviewed}    |    Total Time: {duration} min";
        }

        private string ResolveName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "";

            var c = clientsCollection.Find(Builders<BsonDocument>.Filter.Eq("client_id", id)).FirstOrDefault();
            if (c != null)
                return $"{c.GetValue("first_name", "")} {c.GetValue("last_name", "")}";

            var e = employeesCollection.Find(Builders<BsonDocument>.Filter.Eq("employee_id", id)).FirstOrDefault();
            if (e != null)
                return $"{e.GetValue("first_name", "")} {e.GetValue("last_name", "")}";

            return id;
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            this.Hide();

            var user = AppState.CurrentUser;
            if (user == null)
            {
                Application.Exit();
                return;
            }

            if (user.role?.ToLower() == "supervisor")
            {
                var sup = new ShowSupervisorForm();
                sup.Show();
                return;
            }

            if (user.role?.ToLower() == "agent")
            {
                if (AppState.Navigation.Count > 0)
                {
                    var prev = AppState.Navigation.Pop();
                    prev.Show();
                }
                else
                {
                    var agent = new ShowAgentForm(false);
                    agent.Show();
                }

                return;
            }

            Application.Exit();
        }
    }
}
