using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FootballAIGame.AI.FSM.UserClasses.Messaging
{
    class ReturnToHome : Message
    {
        private static ReturnToHome _instace;

        public static ReturnToHome Instance
        {
            get { return _instace ?? (_instace = new ReturnToHome()); }
        }

        private ReturnToHome() { }
    }
}
