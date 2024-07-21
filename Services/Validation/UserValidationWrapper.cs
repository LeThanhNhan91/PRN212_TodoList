using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Validation
{
    public class UserValidationWrapper : INotifyPropertyChanged
    {
        private EmailValidation _emailValidation;
        private FullNameValidation _fullNameValidation;
        private PasswordValidation _passsWordValidation;

        public EmailValidation EmailValidation
        {
            get { return _emailValidation; }
            set
            {
                _emailValidation = value;
                OnPropertyChanged(nameof(EmailValidation));
            }
        }

        public FullNameValidation FullNameValidation
        {
            get { return _fullNameValidation; }
            set
            {
                _fullNameValidation = value;
                OnPropertyChanged(nameof(FullNameValidation));
            }
        }

        public PasswordValidation PasswordValidation
        {
            get { return _passsWordValidation; }
            set
            {
                _passsWordValidation = value;
                OnPropertyChanged(nameof(PasswordValidation));
            }
        }

        public UserValidationWrapper()
        {
            EmailValidation = new EmailValidation();
            FullNameValidation = new FullNameValidation();
            PasswordValidation = new PasswordValidation();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
