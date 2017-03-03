using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class PlayerGlobalState : PlayerState
    {
        public override void Run()
        {
        }

        public override bool ProcessMessage(Message message)
        {
            if (message is ReturnToHomeMessage)
            {
            //    if (Player is GoalKeeper)
            //        Console.WriteLine("State change: Return home msg -> MoveToHomeRegion");
                Player.StateMachine.ChangeState(new MoveToHomeRegion(Player));

                return true;
            }

            if (message is SupportControllingMessage)
            {
                if (!(Player.StateMachine.CurrentState is SupportControlling))
                {
             //       if (Player is GoalKeeper)
             //           Console.WriteLine("State change: SupportMsg -> SupportControlling");
                    Player.StateMachine.ChangeState(new SupportControlling(Player));

                }

                return true;
            }

            if (message is GoDefaultMessage)
            {
             //   if (Player is GoalKeeper)
            //        Console.WriteLine("State change: goDefaultMsg -> Default");
                Player.StateMachine.ChangeState(new Default(Player));

                return true;
            }

            if (message is PassToPlayerMessage)
            {
                var ball = Ai.Instance.Ball;
                var target = ((PassToPlayerMessage) message).Receiver;

                var time = ball.TimeToCoverDistance(Vector.DistanceBetween(target.Position, ball.Position),
                    Player.MaxKickSpeed);
                var predictedTargetPosition = target.PredictedPositionInTime(time);

                if (Player.CanKickBall(ball))
                {
                    Player.KickBall(ball, predictedTargetPosition);
                    MessageDispatcher.Instance.SendMessage(new ReceivePassMessage(predictedTargetPosition));
               //     if (Player is GoalKeeper)
               //         Console.WriteLine("State change: pass message -> default");
                    Player.StateMachine.ChangeState(new Default(Player));

                }

                return true;
            }

            if (message is ReceivePassMessage)
            {
                var msg = (ReceivePassMessage) message;
              //  if (Player is GoalKeeper)
              //      Console.WriteLine("State change: receive message -> receive");
                Player.StateMachine.ChangeState(new ReceivePass(Player, msg.PassTarget));

                return true;
            }

            if (message is PursueBallMessage)
            {
               // if (Player is GoalKeeper)
              //      Console.WriteLine("State change: pursue message -> pursue");
                Player.StateMachine.ChangeState(new PursueBall(Player));
                return true;
            }

            return false;
        }

        public PlayerGlobalState(Player entity) : base(entity)
        {
        }
    }
}
