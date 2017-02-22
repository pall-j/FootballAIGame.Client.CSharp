using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class ForwardGlobalState : State<Player>
    {
        private ForwardGlobalState() { }

        private static ForwardGlobalState _instance;

        public static ForwardGlobalState Instance
        {
            get { return _instance ?? (_instance = new ForwardGlobalState()); }
        }

        public override void Run(Player player)
        {
            FieldPlayerGlobalState.Instance.Run(player);
        }

        public override bool ProcessMessage(Player entity, Message message)
        {
            return FieldPlayerGlobalState.Instance.ProcessMessage(entity, message);
        }
    }
}
