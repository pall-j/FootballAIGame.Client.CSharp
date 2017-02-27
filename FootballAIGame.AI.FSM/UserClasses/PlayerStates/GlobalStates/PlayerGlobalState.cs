using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class PlayerGlobalState : PlayerState
    {
        public override void Run()
        {
        }

        public override bool ProcessMessage(Message message)
        {
            if (message is ReturnToHomeMessage)
            {
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player));
                return true;
            }
            if (message is SupportControllingMessage)
            {
                Player.StateMachine.ChangeState(new SupportControlling(Player));
                return true;
            }

            return false;
        }

        public PlayerGlobalState(Player entity) : base(entity)
        {
        }
    }
}
