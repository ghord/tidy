using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tidy.Mvvm;

namespace Tidy.Showcase.ViewModels
{
    public class MvvmViewModel
    {

        public MvvmViewModel()
        {
            RelayActionCommand = new RelayCommand(() => MessageBox.Show("RelayCommand"));

            AsyncRelayActionCommand = new RelayCommand(async () =>
            {
                await Task.Delay(1000);
                MessageBox.Show("async RelayCommand");
            });
        }

        public ICommand RelayActionCommand { get; }
        public ICommand AsyncRelayActionCommand { get; }
    }
}
