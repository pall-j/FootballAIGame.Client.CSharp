using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class DefenderGlobalState : State<Defender>
    {
        private DefenderGlobalState() { }

        private static DefenderGlobalState _instance;

        public static DefenderGlobalState Instance
        {
            get { return _instance ?? (_instance = new DefenderGlobalState()); }
        }

        public override void Run(Defender entity)
        {
            FieldPlayerGlobalState<Defender>.Instance.Run(entity);
        }

        public override bool ProcessMessage(Message message)
        {
            return false;
        }
    }
}
