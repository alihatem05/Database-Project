using AgentActivitiesTracker;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AgentActivitiesTracker
{
    [System.ComponentModel.DesignerCategory("")]
    public class ShowAgentForm : Form
    {
        private System.ComponentModel.IContainer components = null;

        private Panel panelCases;
        private DataGridView dgvCases;
        private Button btnBack;
        private Button btnQuit;
        private Button btnAddCase;

        private readonly CaseService _caseService;
        private List<Case> allCases;
        private Case selectedCase;
        private bool isSupervisor = false;

        public ShowAgentForm(bool isSupervisor = false)
        {
            InitializeComponent();
            this.isSupervisor = isSupervisor;
        }

        public ShowAgentForm()
        {
            InitializeComponent();
            _caseService = new CaseService();
        }

        private async void ShowAgentForm_Load(object sender, EventArgs e)
        {
            var user = AppState.CurrentUser;

            try
            {
                allCases = await _caseService.GetCasesByAgentOrSupervisorAsync(user.employee_id);

                if (allCases == null || allCases.Count == 0)
                {
                    MessageBox.Show("No cases found for your account.", "No Cases", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    DisplayCases(new List<Case>());
                    return;
                }

                DisplayCases(allCases);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load cases: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DisplayCases(List<Case> cases)
        {
            dgvCases.Columns.Clear();
            dgvCases.Rows.Clear();

            dgvCases.Columns.Add("Title", "Title");
            dgvCases.Columns.Add("Description", "Description");

            foreach (var c in cases)
            {
                dgvCases.Rows.Add(c.Title, c.Description);
            }

            if (dgvCases.Columns.Contains("Description"))
                dgvCases.Columns["Description"].DefaultCellStyle.WrapMode = DataGridViewTriState.True;

            dgvCases.AutoResizeRows();

            dgvCases.CellClick -= dgvCases_CellClick;
            dgvCases.CellClick += dgvCases_CellClick;
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

        private void BtnQuit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnAddCase_Click(object sender, EventArgs e)
        {
            AppState.Navigation.Push(this);
            var create = new CreateCaseForm();
            this.Hide();
            create.Show();
        }

        private void dgvCases_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || allCases == null || e.RowIndex >= allCases.Count) return;

            selectedCase = allCases[e.RowIndex];
            string caseId = selectedCase.CaseId;

            try
            {
                var db = AppState.Db;
                var caseCollection = db.GetCollection<BsonDocument>("cases");
                var filter = Builders<BsonDocument>.Filter.Eq("case_id", caseId);
                var doc = caseCollection.Find(filter).FirstOrDefault();

                if (doc == null)
                {
                    MessageBox.Show("Case not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string status = doc.GetValue("status", "").ToString().ToLower();

                if (status == "closed")
                {
                    var actionsCollection = db.GetCollection<BsonDocument>("actions");
                    var actions = actionsCollection.Find(filter).ToList();

                    AppState.Navigation.Push(this);
                    var report = new Agent_Activities_Tracker.FormReport(doc, actions);
                    this.Hide();
                    report.Show();
                }
                else
                {
                    AppState.Navigation.Push(this);
                    var view = new Agent_Activities_Tracker.FormViewCase(caseId);
                    this.Hide();
                    view.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open case: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReturnToLogin()
        {
            if (AppState.Navigation.Count > 0)
            {
                var prev = AppState.Navigation.Pop();
                this.Hide();
                prev.Show();
            }
            else
                Application.Exit();
        }

        private void InitializeComponent()
        {
            StartPosition = FormStartPosition.CenterScreen;
            ClientSize = new System.Drawing.Size(984, 481);
            Text = "My Cases";

            panelCases = new Panel { Location = new System.Drawing.Point(12, 12), Size = new System.Drawing.Size(960, 450) };
            dgvCases = new DataGridView
            {
                Location = new System.Drawing.Point(0, 0),
                Size = new System.Drawing.Size(960, 400),
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            panelCases.Controls.Add(dgvCases);

            btnBack = new Button { Text = "Back", Location = new System.Drawing.Point(20, 410), Size = new System.Drawing.Size(100, 40) };
            btnBack.Click += BtnBack_Click;
            panelCases.Controls.Add(btnBack);

            btnQuit = new Button { Text = "Quit", Location = new System.Drawing.Point(130, 410), Size = new System.Drawing.Size(100, 40) };
            btnQuit.Click += BtnQuit_Click;
            panelCases.Controls.Add(btnQuit);

            btnAddCase = new Button { Text = "Add Case", Location = new System.Drawing.Point(240, 410), Size = new System.Drawing.Size(100, 40) };
            btnAddCase.Click += BtnAddCase_Click;
            panelCases.Controls.Add(btnAddCase);

            Controls.Add(panelCases);

            Load += ShowAgentForm_Load;
        }
    }
}
