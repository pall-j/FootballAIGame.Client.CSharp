using System.Collections.Generic;
using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.PlayerStates;

namespace FootballAIGame.Client.AIs.Fsm.TeamStates
{
    class Kickoff : TeamState
    {
        public Kickoff(Team team, FsmAI footballAI) : base(team, footballAI)
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
