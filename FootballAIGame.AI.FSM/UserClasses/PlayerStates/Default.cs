using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class Default : PlayerState
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="Default"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        public Default(Player player) : base(player)
        {
        }

        public override void Run()
        {
            var controlling = Ai.Instance.MyTeam.ControllingPlayer;
            var team = Ai.Instance.MyTeam;

            if (Player is GoalKeeper)
            {
                Player.StateMachine.ChangeState(new DefendGoal(Player));
            }
            else if (controlling != null &&
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

    }
}
