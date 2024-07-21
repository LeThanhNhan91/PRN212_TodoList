using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Validation
{
    public class FullNameValidation : IValidation
    {
        private string _fullName;
        private string _errorMessage;
        private bool _hasError;

        public string FullName
        {
            get { return _fullName; }
            set
            {
                _fullName = value;
                OnPropertyChanged(nameof(FullName));
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
            //kiểm tra FullName người dùng nhập vào theo tiêu chí: 
            // - FullName có độ dài dài từ 6 kí tự trở lên, nếu không thỏa thì ErrorMessage = "FullName must have more than 6 characters"
            //- FullName phải có ít nhất 1 khoảng trắng, nếu không thỏa thì ErrorMessage = "Following the format LastName FirstName"
            if (string.IsNullOrEmpty(FullName) || FullName.Length < 6)
            {
                ErrorMessage = "FullName must have more than 6 characters";
                HasError = true;
            }
            else if (!FullName.Contains(" "))
            {
                ErrorMessage = "Following the format LastName FirstName";
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
