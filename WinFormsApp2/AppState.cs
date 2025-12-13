namespace AgentActivitiesTracker
{
    public static class AppState
    {
        public static Database Db { get; set; }

        public static Employee CurrentUser { get; set; }

        public static System.Collections.Generic.Stack<System.Windows.Forms.Form> Navigation { get; } =
            new System.Collections.Generic.Stack<System.Windows.Forms.Form>();
    }
}
