using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class ForwardGlobalState : State<Forward>
    {
        private ForwardGlobalState() { }

        private static ForwardGlobalState _instance;

        public static ForwardGlobalState Instance
        {
            get { return _instance ?? (_instance = new ForwardGlobalState()); }
        }

        public override void Run(Forward player)
        {
            FieldPlayerGlobalState<Forward>.Instance.Run(player);
        }

        public override bool ProcessMessage(Message message)
        {
            return FieldPlayerGlobalState<Forward>.Instance.ProcessMessage(message);
        }
    }
}
