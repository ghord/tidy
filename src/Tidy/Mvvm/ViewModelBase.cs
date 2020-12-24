using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tidy.Mvvm
{
    /// <summary>
    /// Base class for view models
    /// </summary>
    public abstract class ViewModelBase : INotifyPropertyChanged
    {
        /// <inheritdoc />
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Raises property changed notification for given property
        /// </summary>
        /// <param name="propertyName">Property name</param>
        protected void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Sets the field value and raises notification if the value changed
        /// </summary>
        /// <typeparam name="T">Type of the field</typeparam>
        /// <param name="field">Field reference</param>
        /// <param name="value">Value </param>
        /// <param name="propertyName">Property that change notification is raised for</param>
        /// <returns>True if change notification was raised</returns>
        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if(!Equals(field, value))
            {
                field = value;
                RaisePropertyChanged(propertyName);
                return true;
            }

            return false;
        }
    }
}
