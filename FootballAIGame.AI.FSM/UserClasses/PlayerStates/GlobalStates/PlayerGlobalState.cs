﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class PlayerGlobalState : State<Player>
    {
        private PlayerGlobalState() { }

        private static PlayerGlobalState _instance;

        public static PlayerGlobalState Instance
        {
            get { return _instance ?? (_instance = new PlayerGlobalState()); }
        }

        public override void Run(Player entity)
        {
        }

        public override bool ProcessMessage(Player player, Message message)
        {
            if (message is ReturnToHomeMessage)
            {
                player.StateMachine.ChangeState(MoveToHomeRegion.Instance);
                return true;
            }

            return false;
        }
    }
}
