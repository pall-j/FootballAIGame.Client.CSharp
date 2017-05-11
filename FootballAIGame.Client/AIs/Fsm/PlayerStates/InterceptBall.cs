using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;
using FootballAIGame.Client.AIs.Fsm.TeamStates;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    /// <summary>
    /// Represents the player's intercept state. Player in this state tries to intercept the
    /// ball until he is in <see cref="Parameters.GoalKeeperInterceptRange"/> distance from
    /// his goal or while his team is attacking. Used by the goalkeeper.
    /// </summary>
    /// <seealso cref="PlayerState" />
    class InterceptBall : PlayerState
    {
        /// <summary>
        /// Gets or sets the ball's pursuit.
        /// </summary>
        /// <value>
        /// The ball's <see cref="Pursuit"/>.
        /// </value>
        private Pursuit BallPursuit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InterceptBall"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public InterceptBall(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }

        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public override void Enter()
        {
            BallPursuit = new Pursuit(Player, 1, 1.0, AI.Ball);
            Player.SteeringBehaviorsManager.AddBehavior(BallPursuit);
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
            if (AI.MyTeam.StateMachine.CurrentState is Attacking ||
                Vector.GetDistanceBetween(Player.Position, AI.MyTeam.GoalCenter) >
                Parameters.GoalKeeperInterceptRange)
            {
                Player.StateMachine.ChangeState(new DefendGoal(Player, AI));
            }
        }

        /// <summary>
        /// Occurs when the entity leaves this state.
        /// </summary>
        public override void Exit()
        {
           Player.SteeringBehaviorsManager.RemoveBehavior(BallPursuit);
        }
    }
}
