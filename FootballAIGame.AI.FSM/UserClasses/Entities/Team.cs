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

        public Player PlayerInBallRange { get; set; }

        public Player ControllingPlayer { get; set; }

        public Player PassReceiver { get; set; }

        public List<Player> SupportingPlayers { get; set; }

        public Vector GoalCenter
        {
            get
            {
                return IsOnLeft
                    ? new Vector(0, GameClient.FieldHeight/2)
                    : new Vector(GameClient.FieldWidth, GameClient.FieldHeight/2);
            }
        }

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

                var distSq = Vector.DistanceBetweenSquared(player.Position, position);
                if (minDistSq > distSq)
                {
                    minDistSq = distSq;
                    minPlayer = player;
                }
            }

            return minPlayer;
        }

        public Player NearestPlayerToBall
        {
            get
            {
                return GetNearestPlayerToPosition(Ai.Instance.Ball.Position);
            }
        }

        public bool IsNearerToOpponent(Player player, Player otherPlayer)
        {
            if (IsOnLeft)
                return player.Position.X > otherPlayer.Position.X;
            else
                return player.Position.X < otherPlayer.Position.X;
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

            // update team
            StateMachine.Update();

            // update players
            foreach (var player in Players)
                player.StateMachine.Update();

            // retrieve actions
            var actions = new PlayerAction[11];
            for (int i = 0; i < 11; i++)
                actions[i] = Players[i].GetAction();

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

        public bool IsPassFromControllingSafe(Vector target)
        {
            return IsKickSafe(ControllingPlayer, target);
        }

        public bool IsKickSafe(Player from, Vector target)
        {
            var ball = Ai.Instance.Ball;

            if (from == null)
                return false;

            var toFrom = Vector.Difference(from.Position, target);

            foreach (var opponent in Ai.Instance.OpponentTeam.Players)
            {
                var toOpponent = Vector.Difference(opponent.Position, target);

                var k = Vector.DotProduct(toFrom, toOpponent) / toFrom.Length;
                var interposeTarget = Vector.Sum(target, toFrom.Resized(k));

                if (k > toFrom.Length || k <= 0)
                    continue; // safe

                var controllingToInterpose = Vector.DistanceBetween(from.Position, interposeTarget);

                var t1 = ball.TimeToCoverDistance(controllingToInterpose, from.MaxKickSpeed);
                var t2 = opponent.TimeToGetToTarget(interposeTarget);

                if (t2 < t1)
                    return false;
            }

            return true;
        }

        public void LoadState(GameState state, bool firstTeam)
        {
            var diff = firstTeam ? 0 : 11;
            PlayerInBallRange = null;
            var bestDist = 0.0;

            for (int i = 0; i < Players.Length; i++)
            {
                Players[i].Movement = state.FootballPlayers[i + diff].Movement;
                Players[i].Position = state.FootballPlayers[i + diff].Position;
                Players[i].KickVector = new Vector(0, 0);

                var distToBall = Vector.DistanceBetween(Players[i].Position, Ai.Instance.Ball.Position);

                if (distToBall < Parameters.BallRange &&
                    (PlayerInBallRange == null || bestDist > distToBall))
                {
                    bestDist = distToBall;
                    PlayerInBallRange = Players[i];
                }
            }

            if (firstTeam && state.KickOff)
            {
                IsOnLeft = GoalKeeper.Position.X < 55;
                StateMachine.ChangeState(new Kickoff(this)); // todo maybe change to message
            }

        }

        public bool TryGetShotOnGoal(Player player, out Vector shotTarget)
        {
            // try 10 random positions

            for (int i = 0; i < Parameters.NumberOfGeneratedShotTargets; i++)
            {
                var target =
                    new Vector(0, GameClient.FieldHeight/2.0 + (Ai.Random.NextDouble() - 0.5) * 7.32 / 2);
                if (IsOnLeft)
                    target.X = GameClient.FieldWidth;

                if (IsKickSafe(player, target))
                {
                    shotTarget = target;
                    return true;
                }
            }

            shotTarget = null;
            return false;
        }

        public bool TryGetSafePass(Player player, out Player target)
        {
            target = null;

            foreach (var otherPlayer in Players)
            {
                if (player == otherPlayer)
                    continue;
                if (IsKickSafe(player, otherPlayer.Position))
                {
                    if (target == null || (IsOnLeft && target.Position.X < otherPlayer.Position.X) ||
                        (!IsOnLeft && target.Position.X > otherPlayer.Position.X))
                        target = otherPlayer;
                }
            }

            return target != null;
        }
    }
}
