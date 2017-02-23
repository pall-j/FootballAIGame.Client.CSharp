using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class FieldPlayerGlobalState : State<Player>
    {
        private PlayerGlobalState PlayerGlobalState { get; set; }

        public FieldPlayerGlobalState(Player entity) : base(entity)
        {
            PlayerGlobalState = new PlayerGlobalState(entity);
        }

        public override void Run()
        {
            PlayerGlobalState.Run();   
        }

        public override bool ProcessMessage(Message message)
        {
            return PlayerGlobalState.ProcessMessage(message);
        }
    }
}
