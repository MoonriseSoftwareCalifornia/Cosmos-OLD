namespace Cosmos.Cms.Models
{
    /// <summary>
    /// Live editor SignalR model
    /// </summary>
    public class LiveEditorSignal
    {
        /// <summary>
        /// Edit ID
        /// </summary>
        public string EditorId { get; set; }

        /// <summary>
        /// User Id (Email address)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// User position in document
        /// </summary>
        public int Offset { get; set; }

        /// <summary>
        /// Command
        /// </summary>
        public string Command { get; set; }

        /// <summary>
        /// Is Focused
        /// </summary>
        public bool IsFocused { get; set; }
    }
}
