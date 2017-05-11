using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.Messaging.Messages;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    /// <summary>
    /// Represents the player's pursue ball state. Player in this state pursues ball
    /// until he reaches it or he is not the nearest field player to ball from his team.
    /// If he reaches the ball he will change his state to <see cref="KickBall"/>.
    /// If he is not the nearest field player to ball from his team anymore, then he changes
    /// state to <see cref="MoveToHomeRegion"/>.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.PlayerStates.PlayerState" />
    class PursueBall : PlayerState
    {
        /// <summary>
        /// Gets or sets the ball pursuit.
        /// </summary>
        /// <value>
        /// The ball <see cref="Pursuit"/>.
        /// </value>
        private Pursuit BallPursuit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PursueBall"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public PursueBall(Player player, FsmAI footballAI) : base(player, footballAI)
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

        /// <summary>
        /// Occurs when the entity leaves this state.
        /// </summary>
        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(BallPursuit);
        }
    }
}
