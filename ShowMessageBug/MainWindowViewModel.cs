using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using ShowMessageBug.Annotations;
using ShowMessageBug.Commands;

namespace ShowMessageBug
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand ShowDialogWithInvokeCommand { get; set; }
        public ICommand ShowDialogWithoutInvokeCommand { get; set; }

        public MainWindowViewModel()
        {
            ShowDialogWithInvokeCommand = new AwaitableDelegateCommand(ShowDialogWithInvokeCommandExecute);
            ShowDialogWithoutInvokeCommand = new AwaitableDelegateCommand(ShowDialogWithoutInvokeCommandExecute);
        }

        public async Task ShowDialogWithInvokeCommandExecute()
        {
            var dialogSettings = new MetroDialogSettings{CancellationToken = new CancellationTokenSource(5000).Token};
            await Application.Current.Dispatcher.Invoke(async () => await DialogCoordinator.Instance.ShowMessageAsync(this, "Dialog Title", "Wait 5 seconds for dialog to timeout", MessageDialogStyle.AffirmativeAndNegative, dialogSettings));
        }

        public async Task ShowDialogWithoutInvokeCommandExecute()
        {
            var dialogSettings = new MetroDialogSettings{CancellationToken = new CancellationTokenSource(5000).Token};
            await DialogCoordinator.Instance.ShowMessageAsync(this, "Dialog Title", "Wait 5 seconds for dialog to timeout", MessageDialogStyle.AffirmativeAndNegative, dialogSettings);
        }
    }
}
