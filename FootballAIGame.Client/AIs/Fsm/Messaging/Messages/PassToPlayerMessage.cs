using FootballAIGame.Client.AIs.Fsm.Entities;

namespace FootballAIGame.Client.AIs.Fsm.Messaging.Messages
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
