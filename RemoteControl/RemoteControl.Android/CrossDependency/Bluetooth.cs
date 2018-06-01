using Android.Bluetooth;
using RemoteControl.CrossDependency;
using RemoteControl.Droid.CrossDependency;
using System.Collections.Generic;
using System;
using RemoteControl.Models;
using Android.Widget;

[assembly: Xamarin.Forms.Dependency(typeof(Bluetooth))]
namespace RemoteControl.Droid.CrossDependency
{
    class Bluetooth : IBluetooth
    {
        public List<BluetoothDeviceModel> GetBondedBluetoothDevices()
        {
            List<BluetoothDeviceModel> deviceList = new List<BluetoothDeviceModel>();
            try
            {
                var devices = BluetoothAdapter.DefaultAdapter.BondedDevices;
                int counter = 0;
                foreach (var device in devices)
                {
                    var bluetoothDeviceModel = new BluetoothDeviceModel
                    {
                        Name = device.Name,
                        Id = counter
                    };
                    deviceList.Add(bluetoothDeviceModel);
                    counter++;
                }
            }
            catch(Exception ex)
            {
                Toast.MakeText(Android.App.Application.Context, ex.Message, ToastLength.Long);
            }
            return deviceList;
        }
    }
}