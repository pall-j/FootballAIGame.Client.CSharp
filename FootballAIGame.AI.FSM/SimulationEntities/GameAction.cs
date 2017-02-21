

namespace FootballAIGame.AI.FSM.SimulationEntities
{
    /// <summary>
    /// Represents the AI action that consists of the football players actions.
    /// </summary>
    class GameAction
    {
        /// <summary>
        /// Gets or sets the player's actions. When this instance is sent to the server, there should be 11
        /// players with their movement and kick vectors set.
        /// </summary>
        public PlayerAction[] PlayerActions { get; set; }

        /// <summary>
        /// Gets or sets the simulation step of this action. Describes to which simulation step this action belongs.
        /// </summary>
        public int Step { get; set; }
    }

}
