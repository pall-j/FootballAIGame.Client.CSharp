using System.Collections.Generic;
using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.PlayerStates;

namespace FootballAIGame.Client.AIs.Fsm.TeamStates
{
    /// <summary>
    /// Represents the team's kickoff state. This is the initial state of the team.
    /// The team's state is changed to this when the kickoff is happening.
    /// When the team enters this state, all its players' states are changed to <see cref="Default"/>.
    /// The team's state is changed to <see cref="Attacking"/> if the team is doing the kickoff.
    /// Otherwise the state is changed to <see cref="Defending"/>.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.TeamStates.TeamState" />
    class Kickoff : TeamState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Kickoff" /> class.
        /// </summary>
        /// <param name="team">The <see cref="Team" /> to which this instance belongs.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public Kickoff(Team team, FsmAI footballAI) : base(team, footballAI)
        {
        }

        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public override void Enter()
        {
            Team.ControllingPlayer = null;
            Team.PassReceiver = null;
            Team.SupportingPlayers = new List<Player>();
            foreach (var teamPlayer in Team.Players)
            {
                teamPlayer.SteeringBehaviorsManager.Reset();
                teamPlayer.StateMachine.ChangeState(new Default(teamPlayer, AI));
            }

            if (Team.PlayerInBallRange == null)
            {
                Team.StateMachine.ChangeState(new Defending(Team, AI));
            }
            else
            {
                Team.StateMachine.ChangeState(new Attacking(Team, AI));
            }
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
        }

        /// <summary>
        /// Sets the home regions of the team's players.
        /// </summary>
        public override void SetHomeRegions()
        {
        }
    }
}
