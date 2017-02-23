using System;
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
        public override void Run()
        {
        }

        public override bool ProcessMessage(Message message)
        {
            if (message is ReturnToHomeMessage)
            {
                Entity.StateMachine.ChangeState(new MoveToHomeRegion(Entity));
                return true;
            }

            return false;
        }

        public PlayerGlobalState(Player entity) : base(entity)
        {
        }
    }
}
