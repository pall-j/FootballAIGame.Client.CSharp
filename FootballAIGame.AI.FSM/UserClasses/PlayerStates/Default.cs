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
        public Default(Player player) : base(player)
        {
        }

        public override void Run()
        {
            var controlling = Ai.Instance.MyTeam.ControllingPlayer;
            var team = Ai.Instance.MyTeam;

            if (controlling != null &&
                team.IsNearerToOpponent(Player, controlling) &&
                team.IsPassFromControllingSafe(Player.Position) &&
                team.PassReceiver == null)
            {
                MessageDispatcher.Instance.SendMessage(new PassToPlayerMessage(Player), controlling);
            }
            else if (team.NearestPlayerToBall == Player &&
                     team.PassReceiver == null)
            {
                Player.StateMachine.ChangeState(new PursueBall(Player));
            }
            else if (!Player.IsAtHomeRegion)
            {
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player));
            }
        }

    }
}
