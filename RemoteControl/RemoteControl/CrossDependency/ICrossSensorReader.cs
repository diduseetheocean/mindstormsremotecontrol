using RemoteControl.Helper;

namespace RemoteControl.CrossDependency
{
    public interface ICrossSensorReader
    {
        ICrossSensorDataReceiver mainPageViewModel { get; set; }
        float X { get; set; }
        float Y { get; set; }
        float Z { get; set; }
        void RegisterSensorListener(EnumCustomSensorType sensorType);
        void RemoveSensorListener();
    }
}
