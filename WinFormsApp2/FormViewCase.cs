using AgentActivitiesTracker;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Linq;
using System.Windows.Forms;

namespace Agent_Activities_Tracker
{
    public partial class FormViewCase : Form
    {
        private readonly IMongoCollection<BsonDocument> caseCollection;
        private readonly IMongoCollection<BsonDocument> actionsCollection;
        private readonly IMongoCollection<BsonDocument> clientsCollection;
        private readonly IMongoCollection<BsonDocument> employeesCollection;

        private Label lblCreated, lblClient, lblAgent, lblSupervisor;
        private TextBox txtTitle, txtDescription, txtPriority;
        private DataGridView dgvActions;
        private Button btnBack, btnQuit, btnAddAction, btnUpdate, btnEndCase;

        public FormViewCase()
        {
            InitializeComponent();

            var db = AppState.Db;
            caseCollection = db.GetCollection<BsonDocument>("cases");
            actionsCollection = db.GetCollection<BsonDocument>("actions");
            clientsCollection = db.GetCollection<BsonDocument>("clients");
            employeesCollection = db.GetCollection<BsonDocument>("employees");
        }

        public FormViewCase(string caseId) : this()
        {
            if (string.IsNullOrWhiteSpace(caseId))
            {
                MessageBox.Show("Invalid case ID.");
                return;
            }

            LoadCaseInternal(caseId);
        }

        private void LoadCaseInternal(string caseId)
        {
            var filter = Builders<BsonDocument>.Filter.Eq("case_id", caseId);
            var doc = caseCollection.Find(filter).FirstOrDefault();

            if (doc == null)
            {
                MessageBox.Show("Case not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (AppState.Navigation.Count > 0) { var p = AppState.Navigation.Pop(); this.Hide(); p.Show(); }
                return;
            }

            txtTitle.Text = doc.GetValue("title", "").ToString();
            txtDescription.Text = doc.GetValue("description", "").ToString();
            txtPriority.Text = doc.GetValue("priority", "").ToString();

            if (doc.Contains("creation_date"))
                lblCreated.Text = doc["creation_date"].ToUniversalTime().ToString("yyyy-MM-dd HH:mm");
            else lblCreated.Text = "";

            lblClient.Text = ResolveName(doc.GetValue("client_id", "").ToString());
            lblAgent.Text = ResolveName(doc.GetValue("agent_id", "").ToString());
            lblSupervisor.Text = ResolveName(doc.GetValue("supervisor_id", "").ToString());

            LoadActions(caseId);

            this.Tag = caseId;
        }

        private string ResolveName(string id)
        {
            if (string.IsNullOrEmpty(id)) return "";

            var c = clientsCollection.Find(Builders<BsonDocument>.Filter.Eq("client_id", id)).FirstOrDefault();
            if (c != null) return $"{c.GetValue("first_name", "")} {c.GetValue("last_name", "")}";

            var e = employeesCollection.Find(Builders<BsonDocument>.Filter.Eq("employee_id", id)).FirstOrDefault();
            if (e != null) return $"{e.GetValue("first_name", "")} {e.GetValue("last_name", "")}";

            return id;
        }

        private void LoadActions(string caseId)
        {
            var pipeline = new[]
            {
                new BsonDocument("$match", new BsonDocument("case_id", caseId)),
                new BsonDocument("$sort", new BsonDocument("timestamp", 1))
            };
            var list = actionsCollection.Aggregate<BsonDocument>(pipeline).ToList();

            dgvActions.DataSource = list.Select(a => new
            {
                Type = a.GetValue("action_type", ""),
                Description = a.GetValue("description", ""),
                Timestamp = a.Contains("timestamp") ? a["timestamp"].ToUniversalTime().ToString("yyyy-MM-dd HH:mm") : "",
                Duration = a.GetValue("duration_minutes", ""),
                Reviewed = a.GetValue("is_reviewed", "")
            }).ToList();
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

        private void BtnQuit_Click(object sender, EventArgs e) => Application.Exit();

        private void BtnAddAction_Click(object sender, EventArgs e)
        {
            using var form = new Agent_Activities_Tracker.FormAddAction();
            if (form.ShowDialog() != DialogResult.OK) return;

            string caseId = this.Tag?.ToString();
            if (string.IsNullOrEmpty(caseId))
            {
                MessageBox.Show("No case loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(form.ActionType) ||
                string.IsNullOrWhiteSpace(form.ActionDescription) ||
                form.ActionDuration <= 0)
            {
                MessageBox.Show("Please fill in all fields.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var newAction = new BsonDocument
                {
                    { "action_id", Guid.NewGuid().ToString("N") },
                    { "case_id", caseId },
                    { "action_type", form.ActionType.Trim().ToLower() },
                    { "description", form.ActionDescription.Trim() },
                    { "timestamp", DateTime.UtcNow },
                    { "duration_minutes", form.ActionDuration },
                    { "is_reviewed", false }
                };

                actionsCollection.InsertOne(newAction);

                caseCollection.UpdateOne(
                    Builders<BsonDocument>.Filter.Eq("case_id", caseId),
                    Builders<BsonDocument>.Update
                        .AddToSet("actions", newAction["action_id"])
                        .Set("last_modified", DateTime.UtcNow)
                );

                LoadActions(caseId);
                MessageBox.Show("Action added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (MongoWriteException mwx)
            {
                MessageBox.Show($"Database error: {mwx.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            string caseId = this.Tag?.ToString();
            if (string.IsNullOrEmpty(caseId))
            {
                MessageBox.Show("No case loaded.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(txtTitle.Text) ||
                string.IsNullOrWhiteSpace(txtDescription.Text) ||
                string.IsNullOrWhiteSpace(txtPriority.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Empty Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var allowedPriorities = new[] { "Low", "Medium", "High", "Critical" };
            if (!allowedPriorities.Any(p => p.Equals(txtPriority.Text.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                MessageBox.Show("Invalid priority. Allowed: Low, Medium, High.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var update = Builders<BsonDocument>.Update
                .Set("title", txtTitle.Text.Trim())
                .Set("description", txtDescription.Text.Trim())
                .Set("priority", txtPriority.Text.Trim().ToLower())
                .Set("last_modified", DateTime.UtcNow);

            try
            {
                var result = caseCollection.UpdateOne(
                    Builders<BsonDocument>.Filter.Eq("case_id", caseId),
                    update
                );

                if (result.MatchedCount == 0)
                {
                    MessageBox.Show("Case not found or ID is invalid.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Case updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Database error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        private async void BtnEndCase_Click(object sender, EventArgs e)
        {
            string caseId = this.Tag?.ToString();
            if (string.IsNullOrEmpty(caseId))
            {
                MessageBox.Show("No case loaded.");
                return;
            }

            try
            {
                var service = new CaseService();
                await service.CloseCaseAsync(caseId);

                MessageBox.Show("Case closed successfully.");

                var filter = Builders<BsonDocument>.Filter.Eq("case_id", caseId);
                var doc = caseCollection.Find(filter).FirstOrDefault();
                var actions = actionsCollection.Find(filter)
                                              .Sort(Builders<BsonDocument>.Sort.Ascending("timestamp"))
                                              .ToList();

                var report = new FormReport(doc, actions);
                report.Show();
                this.Hide();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Cannot Close Case");
            }
        }




        private void InitializeComponent()
        {
            WindowState = FormWindowState.Maximized;
            Text = "Case Viewer";
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

            var panelDetails = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 2, Padding = new Padding(0, 0, 20, 0) };
            panelDetails.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panelDetails.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));

            void AddField(string label, Control ctrl)
            {
                panelDetails.Controls.Add(new Label { Text = label, Anchor = AnchorStyles.Right, AutoSize = true, Margin = new Padding(0, 8, 10, 8) });
                ctrl.Dock = DockStyle.Fill; ctrl.Margin = new Padding(0, 8, 0, 8); panelDetails.Controls.Add(ctrl);
            }

            txtTitle = new TextBox();
            txtDescription = new TextBox { Multiline = true, Height = 70 };
            txtPriority = new TextBox();
            lblClient = new Label { BorderStyle = BorderStyle.FixedSingle, AutoSize = false, Height = 28 };
            lblAgent = new Label { BorderStyle = BorderStyle.FixedSingle, AutoSize = false, Height = 28 };
            lblSupervisor = new Label { BorderStyle = BorderStyle.FixedSingle, AutoSize = false, Height = 28 };
            lblCreated = new Label { BorderStyle = BorderStyle.FixedSingle, AutoSize = false, Height = 28 };

            AddField("Title:", txtTitle);
            AddField("Description:", txtDescription);
            AddField("Priority:", txtPriority);
            AddField("Client:", lblClient);
            AddField("Agent:", lblAgent);
            AddField("Supervisor:", lblSupervisor);
            AddField("Created:", lblCreated);

            root.Controls.Add(panelDetails, 0, 0);

            dgvActions = new DataGridView { Dock = DockStyle.Fill, AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill, ReadOnly = true, RowHeadersVisible = false };
            root.Controls.Add(dgvActions, 1, 0);

            var bottomBar = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft, Padding = new Padding(0, 10, 0, 10) };
            root.SetColumnSpan(bottomBar, 2);
            root.Controls.Add(bottomBar, 0, 1);

            btnAddAction = new Button { Text = "Add Action", Width = 150, Height = 40 };
            btnUpdate = new Button { Text = "Update Case", Width = 150, Height = 40 };
            btnEndCase = new Button { Text = "End Case", Width = 150, Height = 40 };
            btnBack = new Button { Text = "Back", Width = 100, Height = 40 };
            btnQuit = new Button { Text = "Quit", Width = 100, Height = 40 };

            btnAddAction.Click += BtnAddAction_Click;
            btnUpdate.Click += BtnUpdate_Click;
            btnEndCase.Click += BtnEndCase_Click;
            btnBack.Click += BtnBack_Click;
            btnQuit.Click += (s, e) => Application.Exit();

            bottomBar.Controls.Add(btnQuit);
            bottomBar.Controls.Add(btnBack);
            bottomBar.Controls.Add(btnEndCase);
            bottomBar.Controls.Add(btnUpdate);
            bottomBar.Controls.Add(btnAddAction);
        }
    }
}
