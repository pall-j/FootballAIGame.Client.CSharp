using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.Messaging.Messages
{
    class ReceivePassMessage : IMessage
    {
        public Vector PassTarget { get; set; }

        public ReceivePassMessage(Vector target)
        {
            PassTarget = target;
        }
    }
}
