﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
#pragma warning disable CS8618
#pragma warning disable CS8625

namespace RevitConsole
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if ( !EqualityComparer<T>.Default.Equals(field, value) )
            {
                field = value;
                OnPropertyChanged(propertyName);
                return true;
            }
            return false;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
