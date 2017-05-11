using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;
using FootballAIGame.Client.AIs.Fsm.TeamStates;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    /// <summary>
    /// Represents the player's defend goal state. The player will interpose
    /// himself between the ball and the controlling player in accordance with the
    /// <see cref="Parameters.DefendGoalDistance"/>. If the player can intercept
    /// the ball in accordance with the <see cref="Parameters.GoalKeeperInterceptRange"/>, 
    /// he will go to <see cref="InterceptBall"/> state. Used by the goalkeeper.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.PlayerStates.PlayerState" />
    class DefendGoal : PlayerState
    {
        /// <summary>
        /// Gets or sets the interpose that is used to move between the ball and the controlling
        /// player from any team.
        /// </summary>
        /// <value>
        /// The <see cref="Interpose"/>.
        /// </value>
        private Interpose Interpose { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefendGoal"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public DefendGoal(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }

        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public override void Enter()
        {
            var goalCenter = new Vector(0, GameClient.FieldHeight / 2);
            if (!AI.MyTeam.IsOnLeft)
                goalCenter.X = GameClient.FieldWidth;

            Interpose = new Interpose(Player, 1, 1.0, AI.Ball, goalCenter)
            {
                PreferredDistanceFromSecond = Parameters.DefendGoalDistance
            };

            Player.SteeringBehaviorsManager.AddBehavior(Interpose);
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
            if (AI.MyTeam.StateMachine.CurrentState is Defending &&
                Vector.GetDistanceBetween(AI.Ball.Position, AI.MyTeam.GoalCenter) < Parameters.GoalKeeperInterceptRange)
            {
                Player.StateMachine.ChangeState(new InterceptBall(Player, AI));
            }
        }

        /// <summary>
        /// Occurs when the entity leaves this state.
        /// </summary>
        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(Interpose);
        }
    }
}
