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
        private TeamGlobalState() { }

        private static TeamGlobalState _instance;

        public static TeamGlobalState Instance
        {
            get { return _instance ?? (_instance = new TeamGlobalState()); }
        }


        public override void Run(Team entity)
        {
        }

        public override bool ProcessMessage(Message message)
        {
            return false;
        }

    }
}
