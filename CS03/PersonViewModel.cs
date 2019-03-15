using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CS03.Exceptions;
using CS03.Tools;

namespace CS03
{
	internal class PersonViewModel : INotifyPropertyChanged, ILoaderOwner
	{
		#region Fields

		private string _first;
		private string _last;
		private string _date;
		private string _address;
		private Visibility _loaderVisibility = Visibility.Hidden;
		private bool _isControlEnabled = true;
		

		private RelayCommand<object> _relay;
		#endregion

		public PersonViewModel()
		{
			LoaderManager.Instance.Initialize(this);
		}

		public string First
		{
			get { return _first; }
			set { _first = value; OnPropertyChanged(); }
		}
		public string Last
		{
			get { return _last; }
			set { _last = value; OnPropertyChanged(); }
		}
		public string Address
		{
			private get { return _address; }
			set { _address = value; OnPropertyChanged(); }
		}

		public string Date
		{
			private get { return _date; }
			set { _date = value; OnPropertyChanged(); }
		}

		private bool CanExecuteCommand()
		{
			return !(string.IsNullOrWhiteSpace(_first)||string.IsNullOrWhiteSpace(_last)||string.IsNullOrWhiteSpace(_date)||string.IsNullOrWhiteSpace(_address));
		}

		public RelayCommand<object> Calculate
		{
			get
			{
				return _relay ?? (_relay = new RelayCommand<object>(
					       PersonCalculationImplementation, o => CanExecuteCommand()));
			}
		}

		private async void PersonCalculationImplementation(object obj)
		{
			LoaderManager.Instance.ShowLoader();

			await Task.Run(() =>
			{
				DateTime birthday = Convert.ToDateTime(_date);
				
				Thread.Sleep(1000);
				try
				{
					Person person = new Person(_first, _last, birthday, _address);
				

				string bday = "";
				string today = "not ";
				if (person.IsBirthday)
				{
					bday = "Happy Birthday!";
					today = "";
				}

				string adult = "";
				if (!person.IsAdult)
					adult = "not ";
				LoaderManager.Instance.HideLoader();
				MessageBox.Show(bday+
				                $"\nYour first name is {person.FirstName} " +
				                $"\n Your last name is {person.LastName}" + 
				                $"\n You were born on {person.BirthDate.Value.ToShortDateString()}" +
								$"\n Your email address is {person.Email}" +
				                $"\n Your sun sign is {person.SunSign} " +
				                $"\n Your chinese sign is {person.ChineseSign}"+
				                $"\n You are {adult}18 or above"+
				                $"\n Today is {today}your birthday"
				               );
				}
				catch (FutureBirthdayException exception)
				{
					LoaderManager.Instance.HideLoader();
					MessageBox.Show(exception.Body, exception.Title, MessageBoxButton.OK);
				}
				catch (TooFarBirthdayException exception)
				{
					LoaderManager.Instance.HideLoader();
					MessageBox.Show(exception.Body, exception.Title, MessageBoxButton.OK);
				}
				catch (InvalidEmailException exception)
				{
					LoaderManager.Instance.HideLoader();
					MessageBox.Show(exception.Body, exception.Title, MessageBoxButton.OK);
				}

			});

			
			}
		
		public Visibility LoaderVisibility
		{
			get { return _loaderVisibility; }
			set {	_loaderVisibility = value;	OnPropertyChanged();}
		}

		public bool IsControlEnabled
		{
			get { return _isControlEnabled; }
			set	{_isControlEnabled = value;OnPropertyChanged();}
		}
		public event PropertyChangedEventHandler PropertyChanged;

		protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}
