using FootballAIGame.Client.AIs.Fsm.Entities;

namespace FootballAIGame.Client.AIs.Fsm.TeamStates
{
    /// <summary>
    /// Provides the base class from which the classes that represent team's states are derived.
    /// Contains the shared functionality of the team's states.
    /// </summary>
    /// <seealso cref="State{Team}" />
    abstract class TeamState : State<Team>
    {
        /// <summary>
        /// Gets or sets the team to which this instance belongs.
        /// </summary>
        /// <value>
        /// The <see cref="Team"/> to which this instance belongs.
        /// </value>
        protected Team Team { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamState"/> class.
        /// </summary>
        /// <param name="team">The <see cref="Team"/> to which this instance belongs.</param>
        /// <param name="footballAI">The <see cref="FsmAI"/> instance to which this instance belongs.</param>
        protected TeamState(Team team, FsmAI footballAI) : base(team, footballAI)
        {
            Team = team;
        }

        /// <summary>
        /// Sets the home regions of the team's players.
        /// </summary>
        public abstract void SetHomeRegions();
    }
}
