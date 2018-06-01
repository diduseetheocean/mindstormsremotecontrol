using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
using RemoteControl.CrossDependency;
using RemoteControl.Models;
using RemoteControl.State;
using Xamarin.Forms;

namespace RemoteControl.ViewModels
{
    public class MainPageViewModel : INotifyPropertyChanged, ICrossSensorDataReceiver
    {
        private bool _manualNavigationEnabled;
        private bool _accelerometerNavigationEnabled;
        private bool _navigationButtonsEnabled;
        private bool _calibrateButtonEnabled;
        private bool _usingManualNavigation;
        private bool _usingAccelerationNavigation;
        private bool _bluetoothDeviceListViewEnabled;
        private bool _connectButtonEnabled;
        private bool _calibrated;
        private bool _ev3Connected;
        private string _connectButtonText;
        private string _calibrateButtonText;
        private string _currentStateName;
        private BluetoothDeviceViewModel _selectedBluetoothDevice;

        private float _xCrossSensor;
        private float _yCrossSensor;
        private float _zCrossSensor;
        private float _xCalibrated; 
        private float _yCalibrated; 
        private float _zCalibrated;

        private RobotStateMachine stateMachine;

        public ObservableCollection<BluetoothDeviceViewModel> BluetoothDeviceCollection { set; get; }
        public ICommand ForwardCmd { get; set; }
        public ICommand BackwardCmd { get; set; }
        public ICommand LeftCmd { get; set; }
        public ICommand RightCmd { get; set; }
        public ICommand StopCmd { get; set; }
        public ICommand CalibrateCmd { get; set; }

        public float XCrossSensor
        {
            get
            {
                return (float)Math.Round(_xCrossSensor, 1);
            }
            set
            {
                _xCrossSensor = value;
                MoveViaAccelerometer();
                OnPropertyChanged("XCrossSensor");
            }
        }

        public float YCrossSensor
        {
            get
            {
                return (float)Math.Round(_yCrossSensor, 1); ;
            }
            set
            {
                _yCrossSensor = value;
                OnPropertyChanged("YCrossSensor");
            }
        }

        public float ZCrossSensor
        {
            get
            {
                return (float)Math.Round(_zCrossSensor, 1); ;
            }
            set
            {
                _zCrossSensor = value;
                MoveViaAccelerometer();
                OnPropertyChanged("ZCrossSensor");
            }
        }

        public bool BluetoothDeviceListViewEnabled
        {
            get { return _bluetoothDeviceListViewEnabled; }
            set
            {
                _bluetoothDeviceListViewEnabled = value;
                OnPropertyChanged("BluetoothDeviceListViewEnabled");
            }
        }

        public bool ManualNavigationEnabled
        {
            get { return _manualNavigationEnabled; }
            set
            {
                _manualNavigationEnabled = value;
                if(!_manualNavigationEnabled)
                {
                    _usingManualNavigation = false;
                    _navigationButtonsEnabled = false;
                }
                OnPropertyChanged("ManualNavigationEnabled");
                OnPropertyChanged("UsingManualNavigation");
                OnPropertyChanged("NavigationButtonsEnabled");
            }
        }

        public bool AccelerometerNavigationEnabled
        {
            get { return _accelerometerNavigationEnabled; }
            set
            {
                _accelerometerNavigationEnabled = value;
                if (!_accelerometerNavigationEnabled)
                {
                    _usingAccelerationNavigation = false;
                    _calibrateButtonEnabled = false;
                }
                OnPropertyChanged("AccelerometerNavigationEnabled");
                OnPropertyChanged("UsingAccelerationNavigation");
            }
        }

        public bool NavigationButtonsEnabled
        {
            get { return _navigationButtonsEnabled; }
            set
            {
                _navigationButtonsEnabled = value;
                OnPropertyChanged("NavigationButtonsEnabled");
            }
        }

        public bool CalibrateButtonEnabled
        {
            get { return _calibrateButtonEnabled; }
            set
            {
                _calibrateButtonEnabled = value;
                OnPropertyChanged("CalibrateButtonEnabled");
            }
        }

        public bool UsingManualNavigation
        {
            get { return _usingManualNavigation; }
            set
            {
                _usingManualNavigation = value;
                if (_usingManualNavigation)
                {
                    _usingAccelerationNavigation = false;
                    _calibrateButtonEnabled = false;
                    RemoveSensors();
                }
                else
                {
                    StopAll();
                }
                _navigationButtonsEnabled = value;
                OnPropertyChanged("CalibrateButtonEnabled");
                OnPropertyChanged("UsingAccelerationNavigation");
                OnPropertyChanged("NavigationButtonsEnabled");
                OnPropertyChanged("UsingManualNavigation");
            }
        }

        public bool UsingAccelerationNavigation
        {
            get { return _usingAccelerationNavigation; }
            set
            {
                _usingAccelerationNavigation = value;
                if (_usingAccelerationNavigation)
                {
                    _usingManualNavigation = false;
                    _navigationButtonsEnabled = false;
                    InitSensors();
                }
                else
                {
                    RemoveSensors();
                    StopAll();
                }
                _calibrateButtonEnabled = value;
                OnPropertyChanged("CalibrateButtonEnabled");
                OnPropertyChanged("UsingManualNavigation");
                OnPropertyChanged("NavigationButtonsEnabled");
                OnPropertyChanged("UsingAccelerationNavigation");
            }
        }

        public bool ConnectButtonEnabled
        {
            get { return _connectButtonEnabled; }
            set
            {
                _connectButtonEnabled = value;
                OnPropertyChanged("ConnectButtonEnabled");
            }
        }

        public String ConnectButtonText
        {
            get { return _connectButtonText; }
            set
            {
                _connectButtonText = value;
                OnPropertyChanged("ConnectButtonText");
            }
        }

        public String CalibrateButtonText
        {
            get { return _calibrateButtonText; }
            set
            {
                _calibrateButtonText = value;
                OnPropertyChanged("CalibrateButtonText");
            }
        }

        public String CurrentStateName
        {
            get { return _currentStateName; }
            set
            {
                _currentStateName = value;
                OnPropertyChanged("CurrentStateName");
            }
        }

        public BluetoothDeviceViewModel SelectedBluetoothDevice
        {
            get
            {
                return _selectedBluetoothDevice;
            }
            set
            {
                if(_selectedBluetoothDevice != null) _selectedBluetoothDevice.Selected = false;
                _selectedBluetoothDevice = value;
                if(_selectedBluetoothDevice != null)
                {
                    _selectedBluetoothDevice.Selected = true;
                    ConnectButtonEnabled = true;
                }
                else
                {
                    ConnectButtonEnabled = false;
                }
                OnPropertyChanged("SelectedBluetoothDevice");
            }
        }

        public bool EV3Connected
        {
            get
            {
                return _ev3Connected; 
            }
            set
            {
                _ev3Connected = value;
                if(_ev3Connected)
                {
                    this.stateMachine.CurrentState = new IdleState();
                    CurrentStateName = this.stateMachine.CurrentState.StateName; 
                }
                else
                {
                    this.stateMachine.CurrentState = new NotConnectedState();
                    CurrentStateName = this.stateMachine.CurrentState.StateName;
                }
            }
        }

        public MainPageViewModel()
        {
            this.stateMachine = new RobotStateMachine(new NotConnectedState());
            CurrentStateName = this.stateMachine.CurrentState.StateName;

            ConnectButtonText = "Connect";
            CalibrateButtonText = "Calibrate";
            BluetoothDeviceListViewEnabled = true;

            InitBluetoothDevices();
            InitCommands();
        }

        private void InitSensors()
        {
            var crossSensorReader = DependencyService.Get<ICrossSensorReader>();
            crossSensorReader.mainPageViewModel = this;
            crossSensorReader.RegisterSensorListener(Helper.EnumCustomSensorType.GameRotationVector);
        }

        private void RemoveSensors()
        {
            var crossSensorReader = DependencyService.Get<ICrossSensorReader>();
            crossSensorReader.RemoveSensorListener();
            XCrossSensor = 0;
            YCrossSensor = 0;
            ZCrossSensor = 0;
            _xCalibrated = 0;
            _yCalibrated = 0;
            _zCalibrated = 0;
            CalibrateButtonText = "Calibrate";
            _calibrated = false;
        }

        private void InitCommands()
        {
            this.ForwardCmd = new Command(() => MoveForward());
            this.BackwardCmd = new Command(() => MoveBackward());
            this.LeftCmd = new Command(() => TurnLeft());
            this.RightCmd = new Command(() => TurnRight());
            this.StopCmd = new Command(() => StopAll());
            this.CalibrateCmd = new Command(() => Calibrate());
        }

        private void InitBluetoothDevices()
        {
            BluetoothDeviceCollection = new ObservableCollection<BluetoothDeviceViewModel>();
            var devices = DependencyService.Get<IBluetooth>().GetBondedBluetoothDevices();
            foreach (var device in devices)
            {
                var bluetoothDeviceViewModel = new BluetoothDeviceViewModel(this)
                {
                    BluetoothDeviceModel = device
                };
                BluetoothDeviceCollection.Add(bluetoothDeviceViewModel);
            }
        }

        private void MoveViaAccelerometer()
        {
            var offset = 0.15f;

            if(!UsingAccelerationNavigation || !_calibrated)
            {
                return;
            }

            if (ZCrossSensor > _zCalibrated + offset)
            {
                TurnLeft();
            }
            else if (ZCrossSensor < _zCalibrated - offset)
            {
                TurnRight();
            }
            else if (XCrossSensor < _xCalibrated - offset)
            {
                MoveForward();
            }
            else if (XCrossSensor > _xCalibrated + offset)
            {
                MoveBackward();
            }
            else
            {
                StopAll();
            }
        }

        private void MoveForward()
        {
            this.stateMachine.MoveForward();
            this.CurrentStateName = this.stateMachine.CurrentState.StateName;
        }

        private void MoveBackward()
        {
            this.stateMachine.MoveBackward();
            this.CurrentStateName = this.stateMachine.CurrentState.StateName;
        }

        private void TurnLeft()
        {
            this.stateMachine.TurnLeft();
            this.CurrentStateName = this.stateMachine.CurrentState.StateName;
        }

        private void TurnRight()
        {
            this.stateMachine.TurnRight();
            this.CurrentStateName = this.stateMachine.CurrentState.StateName;
        }

        private void StopAll()
        {
            this.stateMachine.StopAll();
            this.CurrentStateName = this.stateMachine.CurrentState.StateName;
        }

        private void Calibrate()
        {
            if(!_calibrated)
            {
                _xCalibrated = _xCrossSensor;
                _yCalibrated = _yCrossSensor;
                _zCalibrated = _zCrossSensor;
                CalibrateButtonText = "Reset Calibration";
                _calibrated = true;
            }
            else
            {
                _xCalibrated = _xCrossSensor;
                _yCalibrated = _yCrossSensor;
                _zCalibrated = _zCrossSensor;
                CalibrateButtonText = "Calibrate";
                _calibrated = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
