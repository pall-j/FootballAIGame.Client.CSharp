using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class FieldPlayerGlobalState<TFieldPlayer> : State<TFieldPlayer> where TFieldPlayer : FieldPlayer
    {
        private FieldPlayerGlobalState() { }

        private static FieldPlayerGlobalState<TFieldPlayer> _instance;

        public static FieldPlayerGlobalState<TFieldPlayer> Instance
        {
            get { return _instance ?? (_instance = new FieldPlayerGlobalState<TFieldPlayer>()); }
        }

        public override void Run(TFieldPlayer entity)
        {
            PlayerGlobalState<TFieldPlayer>.Instance.Run(entity);   
        }

        public override bool ProcessMessage(Message message)
        {
            return PlayerGlobalState<TFieldPlayer>.Instance.ProcessMessage(message);
        }
    }
}
