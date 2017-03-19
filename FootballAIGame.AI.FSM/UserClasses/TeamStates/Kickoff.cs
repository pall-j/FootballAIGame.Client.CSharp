using System.Collections.Generic;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    class Kickoff : TeamState
    {
        public Kickoff(Team team, FootballAI footballAI) : base(team, footballAI)
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

        public override void Run()
        {
        }

        public override void SetHomeRegions()
        {
        }

    }
}
