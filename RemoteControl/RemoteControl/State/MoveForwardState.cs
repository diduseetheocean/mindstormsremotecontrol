using System;
using RemoteControl.CrossDependency;
using RemoteControl.Models;
using Xamarin.Forms;

namespace RemoteControl.State
{
    public class MoveForwardState : IState
    {
        public string StateName { get => "MoveForward"; }

        public void MoveBackward(RobotStateMachine stateMaschine)
        {
            DependencyService.Get<IEV3Logic>().MoveBackward();
            stateMaschine.CurrentState = new MoveBackwardState();
        }

        public void MoveForward(RobotStateMachine stateMaschine)
        {}

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
