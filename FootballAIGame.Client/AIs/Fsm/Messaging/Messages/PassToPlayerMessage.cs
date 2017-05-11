using FootballAIGame.Client.AIs.Fsm.Entities;

namespace FootballAIGame.Client.AIs.Fsm.Messaging.Messages
{
    /// <summary>
    /// Represents the message that tells player to pass the ball to the specified receiver.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.Messaging.IMessage" />
    class PassToPlayerMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the pass receiver.
        /// </summary>
        /// <value>
        /// The pass receiver.
        /// </value>
        public Player Receiver { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PassToPlayerMessage"/> class.
        /// </summary>
        /// <param name="receiver">The pass receiver.</param>
        public PassToPlayerMessage(Player receiver)
        {
            Receiver = receiver;
        }
    }
}
