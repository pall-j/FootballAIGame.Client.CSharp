using System.Collections.Generic;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    class Kickoff : TeamState
    {
        public Kickoff(Team team, Ai ai) : base(team, ai)
        {
        }

        public override void Enter()
        {
            Team.ControllingPlayer = null;
            Team.PassReceiver = null;
            Team.SupportingPlayers = new List<Player>();
            foreach (var teamPlayer in Team.Players)
            {
                teamPlayer.SteeringBehaviorsManager.Reset();
                teamPlayer.StateMachine.ChangeState(new Default(teamPlayer, Ai));
            }


            if (Team.PlayerInBallRange == null)
            {
                Team.StateMachine.ChangeState(new Defending(Team, Ai));
            }
            else
            {
                Team.StateMachine.ChangeState(new Attacking(Team, Ai));
            }
        }

        public override void Run()
        {
        }

        public override void SetHomeRegions()
        {
        }

    }
}
