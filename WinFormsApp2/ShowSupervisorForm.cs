using AgentActivitiesTracker;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Agent_Activities_Tracker
{
    [System.ComponentModel.DesignerCategory("")]
    public class ShowSupervisorForm : Form
    {
        private readonly IMongoCollection<BsonDocument> casesCollection;
        private readonly IMongoCollection<BsonDocument> actionsCollection;

        private Panel panelCases;
        private DataGridView dgvCases;
        private Button btnBack;
        private Button btnQuit;

        private List<BsonDocument> caseDocs;

        public ShowSupervisorForm()
        {
            var db = AppState.Db ?? throw new InvalidOperationException("Database not initialized.");

            casesCollection = db.GetCollection<BsonDocument>("cases");
            actionsCollection = db.GetCollection<BsonDocument>("actions");

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new Size(984, 560);
            Text = "Supervisor - My Cases";
            Font = new Font("Segoe UI", 10);

            panelCases = new Panel
            {
                Location = new Point(12, 12),
                Size = new Size(960, 480),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            dgvCases = new DataGridView
            {
                Location = new Point(0, 0),
                Size = new Size(960, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right
            };

            dgvCases.CellDoubleClick += DgvCases_CellDoubleClick;
            panelCases.Controls.Add(dgvCases);

            btnBack = new Button
            {
                Text = "Back",
                Size = new Size(120, 40),
                Location = new Point(20, 420)
            };
            btnBack.Click += BtnBack_Click;
            panelCases.Controls.Add(btnBack);

            btnQuit = new Button
            {
                Text = "Quit",
                Size = new Size(120, 40),
                Location = new Point(160, 420)
            };
            btnQuit.Click += (s, e) => Application.Exit();
            panelCases.Controls.Add(btnQuit);

            Controls.Add(panelCases);

            Load += ShowSupervisorForm_Load;
        }

        private async void ShowSupervisorForm_Load(object sender, EventArgs e)
        {
            var user = AppState.CurrentUser;
            if (user == null)
            {
                MessageBox.Show("No user logged in.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReturnToLogin();
                return;
            }

            try
            {
                var filter = Builders<BsonDocument>.Filter.Eq("supervisor_id", user.employee_id);

                caseDocs = await casesCollection.Find(filter)
                    .Sort(Builders<BsonDocument>.Sort.Descending("creation_date"))
                    .ToListAsync();

                DisplayCases(caseDocs);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load cases:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayCases(List<BsonDocument> docs)
        {
            dgvCases.Columns.Clear();
            dgvCases.Rows.Clear();

            dgvCases.Columns.Add("CaseId", "CaseId");
            dgvCases.Columns.Add("Title", "Title");
            dgvCases.Columns.Add("Description", "Description");
            dgvCases.Columns.Add("Status", "Status");

            foreach (var c in docs)
            {
                string caseId = c.GetValue("case_id", "").ToString();
                string title = c.GetValue("title", "").ToString();
                string description = c.GetValue("description", "").ToString();
                string status = c.GetValue("status", "").ToString();

                dgvCases.Rows.Add(caseId, title, description, status);
            }

            dgvCases.Columns["CaseId"].Visible = false;

            dgvCases.AutoResizeRows();
        }

        private void DgvCases_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            string caseId = dgvCases.Rows[e.RowIndex].Cells["CaseId"].Value.ToString();
            string status = dgvCases.Rows[e.RowIndex].Cells["Status"].Value.ToString().ToLower();

            AppState.Navigation.Push(this);
            this.Hide();

            var filter = Builders<BsonDocument>.Filter.Eq("case_id", caseId);
            var doc = casesCollection.Find(filter).FirstOrDefault();
            var actions = actionsCollection.Find(filter).ToList();

            if (doc == null)
            {
                MessageBox.Show("Case not found.");
                ReturnBack();
                return;
            }

            if (status == "closed")
            {
                var report = new FormReport(doc, actions);
                report.Show();
            }
            else
            {
                var view = new SupervisorViewCaseForm(caseId);
                view.Show();
            }
        }

        private void BtnBack_Click(object sender, EventArgs e)
        {
            ReturnBack();
        }

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

        private void ReturnToLogin()
        {
            ReturnBack();
        }
    }
}
