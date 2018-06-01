using RemoteControl.Models;
using System.Collections.Generic;


namespace RemoteControl.CrossDependency
{
    public interface IBluetooth
    {
        List<BluetoothDeviceModel> GetBondedBluetoothDevices();
    }
}
