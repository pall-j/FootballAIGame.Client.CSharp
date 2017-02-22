using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class MidfielderGlobalState : State<Player>
    {
        private MidfielderGlobalState() { }

        private static MidfielderGlobalState _instance;

        public static MidfielderGlobalState Instance
        {
            get { return _instance ?? (_instance = new MidfielderGlobalState()); }
        }

        public override void Run(Player player)
        {
            FieldPlayerGlobalState.Instance.Run(player);
        }

        public override bool ProcessMessage(Player entity, Message message)
        {
            return FieldPlayerGlobalState.Instance.ProcessMessage(entity, message);
        }
    }
}
