using PRG_MAUI_Car_Register.Model;

namespace PRG_MAUI_Car_Register.Services
{
    public interface IStorageService
    {
        Task<IList<Vehicle>> LoadAsync();
        Task SaveAsync(IEnumerable<Vehicle> vehicles);
    }
}
