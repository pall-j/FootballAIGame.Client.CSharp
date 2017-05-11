using FootballAIGame.Client.AIs.Fsm.Entities;

namespace FootballAIGame.Client.AIs.Fsm.Messaging
{
    /// <summary>
    /// Provides functionality to send messages to players.
    /// Implemented as singleton.
    /// </summary>
    class MessageDispatcher
    {
        /// <summary>
        /// The singleton instance.
        /// </summary>
        private static MessageDispatcher _instance;

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        /// <value>
        /// The singleton instance.
        /// </value>
        public static MessageDispatcher Instance
        {
            get
            {
                return _instance ?? (_instance = new MessageDispatcher());
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="MessageDispatcher"/> class from being created.
        /// </summary>
        private MessageDispatcher() { }

        /// <summary>
        /// Sends the specified message to the specified players.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="receivers">The receivers of the message.</param>
        public void SendMessage(IMessage message, params Player[] receivers)
        {
            foreach (var receiver in receivers)
            {
                receiver.ProcessMessage(message);
            }
        }
    }
}
