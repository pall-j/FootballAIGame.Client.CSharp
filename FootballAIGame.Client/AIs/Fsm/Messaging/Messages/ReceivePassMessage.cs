using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.Messaging.Messages
{
    /// <summary>
    /// Represents the message that tells player to go to the specified position and wait there for a pass.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.Messaging.IMessage" />
    class ReceivePassMessage : IMessage
    {
        /// <summary>
        /// Gets or sets the pass target.
        /// </summary>
        /// <value>
        /// The pass target.
        /// </value>
        public Vector PassTarget { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivePassMessage"/> class.
        /// </summary>
        /// <param name="target">The pass target.</param>
        public ReceivePassMessage(Vector target)
        {
            PassTarget = target;
        }
    }
}
