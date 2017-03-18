using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses.Messaging.Messages
{
    class PassToPlayerMessage : IMessage
    {
        public Player Receiver { get; set; }

        public PassToPlayerMessage(Player receiver)
        {
            Receiver = receiver;
        }
    }
}
