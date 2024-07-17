using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Validation
{
    public interface IValidation : INotifyPropertyChanged
    {
        string ErrorMessage { get; }  
        bool HasError { get; }
        void Validate();
        
    }
}
