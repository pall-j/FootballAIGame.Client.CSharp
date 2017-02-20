using System;
using System.Collections.Generic;
using System.Text;
using FootballAIGameClient.CustomDataTypes;

namespace FootballAIGameClient.SimulationEntities
{
    /// <summary>
    /// Represents football player's action that is send to the server.
    /// </summary>
    class PlayerAction
    {
        /// <summary>
        /// Gets or sets the desired movement vector of the player.
        /// </summary>
        public Vector Movement { get; set; }

        /// <summary>
        /// Gets or sets the desired kick vector of the player.
        /// </summary>
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
