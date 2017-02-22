using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class GoalKeeperGlobalState : State<Player>
    {
        private GoalKeeperGlobalState() { }

        private static GoalKeeperGlobalState _instance;

        public static GoalKeeperGlobalState Instance
        {
            get { return _instance ?? (_instance = new GoalKeeperGlobalState()); }
        }

        public override void Run(Player player)
        {
            PlayerGlobalState.Instance.Run(player);
        }

        public override bool ProcessMessage(Player entity, Message message)
        {
            return PlayerGlobalState.Instance.ProcessMessage(entity, message);
        }
    }
}
