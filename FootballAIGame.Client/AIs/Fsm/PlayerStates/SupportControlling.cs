using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.Messaging.Messages;
using FootballAIGame.Client.AIs.Fsm.SteeringBehaviors;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    /// <summary>
    /// Represents the player's support controlling state. The player in this state
    /// supports the controlling player by moving to the best support position.
    /// If he is able to shot on goal from that position, then he
    /// requests the pass from the controlling player. If there is some other team's
    /// player nearer to the best support position, then that player state is changed
    /// to this state and the player will go to <see cref="Default"/> state.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.PlayerStates.PlayerState" />
    class SupportControlling : PlayerState
    {
        /// <summary>
        /// Gets or sets the arrive behavior that is used to move to the best support position.
        /// </summary>
        /// <value>
        /// The <see cref="Arrive"/>.
        /// </value>
        private Arrive Arrive { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SupportControlling"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public SupportControlling(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }

        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public override void Enter()
        {
            Arrive = new Arrive(Player, 1, 1.0, AI.SupportPositionsManager.BestSupportPosition);
            Player.SteeringBehaviorsManager.AddBehavior(Arrive);
            AI.MyTeam.SupportingPlayers.Add(Player);
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
            Arrive.Target = AI.SupportPositionsManager.BestSupportPosition;
            var team = AI.MyTeam;

            // nearest except goalkeeper and controlling
            var nearest = AI.MyTeam.GetNearestPlayerToPosition(Arrive.Target, team.GoalKeeper, team.ControllingPlayer);

            // goalkeeper shouldn't go too far from his home region
            if (Player is GoalKeeper &&
                Vector.GetDistanceBetween(Arrive.Target, Player.HomeRegion.Center) > Parameters.MaxGoalkeeperSupportingDistance)
            {
                MessageDispatcher.Instance.SendMessage(new SupportControllingMessage(), nearest);
                Player.StateMachine.ChangeState(new Default(Player, AI));
                return;

            }

            // if shot on goal is possible request pass from controlling
            Vector shotVector;
            if (AI.MyTeam.TryGetShotOnGoal(Player, out shotVector) && team.ControllingPlayer != null)
                MessageDispatcher.Instance.SendMessage(new PassToPlayerMessage(Player));

            // someone else is nearer the best position (not goalkeeper)
            if (!(Player is GoalKeeper) && nearest != Player && nearest != team.ControllingPlayer)
            {
                MessageDispatcher.Instance.SendMessage(new SupportControllingMessage(), nearest);
                Player.StateMachine.ChangeState(new Default(Player, AI));
            }

        }

        /// <summary>
        /// Occurs when the entity leaves this state.
        /// </summary>
        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(Arrive);
            AI.MyTeam.SupportingPlayers.Remove(Player);
        }
    }
}
