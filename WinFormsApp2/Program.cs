// Program.cs
using Agent_Activities_Tracker;
using System;
using System.Windows.Forms;

namespace AgentActivitiesTracker
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            AppState.Db = new Database();

            Application.Run(new LoginForm());
        }
    }
}
