using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class GoalKeeperGlobalState : State<GoalKeeper>
    {
        private GoalKeeperGlobalState() { }

        private static GoalKeeperGlobalState _instance;

        public static GoalKeeperGlobalState Instance
        {
            get { return _instance ?? (_instance = new GoalKeeperGlobalState()); }
        }

        public override void Run(GoalKeeper player)
        {
            PlayerGlobalState<GoalKeeper>.Instance.Run(player);
        }

        public override bool ProcessMessage(GoalKeeper entity, Message message)
        {
            return PlayerGlobalState<GoalKeeper>.Instance.ProcessMessage(entity, message);
        }
    }
}
