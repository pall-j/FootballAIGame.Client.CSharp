using FootballAIGame.Client.AIs.Fsm.Entities;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    /// <summary>
    /// Provides the base class from which the classes that represent player's states are derived.
    /// Contains the shared functionality of the player's states.
    /// </summary>
    /// <seealso cref="State{Player}" />
    abstract class PlayerState : State<Player>
    {
        /// <summary>
        /// Gets or sets the player to whom this instance belongs.
        /// </summary>
        /// <value>
        /// The <see cref="Player"/> to whom this instance belongs.
        /// </value>
        protected Player Player { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerState" /> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI"/> instance to which this instance belongs.</param>
        protected PlayerState(Player player, FsmAI footballAI) : base(player, footballAI)
        {
            Player = player;
        }
    }
}
