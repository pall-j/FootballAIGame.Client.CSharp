using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    class Kickoff : TeamState
    {
        public override void Enter()
        {
            Team.ControllingPlayer = null;
            Team.PassReceiver = null;
            Team.SupportingPlayers = new List<Player>();
            foreach (var teamPlayer in Team.Players)
            {
                teamPlayer.SteeringBehaviorsManager.Reset();
                teamPlayer.StateMachine.ChangeState(new Default(teamPlayer));
            }

            Console.WriteLine("KICKOFF");

            if (Team.PlayerInBallRange == null)
            {
                Console.WriteLine("KICKOFF -> DEFENDING");
                Team.StateMachine.ChangeState(new Defending(Team));
            }
            else
            {
                Console.WriteLine("KICKOFF -> ATTACKING");
                Team.StateMachine.ChangeState(new Attacking(Team));
            }
        }

        public override void Run()
        {
        }

        public override bool ProcessMessage(Message message)
        {
            return false;
        }

        public override void SetHomeRegions(Team team)
        {
        }

        public Kickoff(Team team) : base(team)
        {
        }
    }
}
