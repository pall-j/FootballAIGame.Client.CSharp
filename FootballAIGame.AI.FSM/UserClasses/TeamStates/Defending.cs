﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    class Defending : TeamState
    {
        private List<Interpose> Interposes { get; set; }

        public override void Enter()
        {
            SetHomeRegions(Team);

            Interposes = new List<Interpose>();

            var controllingOpponent = Ai.Instance.OpponentTeam.NearestPlayerToBall;

            var firstNearestToControlling = Ai.Instance.OpponentTeam.GetNearestPlayerToPosition(
                controllingOpponent.Position, controllingOpponent);

            var secondNearestToControlling = Ai.Instance.OpponentTeam.GetNearestPlayerToPosition(
                controllingOpponent.Position, controllingOpponent, firstNearestToControlling);

            var interpose1 = new Interpose(Team.Forwards[0], 2, 0.8, controllingOpponent, firstNearestToControlling);
            var interpose2 = new Interpose(Team.Forwards[1], 2, 0.8, controllingOpponent, secondNearestToControlling);

            Interposes.Add(interpose1);
            Interposes.Add(interpose2);

            Team.Forwards[0].SteeringBehaviorsManager.AddBehavior(interpose1);
            Team.Forwards[1].SteeringBehaviorsManager.AddBehavior(interpose2);
        }

        public override void Run()
        {
            if (Team.PlayerInBallRange != null && Ai.Instance.OpponentTeam.PlayerInBallRange == null)
            {
                Team.StateMachine.ChangeState(new Attacking(Team));
                return;
            }

            UpdateSteeringBehaviors();
        }

        private void UpdateSteeringBehaviors()
        {
            var controllingOpponent = Ai.Instance.OpponentTeam.NearestPlayerToBall;

            var firstNearestToControlling = Ai.Instance.OpponentTeam.GetNearestPlayerToPosition(
                controllingOpponent.Position, controllingOpponent);

            var secondNearestToControlling = Ai.Instance.OpponentTeam.GetNearestPlayerToPosition(
                controllingOpponent.Position, controllingOpponent, firstNearestToControlling);

            Interposes[0].First = controllingOpponent;
            Interposes[1].First = controllingOpponent;

            Interposes[0].Second = firstNearestToControlling;
            Interposes[1].Second = secondNearestToControlling;
        }

        public override void Exit()
        {
            for(int i = 0; i < 2; i++)
                Team.Forwards[i].SteeringBehaviorsManager.RemoveBehavior(Interposes[i]);
        }

        public override bool ProcessMessage(Message message)
        {
            return false;
        }

        public override void SetHomeRegions(Team team)
        {

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
