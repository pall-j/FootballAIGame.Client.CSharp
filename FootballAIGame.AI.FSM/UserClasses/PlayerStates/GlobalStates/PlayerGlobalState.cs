using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class PlayerGlobalState<TPlayer> : State<TPlayer> where TPlayer : Player
    {
        private PlayerGlobalState() { }

        private static PlayerGlobalState<TPlayer> _instance;

        public static PlayerGlobalState<TPlayer> Instance
        {
            get { return _instance ?? (_instance = new PlayerGlobalState<TPlayer>()); }
        }

        public override void Run(TPlayer entity)
        {
        }

        public override bool ProcessMessage(Message message)
        {
            return false;
        }
    }
}
