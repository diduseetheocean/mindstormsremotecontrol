using System.Threading.Tasks;
using RemoteControl.ViewModels;

namespace RemoteControl.CrossDependency
{
    public interface IEV3Logic
    {
        Task<bool> ConnectEV3(string name);
        Task<bool> DisconnectEV3();
        void MoveForward();
        void MoveBackward();
        void TurnLeft();
        void TurnRight();
        void StopAll();
    }
}
