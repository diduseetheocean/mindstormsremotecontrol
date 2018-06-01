using System;
using RemoteControl.CrossDependency;
using RemoteControl.Models;
using Xamarin.Forms;

namespace RemoteControl.State
{
    public class TurnLeftState : IState
    {
        public string StateName { get => "TurnLeft"; }

        public void MoveBackward(RobotStateMachine stateMaschine)
        {
            DependencyService.Get<IEV3Logic>().MoveBackward();
            stateMaschine.CurrentState = new MoveBackwardState();
        }

        public void MoveForward(RobotStateMachine stateMaschine)
        {
            DependencyService.Get<IEV3Logic>().MoveForward();
            stateMaschine.CurrentState = new MoveForwardState();
        }

        public void StopAll(RobotStateMachine stateMaschine)
        {
            DependencyService.Get<IEV3Logic>().StopAll();
            stateMaschine.CurrentState = new IdleState();
        }

        public void TurnLeft(RobotStateMachine stateMaschine)
        {}

        public void TurnRight(RobotStateMachine stateMaschine)
        {
            DependencyService.Get<IEV3Logic>().TurnRight();
            stateMaschine.CurrentState = new TurnRightState();
        }
    }
}
