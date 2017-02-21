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
        public abstract void SetHomeRegions(Team team);
    }
}
