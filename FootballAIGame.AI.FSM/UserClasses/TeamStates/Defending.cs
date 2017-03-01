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
    class Defending : TeamState
    {
        public override void Enter()
        {
            Console.WriteLine("ENTERING DEFENDING");
            SetHomeRegions(Team);
        }

        public override void Run()
        {
            if (Team.PlayerInBallRange != null && Ai.Instance.OpponentTeam.PlayerInBallRange == null)
            {
                Team.StateMachine.ChangeState(new Attacking(Team));
                Console.WriteLine("DEFENDING -> ATTACKING");
            }

        }

        public override bool ProcessMessage(Message message)
        {
            return false;
        }

        public override void SetHomeRegions(Team team)
        {
            Console.WriteLine("DEFENDING REGIONS");

            team.GoalKeeper.HomeRegion = Region.GetRegion(0, 4);

            team.Defenders[0].HomeRegion = Region.GetRegion(1, 1);
            team.Defenders[1].HomeRegion = Region.GetRegion(1, 3);
            team.Defenders[2].HomeRegion = Region.GetRegion(1, 5);
            team.Defenders[3].HomeRegion = Region.GetRegion(1, 7);

            team.Midfielders[0].HomeRegion = Region.GetRegion(2, 1);
            team.Midfielders[1].HomeRegion = Region.GetRegion(2, 3);
            team.Midfielders[2].HomeRegion = Region.GetRegion(2, 5);
            team.Midfielders[3].HomeRegion = Region.GetRegion(2, 7);

            team.Forwards[0].HomeRegion = Region.GetRegion(3, 2);
            team.Forwards[1].HomeRegion = Region.GetRegion(3, 6);

            if (team.IsOnLeft) return;

            // team is on the right side -> mirror image
            foreach (var player in team.Players)
            {
                player.HomeRegion = Region.GetRegion(
                    (Region.NumberOfColumns - 1) - player.HomeRegion.X, player.HomeRegion.Y);
            }
        }

        public Defending(Team team) : base(team)
        {
        }
    }
}
