using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Pavlo.MyHelpers.MVVM
{
    /// <summary>
    /// base realization of the INPC using .net 4.5 (Caller Information Attributes)
    /// </summary>
    public class INPCBaseDotNet4_5 : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        //.net4.5
        protected virtual void NotifyPropertyChanged([CallerMemberName]string propertyName="")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public INPCBaseDotNet4_5()
        { }
    }
}
