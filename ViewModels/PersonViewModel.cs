using System;
using System.Threading.Tasks;
using DateProject01.Tools;
using DateProject01.Views;
using DateProject01.Models;
using DateProject01.Tools.Managers;
using System.Threading;
using System.Windows;

namespace DateProject01.ViewModels
{
    public class PersonViewModel : BaseViewModel
    {
        #region Fields
        private string _name;
        private string _surname;
        private string _email;
        private DateTime _birthDate;
        #endregion

        #region Constants
        private string ErrorMessage = "There was a mistake. You have not yet been born or your age is above 135";
        #endregion

        #region Commands

        #endregion

        #region Properties
        public DateTime Birthday
        {
            get { return _birthDate; }
            set { _birthDate = value; OnPropertyChanged(); }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string Surname
        {
            get { return _surname; }
            set { _surname = value; OnPropertyChanged(); }
        }

        public string Email
        {
            get { return _email; }
            set { _email = value; OnPropertyChanged(); }
        }
        #endregion

        #region Functionality
        private int countAge()
        {
            var today = DateTime.Today;
            var age = today.Year - Birthday.Year;
            if (Birthday > today.AddYears(-age)) age--;

            return age;
        }
        #endregion

        #region Commands
        private RelayCommand<object> _signInCommand;

        public RelayCommand<object> RegisterCommand
        {
            get
            {
                return _signInCommand ?? (_signInCommand = new RelayCommand<object>(RegisterImpl,
                           o => !string.IsNullOrWhiteSpace(_name) &&
                                !string.IsNullOrWhiteSpace(_surname) &&
                                !string.IsNullOrWhiteSpace(_email)));
            }
        }

        private async void RegisterImpl(object o)
        {
            LoaderManager.Instance.ShowLoader();
            await Task.Run(() => Thread.Sleep(1000));
            LoaderManager.Instance.HideLoader();

            if (countAge() < 0 || countAge() > 135)
            {
                MessageBox.Show(ErrorMessage);
            }
            else
            {
                Person person = null;

                await Task.Run((() =>
                {
                    person = new Person(_name, _surname, _email, _birthDate);
                }));

                PersonInfoWindow personInfoWindow = new PersonInfoWindow(person);
                personInfoWindow.Show();
            }
        }
        #endregion

    }
}
