using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
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
        public Default(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            Wander = new Wander(Player, 1, 0.2, 0, 2, 4);
            Player.SteeringBehaviorsManager.AddBehavior(Wander);
        }

        public override void Run()
        {
            var controlling = Ai.Instance.MyTeam.ControllingPlayer;
            var team = Ai.Instance.MyTeam;

            if (Player is GoalKeeper)
            {
                Player.StateMachine.ChangeState(new DefendGoal(Player));
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
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player));
            }
        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(Wander);
        }
    }
}
