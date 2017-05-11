using System;
using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    /// <summary>
    /// Represents the player's move to home state. Player in this state
    /// runs to his home region.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.PlayerStates.PlayerState" />
    class MoveToHomeRegion : PlayerState
    {
        /// <summary>
        /// Gets or sets the move to home region arrive behavior.
        /// </summary>
        /// <value>
        /// The move to home region <see cref="Arrive"/>.
        /// </value>
        private Arrive MoveToHomeRegionArrive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MoveToHomeRegion"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public MoveToHomeRegion(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }

        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public override void Enter()
        {
            MoveToHomeRegionArrive = new Arrive(Player, 3, 1, Player.HomeRegion.Center);
            Player.SteeringBehaviorsManager.AddBehavior(MoveToHomeRegionArrive);
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
            MoveToHomeRegionArrive.Target = Player.HomeRegion.Center;
            if (Player.IsAtHomeRegion && Math.Abs(Player.CurrentSpeed) < 0.00001)
                Player.StateMachine.ChangeState(new Default(Player, AI));

        }

        /// <summary>
        /// Occurs when the entity leaves this state.
        /// </summary>
        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(MoveToHomeRegionArrive);
        }

    }
}
