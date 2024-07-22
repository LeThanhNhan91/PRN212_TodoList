using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Services.Validation
{
    public class PasswordValidation : IValidation
    {
        private string _password;
        private string _errorMessage;
        private bool _hasError;

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                Validate();
            }
        }
        public string ErrorMessage
        {
            get { return _errorMessage; }
            private set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool HasError
        {
            get { return _hasError; }
            private set
            {
                _hasError = value;
                OnPropertyChanged(nameof(HasError));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void Validate()
        {
            if (string.IsNullOrEmpty(Password) || Password.Length < 6)
            {
                ErrorMessage = "Must have more than 6 characters";
                HasError = true;
            }
            else if (!Regex.IsMatch(Password, @"[a-z]") || !Regex.IsMatch(Password, @"[A-Z]") || !Regex.IsMatch(Password, @"\d") || !Regex.IsMatch(Password, @"[\W_]"))
            {
                ErrorMessage = "Must have A, a, @, 1";
                HasError = true;
            }
            else
            {
                ErrorMessage = string.Empty;
                HasError = false;
            }

        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
