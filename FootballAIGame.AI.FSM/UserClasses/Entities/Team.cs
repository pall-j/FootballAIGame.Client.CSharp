using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.TeamStates;
using FootballAIGame.AI.FSM.UserClasses.Utilities;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    class Team
    {
        private bool InitialEnter { get; set; }

        public FiniteStateMachine<Team> StateMachine { get; set; }

        public Player[] Players { get; set; }

        public GoalKeeper GoalKeeper { get; set; }

        public List<Defender> Defenders { get; set; }

        public List<Midfielder> Midfielders { get; set; }

        public List<Forward> Forwards { get; set; }

        public Player ControllingPlayer { get; set; }

        public Player PassReceiver { get; set; }

        public List<Player> SupportingPlayers { get; set; }

        /// <summary>
        /// Gets or sets the value indicating whether the team holds currently the left goal post.
        /// </summary>
        public bool IsOnLeft { get; set; }

        public Player GetNearestPlayerToPosition(Vector position, params Player[] skippedPlayers)
        {
            var minPlayer = Players.FirstOrDefault(p => !skippedPlayers.Contains(p));
            if (minPlayer == null)
                return null; // all players are skipped

            var minDistSq = Vector.DistanceBetweenSquared(minPlayer.Position, position);

            foreach (var player in Players)
            {
                if (skippedPlayers.Contains(player))
                    continue;

                var dist = Vector.DistanceBetweenSquared(player.Position, minPlayer.Position);
                if (dist < minDistSq)
                {
                    minDistSq = dist;
                    minPlayer = player;
                }
            }

            return minPlayer;
        }

        public Team(IList<FootballPlayer> footballPlayers)
        {
            StateMachine = new FiniteStateMachine<Team>(this, new Kickoff(this), new TeamGlobalState(this));
            InitialEnter = true;

            Players = new Player[11];
            SupportingPlayers = new List<Player>();

            GoalKeeper = new GoalKeeper(footballPlayers[0]);
            Players[0] = GoalKeeper;

            Defenders = new List<Defender>(4);
            for (int i = 1; i <= 4; i++)
            {
                var defender = new Defender(footballPlayers[i]);
                Defenders.Add(defender);
                Players[i] = defender;
            }

            Midfielders = new List<Midfielder>(4);
            for (int i = 5; i <= 8; i++)
            {
                var midfielder = new Midfielder(footballPlayers[i]);
                Midfielders.Add(midfielder);
                Players[i] = midfielder;
            }

            Forwards = new List<Forward>(2);
            for (int i = 9; i <= 10; i++)
            {
                var forward = new Forward(footballPlayers[i]);
                Forwards.Add(forward);
                Players[i] = forward;
            }

        }

        public PlayerAction[] GetActions()
        {
            if (InitialEnter)
            {
                StateMachine.CurrentState.Enter();
                StateMachine.GlobalState.Enter();

                foreach (var player in Players)
                    player.StateMachine.GlobalState.Enter();

                InitialEnter = false;
            }

            StateMachine.Update();

            var actions = new PlayerAction[11];
            for (int i = 0; i < 11; i++)
            {
                actions[i] = Players[i].GetAction();
            }

            return actions;
        }

        public void ProcessMessage(Message message)
        {
            StateMachine.ProcessMessage(this, message);
        }

        public void UpdateHomeRegions()
        {
            var currentState = StateMachine.CurrentState;
            var teamState = currentState as TeamState;
            Debug.Assert(teamState != null, "currentState is TeamState");
            teamState.SetHomeRegions(this);
        }

        public void LoadState(GameState state, bool firstTeam)
        {
            var diff = firstTeam ? 0 : 11;
            ControllingPlayer = null;
            var bestControllingDist = 0.0;

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].Movement = state.FootballPlayers[i + diff].Movement;
                Players[i].Position = state.FootballPlayers[i + diff].Position;

                var distToBall = Vector.DistanceBetween(Players[i].Position, Ai.Instance.CurrentState.Ball.Position);

                if (distToBall < FootballBall.MinDistanceForKick*2 &&
                    (ControllingPlayer == null || bestControllingDist > distToBall))
                {
                    bestControllingDist = distToBall;
                    ControllingPlayer = Players[i];
                }
            }

            if (state.Step == 0 || state.Step == 750)
            {
                IsOnLeft = GoalKeeper.Position.X < 55;
                StateMachine.ChangeState(new Kickoff(this)); // todo maybe change to message
            }

        }
    }
}
