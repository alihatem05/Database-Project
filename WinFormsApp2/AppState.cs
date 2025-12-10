// AppState.cs
namespace AgentActivitiesTracker
{
    public static class AppState
    {
        public static Database Db { get; set; }

        public static Employee CurrentUser { get; set; }

        // Navigation stack for forms (stack-based back navigation)
        // Push the current form before navigating forward.
        public static System.Collections.Generic.Stack<System.Windows.Forms.Form> Navigation { get; } =
            new System.Collections.Generic.Stack<System.Windows.Forms.Form>();
    }
}
