using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.Messaging
{
    class MessageDispatcher
    {
        private static MessageDispatcher _instace;

        public static MessageDispatcher Instance
        {
            get
            {
                return _instace ?? (_instace = new MessageDispatcher());
            }
        }

        private MessageDispatcher() { }

        public void SendMessage(Message message, params Player[] receivers)
        {
            foreach (var receiver in receivers)
            {
                receiver.ProcessMessage(message);
            }
        }
    }
}
