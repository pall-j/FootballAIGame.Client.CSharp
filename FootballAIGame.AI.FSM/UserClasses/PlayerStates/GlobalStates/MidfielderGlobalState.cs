using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class MidfielderGlobalState : State<Midfielder>
    {
        private MidfielderGlobalState() { }

        private static MidfielderGlobalState _instance;

        public static MidfielderGlobalState Instance
        {
            get { return _instance ?? (_instance = new MidfielderGlobalState()); }
        }

        public override void Run(Midfielder player)
        {
            FieldPlayerGlobalState<Midfielder>.Instance.Run(player);
        }

        public override bool ProcessMessage(Message message)
        {
            return FieldPlayerGlobalState<Midfielder>.Instance.ProcessMessage(message);
        }
    }
}
