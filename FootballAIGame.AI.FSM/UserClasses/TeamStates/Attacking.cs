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
        public override void Enter()
        {
            Console.WriteLine("ENTERING ATTACKING");
            SetHomeRegions(Team);
        }

        public override void Run()
        {
            if (Team.PlayerInBallRange == null &&
                Ai.Instance.OpponentTeam.PlayerInBallRange != null)
            {
                Console.WriteLine("ATTACKING -> DEFENDING");
                Team.StateMachine.ChangeState(new Defending(Team));
            }
            
            if (Team.SupportingPlayers.Count == 0 && Team.ControllingPlayer != null)
            {
                var bestPos = Utilities.SupportPositionsManager.Instance.BestSupportPosition;
                var bestSupporter = Team.GetNearestPlayerToPosition(bestPos, Team.ControllingPlayer);
                MessageDispatcher.Instance.SendMessage(new SupportControllingMessage(), bestSupporter);
            }
        }

        public override bool ProcessMessage(Message message)
        {
            return false;
        }

        public override void SetHomeRegions(Team team)
        {
            Console.WriteLine("ATTACKING REGIONS");
            team.GoalKeeper.HomeRegion = Region.GetRegion(7, 4);

            team.Defenders[0].HomeRegion = Region.GetRegion(6, 4);
            team.Defenders[3].HomeRegion = Region.GetRegion(5, 3);
            team.Defenders[1].HomeRegion = Region.GetRegion(4, 2);
            team.Defenders[2].HomeRegion = Region.GetRegion(4, 6);

            team.Midfielders[0].HomeRegion = Region.GetRegion(3, 4);
            team.Midfielders[1].HomeRegion = Region.GetRegion(2, 2);
            team.Midfielders[2].HomeRegion = Region.GetRegion(2, 4);
            team.Midfielders[3].HomeRegion = Region.GetRegion(2, 6);

            team.Forwards[0].HomeRegion = Region.GetRegion(1, 2);
            team.Forwards[1].HomeRegion = Region.GetRegion(1, 6);

            if (!team.IsOnLeft) return;

            // team is on the left side -> mirror image
            foreach (var player in team.Players)
            {
                player.HomeRegion = Region.GetRegion(
                    (Region.NumberOfColumns - 1) - player.HomeRegion.X, player.HomeRegion.Y);
            }
        }

        public Attacking(Team entity) : base(entity)
        {
        }
    }
}
