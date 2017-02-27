using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class GoalKeeperGlobalState : PlayerState
    {
        private PlayerGlobalState PlayerGlobalState { get; set; }

        public GoalKeeperGlobalState(Player player) : base(player)
        {
            PlayerGlobalState = new PlayerGlobalState(player);
        }

        public override void Run()
        {
            PlayerGlobalState.Run();
        }

        public override bool ProcessMessage(Message message)
        {
            return PlayerGlobalState.ProcessMessage(message);
        }
    }
}
