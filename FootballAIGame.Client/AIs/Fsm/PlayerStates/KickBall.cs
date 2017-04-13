using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.Messaging.Messages;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    class KickBall : PlayerState
    {
        public KickBall(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }

        public override void Enter()
        {
            AI.MyTeam.ControllingPlayer = Player;
            Run(); // run immediately
        }

        public override void Run()
        {
            var team = AI.MyTeam;

            if (team.PassReceiver != null)
            {
                team.PassReceiver = null;
            }

            Vector target;
            if (team.TryGetShotOnGoal(Player, out target))
            {
                Player.KickBall(AI.Ball, target);
                Player.StateMachine.ChangeState(new Default(Player, AI));
                return;
            }
           
            Player passPlayerTarget;
            if (Player.IsInDanger && team.TryGetSafePass(Player, out passPlayerTarget))
            {
                var passTarget = Player.PassBall(AI.Ball, passPlayerTarget);
                MessageDispatcher.Instance.SendMessage(new ReceivePassMessage(passTarget), passPlayerTarget);
                Player.StateMachine.ChangeState(new Default(Player, AI));
                return;
            }
            
            Player.StateMachine.ChangeState(new Dribble(Player, AI));
        }

        public override void Exit()
        {
            if (AI.MyTeam.ControllingPlayer == Player)
                AI.MyTeam.ControllingPlayer = null;
        }
    }
}
