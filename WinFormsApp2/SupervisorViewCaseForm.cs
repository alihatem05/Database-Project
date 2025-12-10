using AgentActivitiesTracker;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Agent_Activities_Tracker
{
    [System.ComponentModel.DesignerCategory("")]
    public partial class SupervisorViewCaseForm : Form
    {
        private readonly IMongoCollection<BsonDocument> caseCollection;
        private readonly IMongoCollection<BsonDocument> actionsCollection;
        private readonly IMongoCollection<BsonDocument> clientsCollection;
        private readonly IMongoCollection<BsonDocument> employeesCollection;

        private Label lblCreated, lblClient, lblAgent, lblSupervisor, txtTitle, txtDescription, txtPriority;
        private DataGridView dgvActions;
        private Button btnBack, btnQuit;

        public SupervisorViewCaseForm()
        {
            InitializeComponent();

            var db = AppState.Db;
            caseCollection = db.GetCollection<BsonDocument>("cases");
            actionsCollection = db.GetCollection<BsonDocument>("actions");
            clientsCollection = db.GetCollection<BsonDocument>("clients");
            employeesCollection = db.GetCollection<BsonDocument>("employees");
        }

        public SupervisorViewCaseForm(string caseId) : this()
        {
            LoadCaseInternal(caseId);
        }

        private void LoadCaseInternal(string caseId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("case_id", caseId);
            var doc = caseCollection.Find(filter).FirstOrDefault();

            if (doc == null)
            {
                MessageBox.Show("Case not found.");
                ReturnBack();
                return;
            }

            txtTitle.Text = doc.GetValue("title", "").ToString();
            txtDescription.Text = doc.GetValue("description", "").ToString();
            txtPriority.Text = doc.GetValue("priority", "").ToString();

            lblClient.Text = ResolveName(doc.GetValue("client_id", "").ToString());
            lblAgent.Text = ResolveName(doc.GetValue("agent_id", "").ToString());
            lblSupervisor.Text = ResolveName(doc.GetValue("supervisor_id", "").ToString());

            lblCreated.Text = doc.Contains("creation_date")
                ? doc["creation_date"].ToUniversalTime().ToString("yyyy-MM-dd HH:mm")
                : "";

            LoadActions(caseId);
            this.Tag = caseId;
        }

        private string ResolveName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "";

            var c = clientsCollection.Find(Builders<BsonDocument>.Filter.Eq("client_id", id)).FirstOrDefault();
            if (c != null) return $"{c["first_name"]} {c["last_name"]}";

            var e = employeesCollection.Find(Builders<BsonDocument>.Filter.Eq("employee_id", id)).FirstOrDefault();
            if (e != null) return $"{e["first_name"]} {e["last_name"]}";

            return id;
        }

        private void LoadActions(string caseId)
        {
            var docs = actionsCollection.Find(Builders<BsonDocument>.Filter.Eq("case_id", caseId))
                                       .Sort(Builders<BsonDocument>.Sort.Ascending("timestamp"))
                                       .ToList();

            dgvActions.Columns.Clear();
            dgvActions.Rows.Clear();
            dgvActions.AllowUserToAddRows = false;

            dgvActions.Columns.Add("ActionId", "ActionId");
            dgvActions.Columns["ActionId"].Visible = false;

            dgvActions.Columns.Add("Type", "Type");
            dgvActions.Columns.Add("Description", "Description");
            dgvActions.Columns.Add("Timestamp", "Timestamp");
            dgvActions.Columns.Add("Duration", "Duration");

            var chk = new DataGridViewCheckBoxColumn
            {
                Name = "Reviewed",
                HeaderText = "Reviewed",
                ReadOnly = false
            };
            dgvActions.Columns.Add(chk);

            foreach (var a in docs)
            {
                if (!a.Contains("action_id") || string.IsNullOrWhiteSpace(a["action_id"].ToString()))
                    continue; // ignore invalid rows

                string timestamp = a.Contains("timestamp")
                    ? a["timestamp"].ToUniversalTime().ToString("yyyy-MM-dd HH:mm")
                    : "";

                string duration = a.Contains("duration_minutes")
                    ? a["duration_minutes"].ToString()
                    : "";

                dgvActions.Rows.Add(
                    a["action_id"].ToString(),
                    a.GetValue("action_type", "").ToString(),
                    a.GetValue("description", "").ToString(),
                    timestamp,
                    duration,
                    a.GetValue("is_reviewed", false).ToBoolean()
                );
            }

            foreach (DataGridViewColumn col in dgvActions.Columns)
                col.ReadOnly = col.Name != "Reviewed";

            dgvActions.CellValueChanged += DgvActions_CellValueChanged;

            dgvActions.CurrentCellDirtyStateChanged += (s, e) =>
            {
                if (dgvActions.IsCurrentCellDirty)
                    dgvActions.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
        }

        private void DgvActions_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;
            if (e.RowIndex == dgvActions.NewRowIndex) return;

            if (dgvActions.Columns[e.ColumnIndex].Name != "Reviewed") return;

            var row = dgvActions.Rows[e.RowIndex];

            if (row.Cells["ActionId"].Value == null) return;

            string actionId = row.Cells["ActionId"].Value.ToString();
            bool newValue = row.Cells["Reviewed"].Value != null && (bool)row.Cells["Reviewed"].Value;

            var filter = Builders<BsonDocument>.Filter.Eq("action_id", actionId);
            var doc = actionsCollection.Find(filter).FirstOrDefault();
            if (doc == null) return;

            bool alreadyReviewed = doc.GetValue("is_reviewed", false).ToBoolean();

            if (alreadyReviewed && !newValue)
            {
                MessageBox.Show("Reviewed actions cannot be un-reviewed.");
                row.Cells["Reviewed"].Value = true;
                return;
            }

            if (!alreadyReviewed && newValue)
            {
                actionsCollection.UpdateOne(filter,
                    Builders<BsonDocument>.Update.Set("is_reviewed", true));
            }
        }

        private void BtnBack_Click(object sender, EventArgs e) => ReturnBack();

        private void ReturnBack()
        {
            if (AppState.Navigation.Count > 0)
            {
                var prev = AppState.Navigation.Pop();
                this.Hide();
                prev.Show();
            }
            else Application.Exit();
        }

        private void BtnQuit_Click(object sender, EventArgs e) => Application.Exit();

        private void InitializeComponent()
        {
            WindowState = FormWindowState.Maximized;
            Text = "Supervisor Case Viewer";
            Font = new System.Drawing.Font("Segoe UI", 10);

            var root = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Padding = new Padding(20)
            };
            Controls.Add(root);

            root.RowStyles.Add(new RowStyle(SizeType.Percent, 90));
            root.RowStyles.Add(new RowStyle(SizeType.Percent, 10));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40));
            root.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 60));

            var panel = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2 };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            void Add(string label, Control c)
            {
                panel.Controls.Add(new Label
                {
                    Text = label,
                    Anchor = AnchorStyles.Right,
                    AutoSize = true,
                    Margin = new Padding(0, 8, 10, 8)
                });
                c.Dock = DockStyle.Fill;
                c.Margin = new Padding(0, 8, 0, 8);
                panel.Controls.Add(c);
            }

            txtTitle = new Label();
            txtDescription = new Label { Height = 70 };
            txtPriority = new Label();
            lblClient = new Label { BorderStyle = BorderStyle.FixedSingle, Height = 28 };
            lblAgent = new Label { BorderStyle = BorderStyle.FixedSingle, Height = 28 };
            lblSupervisor = new Label { BorderStyle = BorderStyle.FixedSingle, Height = 28 };
            lblCreated = new Label { BorderStyle = BorderStyle.FixedSingle, Height = 28 };

            Add("Title:", txtTitle);
            Add("Description:", txtDescription);
            Add("Priority:", txtPriority);
            Add("Client:", lblClient);
            Add("Agent:", lblAgent);
            Add("Supervisor:", lblSupervisor);
            Add("Created:", lblCreated);

            root.Controls.Add(panel, 0, 0);

            dgvActions = new DataGridView
            {
                Dock = DockStyle.Fill,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowHeadersVisible = false
            };

            dgvActions.AllowUserToAddRows = false;
            root.Controls.Add(dgvActions, 1, 0);

            var bottom = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
            root.SetColumnSpan(bottom, 2);

            btnQuit = new Button { Text = "Quit", Width = 120, Height = 40 };
            btnBack = new Button { Text = "Back", Width = 120, Height = 40 };

            btnQuit.Click += BtnQuit_Click;
            btnBack.Click += BtnBack_Click;

            bottom.Controls.Add(btnQuit);
            bottom.Controls.Add(btnBack);

            root.Controls.Add(bottom, 0, 1);
        }
    }
}
