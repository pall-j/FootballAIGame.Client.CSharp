using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    class ReceivePass : PlayerState
    {
        private SteeringBehavior SteeringBehavior { get; set; }

        private Vector PassTarget { get; set; }

        public ReceivePass(Player player, FsmAI footballAI, Vector passTarget) : base(player, footballAI)
        {
            PassTarget = passTarget;
        }

        public override void Enter()
        {
            AI.MyTeam.PassReceiver = Player;
            AI.MyTeam.ControllingPlayer = Player;
            SteeringBehavior = new Arrive(Player, 1, 1.0, PassTarget);
            Player.SteeringBehaviorsManager.AddBehavior(SteeringBehavior);
        }

        public override void Run()
        {
            if (AI.MyTeam.PassReceiver != Player)
            {
                Player.StateMachine.ChangeState(new Default(Player, AI));
                return;
            }

            // lost control
            if (AI.OpponentTeam.PlayerInBallRange != null && AI.MyTeam.PlayerInBallRange == null)
            {
                Player.StateMachine.ChangeState(new Default(Player, AI));
                return;
            }

            if (Player.CanKickBall(AI.Ball))
            {
                Player.StateMachine.ChangeState(new KickBall(Player, AI));
                return;
            }

            if (Vector.DistanceBetween(AI.Ball.Position, Player.Position) < Parameters.BallReceivingRange)
            {
                Player.StateMachine.ChangeState(new PursueBall(Player, AI));
                return;
            }

            UpdatePassTarget();

            var nearestOpponent = AI.OpponentTeam.GetNearestPlayerToPosition(Player.Position);
            var ball = AI.Ball;

            var timeToReceive = ball.TimeToCoverDistance(Vector.DistanceBetween(ball.Position, PassTarget), ball.CurrentSpeed);

            if (nearestOpponent.TimeToGetToTarget(PassTarget) < timeToReceive || 
                Player.TimeToGetToTarget(PassTarget) > timeToReceive)
            {
                if (SteeringBehavior is Arrive)
                {
                    Player.SteeringBehaviorsManager.RemoveBehavior(SteeringBehavior);
                    SteeringBehavior = new Pursuit(Player, SteeringBehavior.Priority, SteeringBehavior.Weight, ball);
                    Player.SteeringBehaviorsManager.AddBehavior(SteeringBehavior);
                }
            }
            else
            {
                if (SteeringBehavior is Pursuit)
                {
                    Player.SteeringBehaviorsManager.RemoveBehavior(SteeringBehavior);
                    SteeringBehavior = new Arrive(Player, SteeringBehavior.Priority, SteeringBehavior.Weight, PassTarget);
                    Player.SteeringBehaviorsManager.AddBehavior(SteeringBehavior);

                }
            }

        }

        private void UpdatePassTarget()
        {
            var ball = AI.Ball;
            var time = ball.TimeToCoverDistance(Vector.DistanceBetween(PassTarget, ball.Position), ball.CurrentSpeed);
            PassTarget = ball.PredictedPositionInTime(time);

            var arrive = SteeringBehavior as Arrive;
            if (arrive != null)
                arrive.Target = PassTarget;
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(SteeringBehavior);
            if (Player == AI.MyTeam.PassReceiver)
                AI.MyTeam.PassReceiver = null;
        }
    }
}
