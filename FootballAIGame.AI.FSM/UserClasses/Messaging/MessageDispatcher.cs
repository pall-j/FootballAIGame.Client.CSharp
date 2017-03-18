using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.Messaging
{
    class MessageDispatcher
    {
        private static MessageDispatcher _instance;

        public static MessageDispatcher Instance
        {
            get
            {
                return _instance ?? (_instance = new MessageDispatcher());
            }
        }

        private MessageDispatcher() { }

        public void SendMessage(IMessage message, params Player[] receivers)
        {
            foreach (var receiver in receivers)
            {
                receiver.ProcessMessage(message);
            }
        }
    }
}
