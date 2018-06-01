using RemoteControl.Models;

namespace RemoteControl.State
{
    public interface IState
    {
        string StateName { get; }
        void MoveForward(RobotStateMachine stateMaschine);
        void MoveBackward(RobotStateMachine stateMaschine);
        void TurnLeft(RobotStateMachine stateMaschine);
        void TurnRight(RobotStateMachine stateMaschine);
        void StopAll(RobotStateMachine stateMaschine);
    }
}
