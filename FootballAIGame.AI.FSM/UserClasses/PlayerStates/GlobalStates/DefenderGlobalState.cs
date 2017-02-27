using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class DefenderGlobalState : PlayerState
    {
        private FieldPlayerGlobalState FieldPlayerGlobalState { get; set; }

        public DefenderGlobalState(Player player) : base(player)
        {
            FieldPlayerGlobalState = new FieldPlayerGlobalState(player);
        }

        public override void Run()
        {
            FieldPlayerGlobalState.Run();
        }

        public override bool ProcessMessage(Message message)
        {
            return FieldPlayerGlobalState.ProcessMessage(message);
        }
    }
}
