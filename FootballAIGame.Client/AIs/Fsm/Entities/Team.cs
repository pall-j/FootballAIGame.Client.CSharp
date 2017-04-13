using System;
using System.Collections.Generic;
using System.Linq;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.TeamStates;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.Entities
{
    class Team
    {
        private bool InitialEnter { get; set; }

        public FsmAI AI { get; set; }

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
                    ? new Vector(0, GameClient.FieldHeight / 2)
                    : new Vector(GameClient.FieldWidth, GameClient.FieldHeight / 2);
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
                return GetNearestPlayerToPosition(AI.Ball.Position);
            }
        }

        public bool IsNearerToOpponent(Player player, Player otherPlayer)
        {
            if (IsOnLeft)
                return player.Position.X > otherPlayer.Position.X;
            else
                return player.Position.X < otherPlayer.Position.X;
        }

        public Team(IList<FootballPlayer> footballPlayers, FsmAI ai)
        {
            AI = ai;

            StateMachine = new FiniteStateMachine<Team>(this, new Kickoff(this, ai), new TeamGlobalState(this, ai));
            InitialEnter = true;

            Players = new Player[11];
            SupportingPlayers = new List<Player>();

            GoalKeeper = new GoalKeeper(footballPlayers[0], ai);
            Players[0] = GoalKeeper;

            Defenders = new List<Defender>(4);
            for (int i = 1; i <= 4; i++)
            {
                var defender = new Defender(footballPlayers[i], ai);
                Defenders.Add(defender);
                Players[i] = defender;
            }

            Midfielders = new List<Midfielder>(4);
            for (int i = 5; i <= 8; i++)
            {
                var midfielder = new Midfielder(footballPlayers[i], ai);
                Midfielders.Add(midfielder);
                Players[i] = midfielder;
            }

            Forwards = new List<Forward>(2);
            for (int i = 9; i <= 10; i++)
            {
                var forward = new Forward(footballPlayers[i], ai);
                Forwards.Add(forward);
                Players[i] = forward;
            }

            // important because enter method of the players state is called before team state enter method
            // in GetActions function during initial enter
            var teamState = StateMachine.CurrentState as TeamState;
            if (teamState != null)
                teamState.SetHomeRegions();
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

                var distToBall = Vector.DistanceBetween(Players[i].Position, AI.Ball.Position);

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
                if (!InitialEnter)
                    StateMachine.ChangeState(new Kickoff(this, AI));
            }

        }

        public PlayerAction[] GetActions()
        {
            if (InitialEnter)
            {
                // first players, then team

                foreach (var player in Players)
                {
                    player.StateMachine.GlobalState.Enter();
                    player.StateMachine.CurrentState.Enter();
                }

                StateMachine.GlobalState.Enter();
                StateMachine.CurrentState.Enter();

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
            {
                actions[i] = Players[i].GetAction();
                if (double.IsInfinity(actions[i].Kick.X) || double.IsNaN(actions[i].Kick.X) ||
                    double.IsInfinity(actions[i].Kick.Y) || double.IsNaN(actions[i].Kick.Y) ||
                    double.IsInfinity(actions[i].Movement.X) || double.IsNaN(actions[i].Movement.X) ||
                    double.IsInfinity(actions[i].Movement.Y) || double.IsNaN(actions[i].Movement.Y))
                    Console.WriteLine("HERE");
            }

            return actions;
        }

        public void ProcessMessage(IMessage message)
        {
            StateMachine.ProcessMessage(this, message);
        }

        public bool IsPassFromControllingSafe(Vector target)
        {
            return IsKickSafe(ControllingPlayer, target);
        }

        public bool IsKickSafe(FootballPlayer from, Vector target)
        {
            var ball = AI.Ball;

            if (from == null)
                return false;

            var toBall = Vector.Difference(ball.Position, target);

            foreach (var opponent in AI.OpponentTeam.Players)
            {
                var toOpponent = Vector.Difference(opponent.Position, target);

                var k = Vector.DotProduct(toBall, toOpponent) / toBall.Length;
                var interposeTarget = Vector.Sum(target, toBall.Resized(k));
                var opponentToInterposeDist = Vector.DistanceBetween(opponent.Position, interposeTarget);

                var opponentToKickablePosition = new Vector(opponent.Position, interposeTarget,
                    Math.Max(0, opponentToInterposeDist - FootballBall.MaxDistanceForKick));

                var kickablePosition = Vector.Sum(opponent.Position, opponentToKickablePosition);

                if (k > toBall.Length || k <= 0)
                    continue; // safe

                var ballToInterposeDist = Vector.DistanceBetween(ball.Position, interposeTarget);

                var t1 = ball.TimeToCoverDistance(ballToInterposeDist, from.MaxKickSpeed);
                var t2 = opponent.TimeToGetToTarget(kickablePosition);

                if (t2 < t1)
                    return false;
            }

            return true;
        }

        public bool IsKickPossible(FootballPlayer player, Vector target, FootballBall ball)
        {
            return !double.IsInfinity(
                ball.TimeToCoverDistance(Vector.DistanceBetween(ball.Position, target), player.MaxKickSpeed));
        }

        public bool TryGetShotOnGoal(FootballPlayer player, out Vector shotTarget)
        {
            return TryGetShotOnGoal(player, out shotTarget, AI.Ball);
        }

        public bool TryGetShotOnGoal(FootballPlayer player, out Vector shotTarget, FootballBall ball)
        {
            for (int i = 0; i < Parameters.NumberOfGeneratedShotTargets; i++)
            {
                var target =
                    new Vector(0, GameClient.FieldHeight / 2.0 + (FsmAI.Random.NextDouble() - 0.5) * 7.32 / 2);
                if (IsOnLeft)
                    target.X = GameClient.FieldWidth;

                if (IsKickPossible(player, target, ball) && IsKickSafe(player, target))
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
            return TryGetSafePass(player, out target, AI.Ball);
        }

        public bool TryGetSafePass(Player player, out Player target, Ball ball)
        {
            target = null;

            foreach (var otherPlayer in Players)
            {
                if (player == otherPlayer)
                    continue;

                if (IsKickPossible(player, otherPlayer.Position, ball) && IsKickSafe(player, otherPlayer.Position))
                {
                    if (target == null || (IsOnLeft && target.Position.X < otherPlayer.Position.X) ||
                        (!IsOnLeft && target.Position.X > otherPlayer.Position.X))
                    {
                        target = otherPlayer;
                    }
                }
            }

            return target != null;
        }

        public Player GetPredictedNearestPlayerToPosition(Vector position, double time, params Player[] skippedPlayers)
        {
            var minPlayer = Players.FirstOrDefault(p => !skippedPlayers.Contains(p));
            if (minPlayer == null)
                return null; // all players are skipped

            var minDistSq = Vector.DistanceBetweenSquared(minPlayer.PredictedPositionInTime(time), position);

            foreach (var player in Players)
            {
                if (skippedPlayers.Contains(player))
                    continue;

                var distSq = Vector.DistanceBetweenSquared(player.PredictedPositionInTime(time), position);
                if (minDistSq > distSq)
                {
                    minDistSq = distSq;
                    minPlayer = player;
                }
            }

            return minPlayer;
        }
    }
}
