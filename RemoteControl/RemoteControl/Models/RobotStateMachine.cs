using RemoteControl.State;

namespace RemoteControl.Models
{
    public class RobotStateMachine
    {
        public IState CurrentState { set; get; }

        public RobotStateMachine(IState stateMachine)
        {
            CurrentState = stateMachine;
        }

        public void MoveForward()
        {
            CurrentState.MoveForward(this);
        }

        public void MoveBackward()
        {
            CurrentState.MoveBackward(this);
        }

        public void TurnLeft()
        {
            CurrentState.TurnLeft(this);
        }

        public void TurnRight()
        {
            CurrentState.TurnRight(this);
        }

        public void StopAll()
        {
            CurrentState.StopAll(this);
        }
    }
}
