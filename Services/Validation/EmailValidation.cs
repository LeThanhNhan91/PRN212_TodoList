using System;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Services.Validation
{
    public class EmailValidation : IValidation
    {
        private string _email;
        private string _errorMessage;
        private bool _hasError;

        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
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

        public void Validate()
        {
            if (string.IsNullOrEmpty(Email) || !Regex.IsMatch(Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                ErrorMessage = "Please enter the correct format: example@gmail.com";
                HasError = true;
            }
            else
            {
                ErrorMessage = string.Empty;
                HasError = false;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
