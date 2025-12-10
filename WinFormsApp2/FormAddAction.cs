using System;
using System.Windows.Forms;

namespace Agent_Activities_Tracker
{
    public partial class FormAddAction : Form
    {
        public string ActionType => txtType.Text.Trim();
        public string ActionDescription => txtDescription.Text.Trim();
        public int ActionDuration => (int)numDuration.Value;

        private TextBox txtType;
        private TextBox txtDescription;
        private NumericUpDown numDuration;

        public FormAddAction()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            Text = "Add New Action";
            Width = 500;
            Height = 380;
            StartPosition = FormStartPosition.CenterScreen;

            var layout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                Padding = new Padding(20),
                AutoSize = true
            };

            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70));

            layout.Controls.Add(new Label
            {
                Text = "Action Type:",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            });

            txtType = new TextBox
            {
                Dock = DockStyle.Top,
                Width = 100
            };
            layout.Controls.Add(txtType);


            layout.Controls.Add(new Label
            {
                Text = "Description:",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            });

            txtDescription = new TextBox
            {
                Multiline = true,
                Dock = DockStyle.Fill,
                Height = 120,
                ScrollBars = ScrollBars.Vertical,
                BorderStyle = BorderStyle.FixedSingle
            };
            layout.Controls.Add(txtDescription);


            layout.Controls.Add(new Label
            {
                Text = "Duration (min):",
                AutoSize = true,
                Anchor = AnchorStyles.Left
            });

            numDuration = new NumericUpDown
            {
                Minimum = 0,
                Maximum = 500,
                Width = 120,
                Dock = DockStyle.Left
            };
            layout.Controls.Add(numDuration);

            var panelButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                FlowDirection = FlowDirection.RightToLeft,
                Padding = new Padding(10),
                Height = 60
            };

            var btnAdd = new Button
            {
                Text = "Add Action",
                Width = 120,
                Height = 40,
            };
            btnAdd.FlatAppearance.BorderSize = 0;
            btnAdd.Click += (s, e) => { DialogResult = DialogResult.OK; Close(); };


            var btnCancel = new Button
            {
                Text = "Cancel",
                Width = 120,
                Height = 40,
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            panelButtons.Controls.Add(btnAdd);
            panelButtons.Controls.Add(btnCancel);


            Controls.Add(layout);
            Controls.Add(panelButtons);
        }
    }
}
