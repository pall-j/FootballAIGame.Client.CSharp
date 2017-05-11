using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    /// <summary>
    /// Represents the player's receive pass state. Player in this state goes to the specified pass target location
    /// and waits for the ball until the ball is in <see cref="Parameters.BallReceivingRange"/> or he can kick the
    /// ball. If the player's team stopped controlling the ball, then goes to <see cref="Default"/>.
    /// If the player cannot reach the pass target sooner than opponent player or the opponent
    /// player would get to ball before it reaches the pass target, then the player
    /// pursues the ball.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.PlayerStates.PlayerState" />
    class ReceivePass : PlayerState
    {
        /// <summary>
        /// Gets or sets the steering behavior used for pursing the ball or arriving at
        /// the pass target location.
        /// </summary>
        /// <value>
        /// The <see cref="SteeringBehavior"/>.
        /// </value>
        private SteeringBehavior SteeringBehavior { get; set; }

        /// <summary>
        /// Gets or sets the pass target.
        /// </summary>
        /// <value>
        /// The pass target <see cref="Vector"/>.
        /// </value>
        private Vector PassTarget { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReceivePass"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The football ai.</param>
        /// <param name="passTarget">The pass target.</param>
        public ReceivePass(Player player, FsmAI footballAI, Vector passTarget) : base(player, footballAI)
        {
            PassTarget = passTarget;
        }

        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public override void Enter()
        {
            AI.MyTeam.PassReceiver = Player;
            AI.MyTeam.ControllingPlayer = Player;
            SteeringBehavior = new Arrive(Player, 1, 1.0, PassTarget);
            Player.SteeringBehaviorsManager.AddBehavior(SteeringBehavior);
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
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

            if (Vector.GetDistanceBetween(AI.Ball.Position, Player.Position) < Parameters.BallReceivingRange)
            {
                Player.StateMachine.ChangeState(new PursueBall(Player, AI));
                return;
            }

            UpdatePassTarget();

            var nearestOpponent = AI.OpponentTeam.GetNearestPlayerToPosition(Player.Position);
            var ball = AI.Ball;

            var timeToReceive = ball.GetTimeToCoverDistance(Vector.GetDistanceBetween(ball.Position, PassTarget), ball.CurrentSpeed);

            if (nearestOpponent.GetTimeToGetToTarget(PassTarget) < timeToReceive || 
                Player.GetTimeToGetToTarget(PassTarget) > timeToReceive)
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

        /// <summary>
        /// Occurs when the entity leaves this state.
        /// </summary>
        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(SteeringBehavior);
            if (Player == AI.MyTeam.PassReceiver)
                AI.MyTeam.PassReceiver = null;
        }

        /// <summary>
        /// Updates the pass target to the current predicted position in time
        /// in which the ball would cover the distance between its position and the current pass target.
        /// </summary>
        private void UpdatePassTarget()
        {
            var ball = AI.Ball;
            var time = ball.GetTimeToCoverDistance(Vector.GetDistanceBetween(PassTarget, ball.Position), ball.CurrentSpeed);
            PassTarget = ball.PredictPositionInTime(time);

            var arrive = SteeringBehavior as Arrive;
            if (arrive != null)
                arrive.Target = PassTarget;
        }

    }
}
