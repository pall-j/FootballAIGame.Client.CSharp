using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.Messaging.Messages;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    class PursueBall : PlayerState
    {
        private Pursuit BallPursuit { get; set; }

        public PursueBall(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }

        public override void Enter()
        {
            BallPursuit = new Pursuit(Player, 1, 1.0, AI.Ball);
            Player.SteeringBehaviorsManager.AddBehavior(BallPursuit);
        }

        public override void Run()
        {
            if (Player.CanKickBall(AI.Ball))
            {
                Player.StateMachine.ChangeState(new KickBall(Player, AI));
                return;
            }

            var nearestToBall = AI.MyTeam.NearestPlayerToBall;
            if (Player != nearestToBall && !(nearestToBall is GoalKeeper))
            {
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player, AI));
                MessageDispatcher.Instance.SendMessage(new PursueBallMessage(), nearestToBall);
            }
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(BallPursuit);
        }
    }
}
