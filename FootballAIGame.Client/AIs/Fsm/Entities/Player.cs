using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.Entities
{
    /// <summary>
    /// Extends the <see cref="FootballPlayer"/>. Adds FSM AI specific functionality.
    /// Provides the base class from which more specific player classes derive.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.SimulationEntities.FootballPlayer" />
    abstract class Player : FootballPlayer
    {
        /// <summary>
        /// Gets or sets the finite state machine of the player.
        /// </summary>
        /// <value>
        /// The state machine of the player.
        /// </value>
        public FiniteStateMachine<Player> StateMachine { get; set; }

        /// <summary>
        /// Gets or sets the home region of the player.
        /// </summary>
        /// <value>
        /// The home region of the player.
        /// </value>
        public Region HomeRegion { get; set; }

        /// <summary>
        /// Gets or sets the steering behaviors manager of the player.
        /// </summary>
        /// <value>
        /// The steering behaviors manager of the player.
        /// </value>
        public SteeringBehaviorsManager SteeringBehaviorsManager { get; set; }

        /// <summary>
        /// Gets a value indicating whether the player is at his home region.
        /// </summary>
        /// <value>
        /// <c>true</c> if the player is at his home region; otherwise, <c>false</c>.
        /// </value>
        public bool IsAtHomeRegion
        {
            get
            {
                return Vector.GetDistanceBetween(HomeRegion.Center, Position) <= Parameters.PlayerInHomeRegionRange;
            }    
        }

        /// <summary>
        /// Gets a value indicating whether the player is in danger.
        /// Player is in danger if there is an opponent player in <see cref="Parameters.DangerRange"/> distance.
        /// </summary>
        /// <value>
        /// <c>true</c> if the player is in danger; otherwise, <c>false</c>.
        /// </value>
        public bool IsInDanger
        {
            get
            {
                var nearest = AI.OpponentTeam.GetNearestPlayerToPosition(Position);

                var predictedPosition = PredictPositionInTime(1);
                var predictedNearest = AI.OpponentTeam.PredictNearestPlayerToPosition(predictedPosition, 1);

                return Vector.GetDistanceBetween(nearest.Position, Position) < Parameters.DangerRange ||
                       Vector.GetDistanceBetween(predictedNearest.Position, predictedPosition) < Parameters.DangerRange;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="FsmAI"/> instance to which this instance belongs.
        /// </summary>
        /// <value>
        /// The <see cref="FsmAI"/> instance to which this instance belongs.
        /// </value>
        protected FsmAI AI { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Player"/> class.
        /// </summary>
        /// <param name="player">The football player.</param>
        /// <param name="footballAI">The <see cref="FsmAI"/> instance to which this player belongs.</param>
        protected Player(FootballPlayer player, FsmAI footballAI) : base(player.Id)
        {
            AI = footballAI;

            Position = player.Position;
            Movement = player.Movement;
            KickVector = player.KickVector;

            Speed = player.Speed;
            KickPower = player.KickPower;
            Possession = player.Possession;
            Precision = player.Precision;

            SteeringBehaviorsManager = new SteeringBehaviorsManager(this);
        }

        /// <summary>
        /// Processes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public abstract void ProcessMessage(IMessage message);

        /// <summary>
        /// Gets the player's action in the current state.
        /// </summary>
        /// <returns>The <see cref="PlayerAction"/> containing the action of the player in the current state.</returns>
        public PlayerAction GetAction()
        {
            var action = new PlayerAction
            {
                Movement = Vector.GetSum(SteeringBehaviorsManager.GetAccelerationVector(), Movement),
                Kick = KickVector
            };

            return action;
        }
    }
}
