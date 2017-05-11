using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.SimulationEntities
{
    /// <summary>
    /// Represents football player's action that is send to the server.
    /// </summary>
    class PlayerAction
    {
        /// <summary>
        /// Gets or sets the desired movement vector of the player.
        /// </summary>
        /// <value>
        /// The movement <see cref="Vector"/>.
        /// </value>
        public Vector Movement { get; set; }

        /// <summary>
        /// Gets or sets the desired kick vector of the player.
        /// </summary>
        /// <value>
        /// The kick <see cref="Vector"/>.
        /// </value>
        public Vector Kick { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerAction"/> class.
        /// </summary>
        public PlayerAction()
        {
            Movement = new Vector();
            Kick = new Vector();
        }
    }
}
