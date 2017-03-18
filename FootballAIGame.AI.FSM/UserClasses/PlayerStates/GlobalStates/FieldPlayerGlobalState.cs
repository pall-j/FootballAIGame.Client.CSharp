using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class FieldPlayerGlobalState : PlayerState
    {
        private PlayerGlobalState PlayerGlobalState { get; set; }

        public FieldPlayerGlobalState(Player player, Ai ai) : base(player, ai)
        {
            PlayerGlobalState = new PlayerGlobalState(player, ai);
        }

        public override void Run()
        {
            var team = Ai.MyTeam;

            if (Player.CanKickBall(Ai.Ball))
            {
                Player.StateMachine.ChangeState(new KickBall(Player, Ai));
            }
            else if (team.NearestPlayerToBall == Player &&
                     team.PassReceiver == null)
            {
                Player.StateMachine.ChangeState(new PursueBall(Player, Ai));
            }


            PlayerGlobalState.Run();   
        }

        public override bool ProcessMessage(IMessage message)
        {
            return PlayerGlobalState.ProcessMessage(message);
        }
    }
}
