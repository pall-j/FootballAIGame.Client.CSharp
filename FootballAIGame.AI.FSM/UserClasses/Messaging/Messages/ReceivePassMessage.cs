using FootballAIGame.AI.FSM.CustomDataTypes;

namespace FootballAIGame.AI.FSM.UserClasses.Messaging.Messages
{
    class ReceivePassMessage : Message
    {
        public Vector PassTarget { get; set; }

        public ReceivePassMessage(Vector target)
        {
            PassTarget = target;
        }
    }
}
