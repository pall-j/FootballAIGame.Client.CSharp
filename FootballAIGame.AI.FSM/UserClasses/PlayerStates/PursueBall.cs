using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class PursueBall : PlayerState
    {
        private Pursuit BallPursuit { get; set; }

        public PursueBall(Player player, Ai ai) : base(player, ai)
        {
        }

        public override void Enter()
        {
            BallPursuit = new Pursuit(Player, 1, 1.0, Ai.Ball);
            Player.SteeringBehaviorsManager.AddBehavior(BallPursuit);
        }

        public override void Run()
        {
            if (Player.CanKickBall(Ai.Ball))
            {
                Player.StateMachine.ChangeState(new KickBall(Player, Ai));
                return;
            }

            var nearestToBall = Ai.MyTeam.NearestPlayerToBall;
            if (Player != nearestToBall && !(nearestToBall is GoalKeeper))
            {
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player, Ai));
                MessageDispatcher.Instance.SendMessage(new PursueBallMessage(), nearestToBall);
            }
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(BallPursuit);
        }
    }
}
