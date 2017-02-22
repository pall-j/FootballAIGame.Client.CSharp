using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballAIGame.AI.FSM.UserClasses.Messaging
{
    class ReturnToHomeMessage : Message
    {
        private static ReturnToHomeMessage _instace;

        public static ReturnToHomeMessage Instance
        {
            get { return _instace ?? (_instace = new ReturnToHomeMessage()); }
        }

        private ReturnToHomeMessage() { }
    }
}
