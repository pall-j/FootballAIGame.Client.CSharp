using System.Collections.Generic;
using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.TeamStates
{
    /// <summary>
    /// Represents team's defending state. The team stays in this state while the opponent team
    /// is controlling the ball.
    /// When the opponent team looses the ball and this team starts controlling the ball, 
    /// the state is changed to <see cref="Attacking"/>.
    /// While the team is in this state, the team's forwards are interposing themselves between
    /// the opponent's controlling player and two of his nearest players 
    /// to the controlling player.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.TeamStates.TeamState" />
    class Defending : TeamState
    {
        /// <summary>
        /// Gets or sets the interposes.
        /// </summary>
        /// <value>
        /// The interposes.
        /// </value>
        private List<Interpose> Interposes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Defending" /> class.
        /// </summary>
        /// <param name="team">The <see cref="Team" /> to which this instance belongs.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public Defending(Team team, FsmAI footballAI) : base(team, footballAI)
        {
        }

        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public override void Enter()
        {
            SetHomeRegions();

            Interposes = new List<Interpose>();

            var controllingOpponent = AI.OpponentTeam.NearestPlayerToBall;

            var firstNearestToControlling = AI.OpponentTeam.GetNearestPlayerToPosition(
                controllingOpponent.Position, controllingOpponent);

            var secondNearestToControlling = AI.OpponentTeam.GetNearestPlayerToPosition(
                controllingOpponent.Position, controllingOpponent, firstNearestToControlling);

            var interpose1 = new Interpose(Team.Forwards[0], 2, 0.8, controllingOpponent, firstNearestToControlling);
            var interpose2 = new Interpose(Team.Forwards[1], 2, 0.8, controllingOpponent, secondNearestToControlling);

            Interposes.Add(interpose1);
            Interposes.Add(interpose2);

            Team.Forwards[0].SteeringBehaviorsManager.AddBehavior(interpose1);
            Team.Forwards[1].SteeringBehaviorsManager.AddBehavior(interpose2);
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
            if (Team.PlayerInBallRange != null && AI.OpponentTeam.PlayerInBallRange == null)
            {
                Team.StateMachine.ChangeState(new Attacking(Team, AI));
                return;
            }

            UpdateInterposes();
        }

        /// <summary>
        /// Updates the interposes of the team's forwards that interpose themselves between
        /// the opponent's controlling player and two of his nearest players 
        /// to the controlling player.
        /// </summary>
        private void UpdateInterposes()
        {
            var controllingOpponent = AI.OpponentTeam.NearestPlayerToBall;

            var firstNearestToControlling = AI.OpponentTeam.GetNearestPlayerToPosition(
                controllingOpponent.Position, controllingOpponent);

            var secondNearestToControlling = AI.OpponentTeam.GetNearestPlayerToPosition(
                controllingOpponent.Position, controllingOpponent, firstNearestToControlling);

            Interposes[0].First = controllingOpponent;
            Interposes[1].First = controllingOpponent;

            Interposes[0].Second = firstNearestToControlling;
            Interposes[1].Second = secondNearestToControlling;
        }

        /// <summary>
        /// Occurs when the entity leaves this state.
        /// </summary>
        public override void Exit()
        {
            for(int i = 0; i < 2; i++)
                Team.Forwards[i].SteeringBehaviorsManager.RemoveBehavior(Interposes[i]);
        }

        /// <summary>
        /// Sets the home regions of the team's players.
        /// </summary>
        public override void SetHomeRegions()
        {
            Team.GoalKeeper.HomeRegion = Region.GetRegion(0, 4);

            Team.Defenders[0].HomeRegion = Region.GetRegion(1, 1);
            Team.Defenders[1].HomeRegion = Region.GetRegion(1, 3);
            Team.Defenders[2].HomeRegion = Region.GetRegion(1, 5);
            Team.Defenders[3].HomeRegion = Region.GetRegion(1, 7);

            Team.Midfielders[0].HomeRegion = Region.GetRegion(2, 1);
            Team.Midfielders[1].HomeRegion = Region.GetRegion(2, 3);
            Team.Midfielders[2].HomeRegion = Region.GetRegion(2, 5);
            Team.Midfielders[3].HomeRegion = Region.GetRegion(2, 7);

            Team.Forwards[0].HomeRegion = Region.GetRegion(3, 2);
            Team.Forwards[1].HomeRegion = Region.GetRegion(3, 6);

            if (Team.IsOnLeft) return;

            // team is on the right side -> mirror image
            foreach (var player in Team.Players)
            {
                player.HomeRegion = Region.GetRegion(
                    (Region.NumberOfColumns - 1) - player.HomeRegion.X, player.HomeRegion.Y);
            }
        }

    }
}
