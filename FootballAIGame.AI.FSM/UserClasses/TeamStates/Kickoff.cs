using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    class Kickoff : TeamState
    {
        private Kickoff() { }

        private static Kickoff _instance;

        public static Kickoff Instance
        {
            get { return _instance ?? (_instance = new Kickoff()); }
        }

        public override void Enter(Team team)
        {
            var nearestToBall = Ai.Instance.CurrentState.NearestPlayerToBall;

            if (nearestToBall.Id < 11) // team is nearest to ball
                team.StateMachine.ChangeState(Attacking.Instance);
            else
                team.StateMachine.ChangeState(Attacking.Instance);
        }

        public override void Run(Team team)
        {
        }

        public override bool ProcessMessage(Team entity, Message message)
        {
            return false;
        }

        public override void SetHomeRegions(Team team)
        {
        }
    }
}
