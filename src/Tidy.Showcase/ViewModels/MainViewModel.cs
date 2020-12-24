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
    public class MainViewModel : ViewModelBase
    {
        public MvvmViewModel Mvvm { get; } = new MvvmViewModel();
    }
}
