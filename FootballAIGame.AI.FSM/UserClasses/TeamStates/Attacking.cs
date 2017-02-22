using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    class Attacking : TeamState
    {
        private Attacking() { }

        private static Attacking _instance;

        public static Attacking Instance
        {
            get { return _instance ?? (_instance = new Attacking()); }
        }

        public override void Enter(Team team)
        {
            SetHomeRegions(team);
            foreach (var player in team.Players)
            {
                MessageDispatcher.Instance.SendMessage(ReturnToHome.Instance, player);
            }
        }

        public override void Run(Team entity)
        {
        }

        public override bool ProcessMessage(Team entity, Message message)
        {
            return false;
        }

        public override void SetHomeRegions(Team team)
        {
            Console.WriteLine(Ai.Instance.CurrentState.Step);
            Console.WriteLine(team.IsOnLeft);

            team.GoalKeeper.HomeRegion = Region.GetRegion(7, 4); // goalkeeper is on team side!

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

            if (!team.IsOnLeft) return;

            // team is on the left side -> mirror image
            foreach (var player in team.Players)
            {
                player.HomeRegion = Region.GetRegion(
                    (Region.NumberOfColumns - 1) - player.HomeRegion.X, player.HomeRegion.Y);
            }
        }
    }
}
