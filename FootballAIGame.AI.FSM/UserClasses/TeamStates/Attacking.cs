using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    class Attacking : TeamState
    {
        public Attacking(Team team) : base(team)
        {
        }

        public override void Enter()
        {
            SetHomeRegions();
        }

        public override void Run()
        {
            if (Team.PlayerInBallRange == null &&
                Ai.Instance.OpponentTeam.PlayerInBallRange != null)
            {
                Team.StateMachine.ChangeState(new Defending(Team));
            }
            
            if (Team.SupportingPlayers.Count == 0 && Team.ControllingPlayer != null)
            {
                var bestPos = Utilities.SupportPositionsManager.Instance.BestSupportPosition;
                var bestSupporter = Team.GetNearestPlayerToPosition(bestPos, Team.ControllingPlayer);
                MessageDispatcher.Instance.SendMessage(new SupportControllingMessage(), bestSupporter);
            }
        }

        public override void SetHomeRegions()
        {
            Team.GoalKeeper.HomeRegion = Region.GetRegion(7, 4);

            Team.Defenders[0].HomeRegion = Region.GetRegion(6, 4);
            Team.Defenders[1].HomeRegion = Region.GetRegion(5, 3);
            Team.Defenders[2].HomeRegion = Region.GetRegion(4, 2);
            Team.Defenders[3].HomeRegion = Region.GetRegion(4, 6);

            Team.Midfielders[0].HomeRegion = Region.GetRegion(3, 4);
            Team.Midfielders[1].HomeRegion = Region.GetRegion(2, 2);
            Team.Midfielders[2].HomeRegion = Region.GetRegion(2, 4);
            Team.Midfielders[3].HomeRegion = Region.GetRegion(2, 6);

            Team.Forwards[0].HomeRegion = Region.GetRegion(1, 2);
            Team.Forwards[1].HomeRegion = Region.GetRegion(1, 6);

            if (!Team.IsOnLeft) return;

            // team is on the left side -> mirror image
            foreach (var player in Team.Players)
            {
                player.HomeRegion = Region.GetRegion(
                    (Region.NumberOfColumns - 1) - player.HomeRegion.X, player.HomeRegion.Y);
            }
        }

    }
}
