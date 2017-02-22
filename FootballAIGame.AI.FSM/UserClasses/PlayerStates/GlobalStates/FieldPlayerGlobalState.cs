using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class FieldPlayerGlobalState : State<Player>
    {
        private FieldPlayerGlobalState() { }

        private static FieldPlayerGlobalState _instance;

        public static FieldPlayerGlobalState Instance
        {
            get { return _instance ?? (_instance = new FieldPlayerGlobalState()); }
        }

        public override void Run(Player entity)
        {
            PlayerGlobalState.Instance.Run(entity);   
        }

        public override bool ProcessMessage(Player entity, Message message)
        {
            return PlayerGlobalState.Instance.ProcessMessage(entity, message);
        }
    }
}
