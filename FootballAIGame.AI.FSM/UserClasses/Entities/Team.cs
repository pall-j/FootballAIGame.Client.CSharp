using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using FootballAIGameClient.SimulationEntities;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    class Team
    {
        public FiniteStateMachine<Team> StateMachine { get; set; }

        public Player[] Players { get; set; }

        public GoalKeeper GoalKeeper { get; set; }

        public List<Defender> Defenders { get; set; }

        public List<Midfielder> Midfielders { get; set; }

        public List<Forward> Forwards { get; set; }

        public Team(IList<FootballPlayer> footballPlayers)
        {
            Players = new Player[11];

            GoalKeeper = new GoalKeeper(footballPlayers[0]);
            Players[0] = GoalKeeper;

            for (int i = 1; i <= 4; i++)
            {
                var defender = new Defender(footballPlayers[i]);
                Defenders.Add(defender);
                Players[i] = defender;
            }

            for (int i = 5; i <= 8; i++)
            {
                var midfielder = new Midfielder(footballPlayers[i]);
                Midfielders.Add(midfielder);
                Players[i] = midfielder;
            }

            for (int i = 9; i <= 10; i++)
            {
                var forward = new Forward(footballPlayers[i]);
                Forwards.Add(forward);
                Players[i] = forward;
            }
        }

        public void Update()
        {
            StateMachine.Update();

            foreach (var player in Players)
            {
                player.Update();
            }
        }
    }
}
