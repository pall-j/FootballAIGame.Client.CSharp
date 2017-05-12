using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;

namespace FootballAIGame.Client.AIs.Fsm.TeamStates
{
    /// <summary>
    /// Represents the team's global state.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.State{Team}" />
    class TeamGlobalState : State<Team>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TeamGlobalState" /> class.
        /// </summary>
        /// <param name="team">The <see cref="Team"/> to which this instance belongs.</param>
        /// <param name="footballAI">The <see cref="FsmAI"/> instance to which this instance belongs.</param>
        public TeamGlobalState(Team team, FsmAI footballAI) : base(team, footballAI)
        {
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
        }

        /// <summary>
        /// Processes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        ///   <c>true</c> if the specified message was handled; otherwise, <c>false</c>
        /// </returns>
        public override bool ProcessMessage(IMessage message)
        {
            return false;
        }
    }
}
