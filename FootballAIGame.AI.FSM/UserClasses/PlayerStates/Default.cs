using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class Default : PlayerState
    {
        private Wander Wander { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Default"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        public Default(Player player, FootballAI footballAI) : base(player, footballAI)
        {
        }

        public override void Enter()
        {
            Wander = new Wander(Player, 1, 0.2, 0, 2, 4);
            Player.SteeringBehaviorsManager.AddBehavior(Wander);
        }

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

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(Wander);
        }
    }
}
