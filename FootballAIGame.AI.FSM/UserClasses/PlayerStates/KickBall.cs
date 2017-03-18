using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class KickBall : PlayerState
    {
        public KickBall(Player player, Ai ai) : base(player, ai)
        {
        }

        public override void Enter()
        {
            Ai.MyTeam.ControllingPlayer = Player;
            Run(); // run immediately
        }

        public override void Run()
        {
            var team = Ai.MyTeam;

            if (team.PassReceiver != null)
            {
                team.PassReceiver = null;
            }

            Vector target;
            if (team.TryGetShotOnGoal(Player, out target))
            {
                Player.KickBall(Ai.Ball, target);
                Player.StateMachine.ChangeState(new Default(Player, Ai));
                return;
            }
           
            Player passPlayerTarget;
            if (Player.IsInDanger && team.TryGetSafePass(Player, out passPlayerTarget))
            {
                var passTarget = Player.PassBall(Ai.Ball, passPlayerTarget);
                MessageDispatcher.Instance.SendMessage(new ReceivePassMessage(passTarget), passPlayerTarget);
                Player.StateMachine.ChangeState(new Default(Player, Ai));
                return;
            }
            
            Player.StateMachine.ChangeState(new Dribble(Player, Ai));
        }

        public override void Exit()
        {
            if (Ai.MyTeam.ControllingPlayer == Player)
                Ai.MyTeam.ControllingPlayer = null;
        }
    }
}
