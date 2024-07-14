using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList
{
    public class Todos : INotifyPropertyChanged
    {

        public Todos()
        {
            _allTodos = new ObservableCollection<Todo>() 
            {
                new Todo() { Desc = "Throw the trash"},
                new Todo() { Desc = "Walk the dog"},
                new Todo() { Desc = "Brush teeth"}
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void OnPropertyChanged (string propertyName)
        {
            //method to invoke the event
            PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private ObservableCollection<Todo> _allTodos;
        public ObservableCollection<Todo> AllTodos
        {
            get { return _allTodos; }
            set 
            { 
                _allTodos = value;
                OnPropertyChanged(nameof(AllTodos));
            }
        }
    }
}
