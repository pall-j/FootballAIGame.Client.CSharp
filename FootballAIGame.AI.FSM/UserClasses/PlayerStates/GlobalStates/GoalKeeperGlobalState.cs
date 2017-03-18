using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class GoalKeeperGlobalState : PlayerState
    {
        private PlayerGlobalState PlayerGlobalState { get; set; }

        public GoalKeeperGlobalState(Player player, Ai ai) : base(player, ai)
        {
            PlayerGlobalState = new PlayerGlobalState(player, ai);
        }

        public override void Run()
        {
            if (Player.CanKickBall(Ai.Ball))
            {
                Player passTargetPlayer;
                if (Ai.MyTeam.TryGetSafePass(Player, out passTargetPlayer))
                {
                    var passTarget = Player.PassBall(Ai.Ball, passTargetPlayer);
                    MessageDispatcher.Instance.SendMessage(new ReceivePassMessage(passTarget), passTargetPlayer);
                }
                else
                {
                    // find a safe direction and kick the ball there
                    var x = GameClient.FieldWidth / 2;
                    var safeDirectionFound = false;

                    for (int y = 10; y < GameClient.FieldHeight; y += 5)
                    {
                        var target = new Vector(x, y);
                        if (Ai.MyTeam.IsKickSafe(Player, target))
                        {
                            Player.KickBall(Ai.Ball, target);
                            safeDirectionFound = true;
                            break;
                        }
                    }

                    if (!safeDirectionFound)
                    {
                        // kick randomly
                        var target = new Vector(x, Ai.Random.Next(1, (int)GameClient.FieldHeight - 1));
                        Player.KickBall(Ai.Ball, target);
                    }
                }
            }
            PlayerGlobalState.Run();
        }

        public override bool ProcessMessage(IMessage message)
        {
            if (message is ReceivePassMessage)
            {
                Player.StateMachine.ChangeState(new InterceptBall(Player, Ai));
                return true;
            }
            if (message is GoDefaultMessage)
            {
                Player.StateMachine.ChangeState(new DefendGoal(Player, Ai));
                return true;
            }

            return PlayerGlobalState.ProcessMessage(message);
        }
    }
}
