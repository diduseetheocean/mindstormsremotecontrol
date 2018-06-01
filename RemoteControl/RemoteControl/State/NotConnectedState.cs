using System;
using RemoteControl.Models;

namespace RemoteControl.State
{
    public class NotConnectedState : IState
    {
        public string StateName => "Not connected";

        public void MoveBackward(RobotStateMachine stateMaschine)
        {}

        public void MoveForward(RobotStateMachine stateMaschine)
        {}

        public void StopAll(RobotStateMachine stateMaschine)
        {}

        public void TurnLeft(RobotStateMachine stateMaschine)
        {}

        public void TurnRight(RobotStateMachine stateMaschine)
        {}
    }
}
