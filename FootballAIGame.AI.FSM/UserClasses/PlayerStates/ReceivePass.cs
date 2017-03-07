using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class ReceivePass : PlayerState
    {
        private SteeringBehavior SteeringBehavior { get; set; }

        private Vector PassTarget { get; set; }

        public ReceivePass(Player player, Vector passTarget) : base(player)
        {
            PassTarget = passTarget;
        }

        public override void Enter()
        {
            Ai.Instance.MyTeam.PassReceiver = Player;
            Ai.Instance.MyTeam.ControllingPlayer = Player;
            SteeringBehavior = new Arrive(Player, 1, 1.0, PassTarget);
            Player.SteeringBehaviorsManager.AddBehavior(SteeringBehavior);
        }

        public override void Run()
        {
            if (Ai.Instance.MyTeam.PassReceiver != Player)
            {
                Player.StateMachine.ChangeState(new Default(Player));
                return;
            }

            // lost control
            if (Ai.Instance.OpponentTeam.PlayerInBallRange != null && Ai.Instance.MyTeam.PlayerInBallRange == null)
            {
                Player.StateMachine.ChangeState(new Default(Player));
                return;
            }

            if (Player.CanKickBall(Ai.Instance.Ball))
            {
                Player.StateMachine.ChangeState(new KickBall(Player));
                return;
            }

            if (Vector.DistanceBetween(Ai.Instance.Ball.Position, Player.Position) < Parameters.BallReceivingRange)
            {
                Player.StateMachine.ChangeState(new PursueBall(Player));
                return;
            }

            UpdatePassTarget();

            var nearestOpponent = Ai.Instance.OpponentTeam.GetNearestPlayerToPosition(Player.Position);
            var ball = Ai.Instance.Ball;

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
            var ball = Ai.Instance.Ball;
            var time = ball.TimeToCoverDistance(Vector.DistanceBetween(PassTarget, ball.Position), ball.CurrentSpeed);
            PassTarget = ball.PredictedPositionInTime(time);

            var arrive = SteeringBehavior as Arrive;
            if (arrive != null)
                arrive.Target = PassTarget;
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(SteeringBehavior);
            if (Player == Ai.Instance.MyTeam.PassReceiver)
                Ai.Instance.MyTeam.PassReceiver = null;
        }
    }
}
