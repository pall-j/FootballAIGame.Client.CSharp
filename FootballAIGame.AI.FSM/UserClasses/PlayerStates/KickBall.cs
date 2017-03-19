using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class KickBall : PlayerState
    {
        public KickBall(Player player, FootballAI footballAI) : base(player, footballAI)
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
