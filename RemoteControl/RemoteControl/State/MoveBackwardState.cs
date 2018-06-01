using System;
using RemoteControl.CrossDependency;
using RemoteControl.Models;
using Xamarin.Forms;

namespace RemoteControl.State
{
    public class MoveBackwardState : IState
    {
        public string StateName { get => "MoveBackward"; }

        public void MoveBackward(RobotStateMachine stateMaschine)
        {}

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
        {
            DependencyService.Get<IEV3Logic>().TurnLeft();
            stateMaschine.CurrentState = new TurnLeftState();
        }

        public void TurnRight(RobotStateMachine stateMaschine)
        {
            DependencyService.Get<IEV3Logic>().TurnRight();
            stateMaschine.CurrentState = new TurnRightState();
        }
    }
}
