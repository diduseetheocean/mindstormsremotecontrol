using System;
using Android.Content;
using Android.Hardware;
using Android.Runtime;
using RemoteControl.CrossDependency;
using RemoteControl.Droid.CrossDependency;
using RemoteControl.Helper;

[assembly: Xamarin.Forms.Dependency(typeof(CrossSensorReader))]
namespace RemoteControl.Droid.CrossDependency
{
    public class CrossSensorReader : Java.Lang.Object, ICrossSensorReader, ISensorEventListener
    {
        public ICrossSensorDataReceiver mainPageViewModel { get; set; }

        static readonly Object _syncLock = new Object();
        SensorManager _sensorManager;

        private float _x;
        private float _y;
        private float _z;

        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                _x = value;
                this.mainPageViewModel.XCrossSensor = _x;
            }
        }

        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                _y = value;
                this.mainPageViewModel.YCrossSensor = _y;
            }
        }

        public float Z
        {
            get
            {
                return _z;
            }
            set
            {
                _z = value;
                this.mainPageViewModel.ZCrossSensor = _z;
            }
        }

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        }

        public void OnSensorChanged(SensorEvent e)
        {
            lock (_syncLock)
            {
                X = e.Values[0];
                Y = e.Values[1];
                Z = e.Values[2];
            }
        }

        public void RegisterSensorListener(EnumCustomSensorType sensorType)
        {
            SensorType aSensorType;
            switch (sensorType)
            {
                case EnumCustomSensorType.Accelerometer:
                    aSensorType = SensorType.Accelerometer;
                    break;
                case EnumCustomSensorType.LinearAccelerometer:
                    aSensorType = SensorType.LinearAcceleration;
                    break;
                case EnumCustomSensorType.RotationVector:
                    aSensorType = SensorType.RotationVector;
                    break;
                case EnumCustomSensorType.GameRotationVector:
                    aSensorType = SensorType.GameRotationVector;
                    break;
                case EnumCustomSensorType.Gyroscope:
                    aSensorType = SensorType.Gyroscope;
                    break;
                default:
                    aSensorType = SensorType.Accelerometer;
                    break;
            }

            _sensorManager = (SensorManager)Android.App.Application.Context.GetSystemService(Context.SensorService);

            var sensodrType = sensorType;
            _sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(aSensorType), SensorDelay.Ui);
        }

        public void RemoveSensorListener()
        {
            if(_sensorManager != null)
            {
                _sensorManager.UnregisterListener(this);
            }
        }
    }
}
