

namespace CesiPlayground.Core.Storing
{
    /// <summary>
    /// Static hub providing access to distinct Blackboard instances
    /// for client-side and server-side runtime data.
    /// </summary>
    public static class BlackboardHub
    {
        /// <summary>
        /// Blackboard for client-side state (e.g., UI state, local player preferences).
        /// This data is not authoritative.
        /// </summary>
        public static readonly Blackboard Client = new Blackboard();

        /// <summary>
        /// Blackboard for server-side state (e.g., game rules, authoritative player positions).
        /// This data is the source of truth.
        /// </summary>
        public static readonly Blackboard Server = new Blackboard();
    }
}