﻿using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class DefenderGlobalState : PlayerState
    {
        private FieldPlayerGlobalState FieldPlayerGlobalState { get; set; }

        public DefenderGlobalState(Player player, FootballAI footballAI) : base(player, footballAI)
        {
            FieldPlayerGlobalState = new FieldPlayerGlobalState(player, footballAI);
        }

        public override void Run()
        {
            FieldPlayerGlobalState.Run();
        }

        public override bool ProcessMessage(IMessage message)
        {
            return FieldPlayerGlobalState.ProcessMessage(message);
        }
    }
}
