using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.TeamStates
{
    abstract class TeamState : State<Team>
    {
        protected Team Team { get; set; }

        protected TeamState(Team team) : base(team)
        {
            Team = team;
        }

        public abstract void SetHomeRegions();
    }
}
