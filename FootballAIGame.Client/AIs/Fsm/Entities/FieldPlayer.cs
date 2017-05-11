using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.Entities
{
    /// <summary>
    /// Represents the field player. All football players besides goalkeeper are field players.
    /// Provides the base class from which more specific field player classes derive.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.Entities.Player" />
    abstract class FieldPlayer : Player
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FieldPlayer"/> class.
        /// </summary>
        /// <param name="player">The football player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this player belongs.</param>
        protected FieldPlayer(FootballPlayer player, FsmAI footballAI) : 
            base(player, footballAI)
        {
        }
    }
}
