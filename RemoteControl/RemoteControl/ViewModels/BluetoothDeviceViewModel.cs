using RemoteControl.CrossDependency;
using RemoteControl.Models;
using System.ComponentModel;
using Xamarin.Forms;
using RemoteControl.Helper;
using System.Windows.Input;

namespace RemoteControl.ViewModels
{
    public class BluetoothDeviceViewModel : INotifyPropertyChanged
    {
        private MainPageViewModel _mainPageViewModel;
        private bool _selected;

        public BluetoothDeviceModel BluetoothDeviceModel { get; set; }
        public ICommand ConnectEV3Cmd { get; set; }

        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnPropertyChanged("Selected");
            }
        }

        public BluetoothDeviceViewModel(MainPageViewModel mainPageViewModel)
        {
            this._mainPageViewModel = mainPageViewModel;
            InitCommand();
        }

        private void InitCommand()
        {
            this.ConnectEV3Cmd =  new Command(() => TryConnectEV3());
        }

        public async void TryConnectEV3()
        {
            CustomNotify.ShowLoading();
            this._mainPageViewModel.ConnectButtonEnabled = false;
            if (!this._mainPageViewModel.EV3Connected)
            {
                var success = await DependencyService.Get<IEV3Logic>().ConnectEV3(BluetoothDeviceModel.Name);
                if (success)
                {
                    this._mainPageViewModel.ConnectButtonText = "Disconnect";
                    this._mainPageViewModel.ManualNavigationEnabled = true;
                    this._mainPageViewModel.NavigationButtonsEnabled = true;
                    this._mainPageViewModel.AccelerometerNavigationEnabled = true;
                    this._mainPageViewModel.UsingManualNavigation = true;
                    this._mainPageViewModel.EV3Connected = true;
                    this._mainPageViewModel.BluetoothDeviceListViewEnabled = false;
                }
                else
                {
                    CustomNotify.ShowToast("Connection failure");
                }
                CustomNotify.HideLoading();
                this._mainPageViewModel.ConnectButtonEnabled = true;
            }
            else
            {
                var success = await DependencyService.Get<IEV3Logic>().DisconnectEV3();
                if (success)
                {
                    this._mainPageViewModel.ConnectButtonText = "Connect";
                    this._mainPageViewModel.ManualNavigationEnabled = false;
                    this._mainPageViewModel.AccelerometerNavigationEnabled = false;
                    this._mainPageViewModel.EV3Connected = false;
                    this._mainPageViewModel.BluetoothDeviceListViewEnabled = true;
                }
                else
                {
                    CustomNotify.ShowToast("Connection failure");
                }
                CustomNotify.HideLoading();
                this._mainPageViewModel.ConnectButtonEnabled = true;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
