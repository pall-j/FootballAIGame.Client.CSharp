using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.Messaging.Messages;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    /// <summary>
    /// Represents the player's default state. Its the initial state of all players.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.PlayerStates.PlayerState" />
    class Default : PlayerState
    {
        /// <summary>
        /// Gets or sets the wander.
        /// </summary>
        /// <value>
        /// The wander.
        /// </value>
        private Wander Wander { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Default" /> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public Default(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }

        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public override void Enter()
        {
            Wander = new Wander(Player, 1, 0.2, 0, 2, 4);
            Player.SteeringBehaviorsManager.AddBehavior(Wander);
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
            var controlling = AI.MyTeam.ControllingPlayer;
            var team = AI.MyTeam;

            if (Player is GoalKeeper)
            {
                Player.StateMachine.ChangeState(new DefendGoal(Player, AI));
                return;
            }

            if (controlling != null &&
                team.IsNearerToOpponent(Player, controlling) &&
                team.IsPassFromControllingSafe(Player.Position) &&
                team.PassReceiver == null)
            {
                MessageDispatcher.Instance.SendMessage(new PassToPlayerMessage(Player), controlling);
            }
            else if (!Player.IsAtHomeRegion)
            {
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player, AI));
            }
        }

        /// <summary>
        /// Occurs when the entity leaves this state.
        /// </summary>
        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(Wander);
        }
    }
}
