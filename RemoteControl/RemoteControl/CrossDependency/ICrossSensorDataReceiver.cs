using System;
namespace RemoteControl.CrossDependency
{
    public interface ICrossSensorDataReceiver
    {
        float XCrossSensor { get; set; }
        float YCrossSensor { get; set; }
        float ZCrossSensor { get; set; }
    }
}
