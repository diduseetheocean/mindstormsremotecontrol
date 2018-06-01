using RemoteControl.CrossDependency;
using RemoteControl.Droid.CrossDependency;
using RemoteControl.ViewModels;
using MonoBrick.EV3;
using System;
using Android.Widget;
using Android.Views;
using System.Threading.Tasks;

[assembly: Xamarin.Forms.Dependency(typeof(PlatformLogic))]
namespace RemoteControl.Droid.CrossDependency
{
    class PlatformLogic : IEV3Logic
    {
        static Android.Bluetooth.BluetoothDevice device;
        static MonoBrick.Bluetooth<Command, Reply> connection;
        Brick<Sensor, Sensor, Sensor, Sensor> brick;
        private sbyte speed;

        public async Task<bool> ConnectEV3(string name)
        {
            var value = false;
            await Task.Run(() =>
            {
                device = MonoBrick.Bluetooth<Command, Reply>.GetBondDevice(name);
                connection = new MonoBrick.Bluetooth<Command, Reply>(device);
                this.brick = new Brick<Sensor, Sensor, Sensor, Sensor>(connection);
                try
                {
                    this.brick.Connection.Open();
                    brick.Vehicle.LeftPort = MotorPort.OutA;
                    brick.Vehicle.RightPort = MotorPort.OutC;
                    brick.Vehicle.ReverseLeft = false;
                    brick.Vehicle.ReverseRight = false;
                    this.speed = 50;

                    value = true;
                }
                catch (MonoBrick.ConnectionException ex)
                {
                    device = null;
                    connection = null;
                    this.brick = null;
                }
            });
            return value;
        }

        public async Task<bool> DisconnectEV3()
        {
            var value = false;
            await Task.Run(() =>
            {
                try
                {
                    StopAll();
                    this.brick.Connection.Close();
                    device = null;
                    connection = null;
                    this.brick = null;

                    value = true;
                }
                catch (MonoBrick.ConnectionException ex)
                {}
            });
            return value;
        }

        public void MoveForward()
        {
            if(connection != null)
            {
                brick?.Vehicle.Forward(speed); 
            }
        }

        public void MoveBackward()
        {
            if (connection != null)
            {
                this.brick?.Vehicle.Backward(speed);
            }
        }

        public void TurnLeft()
        {
            if (connection != null)
            {
                this.brick?.Vehicle.SpinLeft(speed);
            }
        }

        public void TurnRight()
        {
            if (connection != null)
            {
                this.brick?.Vehicle.SpinRight(speed);
            }
        }

        public void StopAll()
        {
            if (connection != null)
            {
                this.brick?.Vehicle.Brake();
            }
        }
    }
}