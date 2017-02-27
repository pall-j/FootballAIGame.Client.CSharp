namespace FootballAIGame.AI.FSM.UserClasses.Messaging.Messages
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
