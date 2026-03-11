using PRG_MAUI_Car_Register.Model;
using PRG_MAUI_Car_Register.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;
   
namespace PRG_MAUI_Car_Register.ViewModel
{
    public class MainPageViewModel : INotifyPropertyChanged
    {
        private readonly IStorageService _storage;

        public ObservableCollection<Vehicle> Vehicles { get; } = new();

        public List<string> VehicleTypes { get; } =
            new() { "Bil", "MC", "Lastbil" };

        private string selectedVehicleType;
        public string SelectedVehicleType
        {
            get => selectedVehicleType;
            set { selectedVehicleType = value; OnPropertyChanged(nameof(SelectedVehicleType)); }
        }

        private string searchRegistrationNumber;
        public string SearchRegistrationNumber
        {
            get => searchRegistrationNumber;
            set { searchRegistrationNumber = value; OnPropertyChanged(nameof(SearchRegistrationNumber)); }
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

        public MainPageViewModel(IStorageService storage)
        {
            _storage = storage;

            SelectedVehicleType = VehicleTypes[0];

            RegisterCommand = new Command(AddRegisterCommand);
            SearchCommand = new Command(SearchVehicle);
            FilterCommand = new Command<string>(FilterVehicles);

            _ = LoadAsync();
        }

        private async Task LoadAsync()
        {
            var vehicles = await _storage.LoadAsync();

            Vehicles.Clear();
            foreach (var v in vehicles)
                Vehicles.Add(v);
        }

        private async void AddRegisterCommand()
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

                if (vehicle == null)
                    return;

                vehicle.RegistrationNumber = RegistrationNumber;
                vehicle.Manufacturer = Manufacturer;
                vehicle.Model = Model;
                vehicle.Year = Year;

                Vehicles.Add(vehicle);

                await SaveAsync();

                RegistrationNumber = "";
                Manufacturer = "";
                Model = "";
                Year = "";
            }
            catch (ArgumentException ex)
            {
                await Application.Current.MainPage.DisplayAlert("Fel", ex.Message, "OK");
            }
        }

        private async Task SaveAsync()
        {
            await _storage.SaveAsync(Vehicles.ToList());
        }

        private void SearchVehicle()
        {
            var found = Vehicles.FirstOrDefault(v =>
                v.RegistrationNumber?.ToLower() == SearchRegistrationNumber?.ToLower());

            SearchResult = found != null
                ? $"Fordon hittat:\n{found.RegistrationNumber} {found.Manufacturer} {found.Model}"
                : "Inget fordon hittades.";
        }

        private void FilterVehicles(string type)
        {
            var allVehicles = Vehicles.ToList();

            IEnumerable<Vehicle> filtered = type switch
            {
                "Bil" => allVehicles.Where(v => v is Car),
                "MC" => allVehicles.Where(v => v is MC),
                "Lastbil" => allVehicles.Where(v => v is Truck),
                _ => allVehicles
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