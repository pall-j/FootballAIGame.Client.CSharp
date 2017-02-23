using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    class TeamGlobalState : State<Team>
    {
        public override void Run()
        {
        }

        public override bool ProcessMessage(Message message)
        {
            return false;
        }

        public TeamGlobalState(Team team) : base(team)
        {
        }
    }
}
