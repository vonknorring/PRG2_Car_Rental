using PRG_MAUI_Car_Register.Model;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace PRG_MAUI_Car_Register.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Vehicle> Vehicles { get; } = new();

        public List<string> VehicleTypes { get; } =
            new() { "Bil", "MC", "Lastbil" };

        private string selectedVehicleType;
        public string SelectedVehicleType
        {
            get => selectedVehicleType;
            set { selectedVehicleType = value; OnPropertyChanged(nameof(SelectedVehicleType)); }
        }

        private string registrationNumber;
        public string RegistrationNumber
        {
            get => registrationNumber;
            set { registrationNumber = value; OnPropertyChanged(nameof(RegistrationNumber)); }
        }

        private string manufacturer;
        public string Manufacturer
        {
            get => manufacturer;
            set { manufacturer = value; OnPropertyChanged(nameof(Manufacturer)); }
        }

        private string model;
        public string Model
        {
            get => model;
            set { model = value; OnPropertyChanged(nameof(Model)); }
        }

        private string year;
        public string Year
        {
            get => year;
            set { year = value; OnPropertyChanged(nameof(Year)); }
        }

        private string searchResult;
        public string SearchResult
        {
            get => searchResult;
            set { searchResult = value; OnPropertyChanged(nameof(SearchResult)); }
        }

        public ICommand RegisterCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand FilterCommand { get; }

        public MainPageViewModel()
        {
            SelectedVehicleType = VehicleTypes[0];

            RegisterCommand = new Command(AddVehicle);
            SearchCommand = new Command(SearchVehicle);
            FilterCommand = new Command<string>(FilterVehicles);
        }

        private void AddVehicle()
        {
            try 
            { 
                Vehicle? vehicle = SelectedVehicleType switch
                {
                    "Bil" => new Car(),
                    "MC" => new MC(),
                    "Lastbil" => new Truck(),
                    _ => null
                };

                vehicle.RegistrationNumber = RegistrationNumber;
                vehicle.Manufacturer = Manufacturer;
                vehicle.Model = Model;
                vehicle.Year = Year;

                Vehicles.Add(vehicle);

                RegistrationNumber = "";
                Manufacturer = "";
                Model = "";
                Year = "";
            }
            catch(ArgumentException ex)
            {
            Application.Current.MainPage.DisplayAlert("Fel", ex.Message, "OK");            
            }
        }

        private void SearchVehicle()
        {
            var found = Vehicles.FirstOrDefault(v =>
                v.RegistrationNumber?.ToLower() == RegistrationNumber?.ToLower());

            SearchResult = found != null
                ? $"Fordon hittat:\n{found.RegistrationNumber} {found.Manufacturer} {found.Model}"
                : "Inget fordon hittades.";
        }

        private void FilterVehicles(string type)
        {
            IEnumerable<Vehicle> filtered = type switch
            {
                "Bil" => Vehicles.Where(v => v is Car),
                "MC" => Vehicles.Where(v => v is MC),
                "Lastbil" => Vehicles.Where(v => v is Truck),
                _ => Vehicles
            };

            Vehicles.Clear();
            foreach (var v in filtered)
                Vehicles.Add(v);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyName)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
