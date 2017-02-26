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
        public override void Enter()
        {
            var nearestToBall = Ai.Instance.CurrentState.NearestPlayerToBall;

            if (nearestToBall.Id < 11) // team is nearest to ball
                Entity.StateMachine.ChangeState(new Defending(Entity));
            else
                Entity.StateMachine.ChangeState(new Defending(Entity));
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
