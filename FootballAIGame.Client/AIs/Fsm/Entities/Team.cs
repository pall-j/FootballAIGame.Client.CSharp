using System;
using System.Collections.Generic;
using System.Linq;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.TeamStates;
using FootballAIGame.Client.CustomDataTypes;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.Entities
{
    /// <summary>
    /// Represents the football team.
    /// </summary>
    class Team
    {
        /// <summary>
        /// Gets or sets a value indicating whether the initial states' (of the team and its players) enter methods
        /// have already been called.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the initial states' enter methods have already been called; otherwise, <c>false</c>.
        /// </value>
        private bool InitialEnter { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FsmAI"/> instance to which this instance belongs.
        /// </summary>
        /// <value>
        /// The <see cref="FsmAI"/> instance to which this instance belongs.
        /// </value>
        public FsmAI AI { get; set; }

        /// <summary>
        /// Gets or sets the finite state machine of the team.
        /// </summary>
        /// <value>
        /// The <see cref="FiniteStateMachine{TEntity}"/> of the team.
        /// </value>
        public FiniteStateMachine<Team> StateMachine { get; set; }

        /// <summary>
        /// Gets or sets the array of team's players.
        /// </summary>
        /// <value>
        /// The array of team's players.
        /// </value>
        public Player[] Players { get; set; }

        /// <summary>
        /// Gets or sets the team's goalkeeper.
        /// </summary>
        /// <value>
        /// The team's <see cref="GoalCenter"/>.
        /// </value>
        public GoalKeeper GoalKeeper { get; set; }

        /// <summary>
        /// Gets or sets the list of team's defenders.
        /// </summary>
        /// <value>
        /// The <see cref="List{T}"/> of team's <see cref="Defender"/>s.
        /// </value>
        public List<Defender> Defenders { get; set; }

        /// <summary>
        /// Gets or sets the list of team's midfielders.
        /// </summary>
        /// <value>
        /// The <see cref="List{T}"/> of team's <see cref="Midfielder"/>s.
        /// </value>
        public List<Midfielder> Midfielders { get; set; }

        /// <summary>
        /// Gets or sets the list of team's forwards.
        /// </summary>
        /// <value>
        /// The <see cref="List{T}"/> of team's <see cref="Forward"/>s.
        /// </value>
        public List<Forward> Forwards { get; set; }

        /// <summary>
        /// Gets or sets the player that is in the ball range.
        /// Player is in ball range if he is nearer than <see cref="Parameters.BallRange"/> from the ball.
        /// If there are more players in the ball range, only one of them is referenced from here.
        /// </summary>
        /// <value>
        /// The <see cref="Player"/> in the ball range if there is one; otherwise, null.
        /// </value>
        public Player PlayerInBallRange { get; set; }

        /// <summary>
        /// Gets or sets the player, that is currently controlling the ball.
        /// </summary>
        /// <value>
        /// The controlling <see cref="Player"/> if there is one; otherwise, null.
        /// </value>
        public Player ControllingPlayer { get; set; }

        /// <summary>
        /// Gets or sets the current pass receiver.
        /// </summary>
        /// <value>
        /// The pass receiver <see cref="Player"/> if there is one; otherwise, null.
        /// </value>
        public Player PassReceiver { get; set; }

        /// <summary>
        /// Gets or sets the list of supporting players that support the current controlling player.
        /// </summary>
        /// <value>
        /// The <see cref="List{T}"/> of supporting players.
        /// </value>
        public List<Player> SupportingPlayers { get; set; }

        /// <summary>
        /// Gets the team's goal center.
        /// </summary>
        /// <value>
        /// The team's goal center position <see cref="Vector"/>.
        /// </value>
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
        /// Gets or sets the value indicating whether the team currently holds the left goal post.
        /// </summary>
        /// <value>
        /// <c>true</c> if the team currently holds the left goal post; otherwise, <c>false</c>.
        /// </value>
        public bool IsOnLeft { get; set; }

        /// <summary>
        /// Gets the nearest player, from the team's players besides the specified <see cref="skippedPlayers"/>, 
        /// to the specified position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="skippedPlayers">The skipped players.</param>
        /// <returns>The nearest team's <see cref="Player"/> if there is at least one player outside of the
        /// specified <see cref="skippedPlayers"/>; otherwise, null.</returns>
        public Player GetNearestPlayerToPosition(Vector position, params Player[] skippedPlayers)
        {
            var minPlayer = Players.FirstOrDefault(p => !skippedPlayers.Contains(p));
            if (minPlayer == null)
                return null; // all players are skipped

            var minDistSq = Vector.GetDistanceBetweenSquared(minPlayer.Position, position);

            foreach (var player in Players)
            {
                if (skippedPlayers.Contains(player))
                    continue;

                var distSq = Vector.GetDistanceBetweenSquared(player.Position, position);
                if (minDistSq > distSq)
                {
                    minDistSq = distSq;
                    minPlayer = player;
                }
            }

            return minPlayer;
        }

        /// <summary>
        /// Gets the nearest team's player to the ball.
        /// </summary>
        /// <value>
        /// The nearest team's <see cref="Player"/> to the ball.
        /// </value>
        public Player NearestPlayerToBall
        {
            get
            {
                return GetNearestPlayerToPosition(AI.Ball.Position);
            }
        }

        /// <summary>
        /// Determines whether the specified player is nearer to an opponent than other specified player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="otherPlayer">The other player.</param>
        /// <returns>
        ///   <c>true</c> if <see cref="player"/> is nearer to an opponent than <see cref="otherPlayer"/>; otherwise, <c>false</c>.
        /// </returns>
        public bool IsNearerToOpponent(Player player, Player otherPlayer)
        {
            if (IsOnLeft)
                return player.Position.X > otherPlayer.Position.X;
            else
                return player.Position.X < otherPlayer.Position.X;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Team"/> class.
        /// </summary>
        /// <param name="footballPlayers">The team's football players.</param>
        /// <param name="ai">The <see cref="FsmAI"/> instance to which this instance belongs.</param>
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

        /// <summary>
        /// Loads the state. Updates position and movement vectors of all team's players accordingly.
        /// </summary>
        /// <param name="state">The game state.</param>
        /// <param name="firstTeam">If set to <c>true</c>, then the team is the first team from the match.</param>
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

                var distToBall = Vector.GetDistanceBetween(Players[i].Position, AI.Ball.Position);

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

        /// <summary>
        /// Gets the team's players' actions in the current state..
        /// </summary>
        /// <returns>The array of <see cref="PlayerAction"/> containing the actions of the team's players in the current state.</returns>
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

        /// <summary>
        /// Processes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ProcessMessage(IMessage message)
        {
            StateMachine.ProcessMessage(this, message);
        }

        /// <summary>
        /// Determines whether the pass from the controlling player to the specified target is safe from opponents.
        /// </summary>
        /// <param name="target">The target.</param>
        /// <returns>
        ///   <c>true</c> if the pass from the controlling player to the specified target is safe; otherwise, <c>false</c>.
        ///   If there isn't a controlling player, then returns <c>false</c>.
        /// </returns>
        public bool IsPassFromControllingSafe(Vector target)
        {
            return IsPassSafe(ControllingPlayer, target);
        }

        /// <summary>
        /// Determines whether the pass from the specified player to the specified target is safe from opponents.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="target">The target.</param>
        /// <returns>
        /// <c>true</c> if the pass from the specified player to the specified target is safe; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPassSafe(FootballPlayer player, Vector target)
        {
            var ball = AI.Ball;

            if (player == null)
                return false;

            var toBall = Vector.GetDifference(ball.Position, target);

            foreach (var opponent in AI.OpponentTeam.Players)
            {
                var toOpponent = Vector.GetDifference(opponent.Position, target);

                var k = Vector.GetDotProduct(toBall, toOpponent) / toBall.Length;
                var interposeTarget = Vector.GetSum(target, toBall.GetResized(k));
                var opponentToInterposeDist = Vector.GetDistanceBetween(opponent.Position, interposeTarget);

                var opponentToKickablePosition = new Vector(opponent.Position, interposeTarget,
                    Math.Max(0, opponentToInterposeDist - FootballBall.MaxDistanceForKick));

                var kickablePosition = Vector.GetSum(opponent.Position, opponentToKickablePosition);

                if (k > toBall.Length || k <= 0)
                    continue; // safe

                var ballToInterposeDist = Vector.GetDistanceBetween(ball.Position, interposeTarget);

                var t1 = ball.GetTimeToCoverDistance(ballToInterposeDist, player.MaxKickSpeed);
                var t2 = opponent.GetTimeToGetToTarget(kickablePosition);

                if (t2 < t1)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Determines whether the pass from the specified player to the specified target is possible.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="target">The target.</param>
        /// <param name="ball">The ball.</param>
        /// <returns>
        /// <c>true</c> if the pass from the specified player to the specified target is possible; otherwise, <c>false</c>.
        /// </returns>
        public bool IsPassPossible(FootballPlayer player, Vector target, FootballBall ball)
        {
            return !double.IsInfinity(
                ball.GetTimeToCoverDistance(Vector.GetDistanceBetween(ball.Position, target), player.MaxKickSpeed));
        }

        /// <summary>
        /// Tries to get the shot on goal. The shot must be safe from opponent.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="shotTarget">The shot target.</param>
        /// <returns>
        ///   <c>true</c> if <see cref="shotTarget"/> was set; otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetShotOnGoal(FootballPlayer player, out Vector shotTarget)
        {
            return TryGetShotOnGoal(player, out shotTarget, AI.Ball);
        }

        /// <summary>
        /// Tries to get the shot on goal with the specified ball. The shot must be safe from opponent.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="shotTarget">The shot target.</param>
        /// <param name="ball">The ball.</param>
        /// <returns>
        ///   <c>true</c> if <see cref="shotTarget"/> was set; otherwise, <c>false</c>.
        /// </returns>
        public bool TryGetShotOnGoal(FootballPlayer player, out Vector shotTarget, FootballBall ball)
        {
            for (int i = 0; i < Parameters.NumberOfGeneratedShotTargets; i++)
            {
                var target =
                    new Vector(0, GameClient.FieldHeight / 2.0 + (FsmAI.Random.NextDouble() - 0.5) * 7.32 / 2);
                if (IsOnLeft)
                    target.X = GameClient.FieldWidth;

                if (IsPassPossible(player, target, ball) && IsPassSafe(player, target))
                {
                    shotTarget = target;
                    return true;
                }
            }

            shotTarget = null;
            return false;
        }

        /// <summary>
        /// Tries to get the safe pass form the specified player to any other team's player.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="target">The target player.</param>
        ///   <c>true</c> if <see cref="target"/> was set; otherwise, <c>false</c>.
        public bool TryGetSafePass(Player player, out Player target)
        {
            return TryGetSafePass(player, out target, AI.Ball);
        }

        /// <summary>
        /// Tries to get the safe pass form the specified player to any other team's player with the specified ball.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="target">The target player.</param>
        /// <param name="ball">The ball.</param>
        /// <returns></returns>
        /// <c>true</c> if <see cref="target" /> was set; otherwise, <c>false</c>.
        public bool TryGetSafePass(Player player, out Player target, Ball ball)
        {
            target = null;

            foreach (var otherPlayer in Players)
            {
                if (player == otherPlayer)
                    continue;

                if (IsPassPossible(player, otherPlayer.Position, ball) && IsPassSafe(player, otherPlayer.Position))
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

        /// <summary>
        /// Predicts the nearest team's player, besides the specified <see cref="skippedPlayers"/>, 
        /// to the specified position in the specified time.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="time">The relative time to the current time.</param>
        /// <param name="skippedPlayers">The skipped players.</param>
        /// <returns>
        /// The nearest <see cref="Player"/> to the specified position in the specified time if there is at 
        /// least one player outside of the specified <see cref="skippedPlayers"/>; otherwise, null.
        /// </returns>
        public Player PredictNearestPlayerToPosition(Vector position, double time, params Player[] skippedPlayers)
        {
            var minPlayer = Players.FirstOrDefault(p => !skippedPlayers.Contains(p));
            if (minPlayer == null)
                return null; // all players are skipped

            var minDistSq = Vector.GetDistanceBetweenSquared(minPlayer.PredictPositionInTime(time), position);

            foreach (var player in Players)
            {
                if (skippedPlayers.Contains(player))
                    continue;

                var distSq = Vector.GetDistanceBetweenSquared(player.PredictPositionInTime(time), position);
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
