using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Diagnostics;
using PRG_MAUI_Car_Register.Model;

namespace PRG_MAUI_Car_Register.Services
{
    

    public class JsonStorageService : IStorageService
    {
        private readonly string _filePath;

        public JsonStorageService()
        {
            _filePath = Path.Combine(FileSystem.AppDataDirectory, $"{typeof(Vehicle).Name.ToLower()}s.json");
        }

        public async Task<IList<Vehicle>> LoadAsync()
        {
            if (!File.Exists(_filePath))
            {
                await File.WriteAllTextAsync(_filePath, "[]");
                return new List<Vehicle>();
            }

           

            var json = await File.ReadAllTextAsync(_filePath);
            return JsonSerializer.Deserialize<List<Vehicle>>(json) ?? new List<Vehicle>();
        }

        public async Task SaveAsync(IEnumerable<Vehicle> items)
        {
            var json = JsonSerializer.Serialize(items, new JsonSerializerOptions { WriteIndented = true });
            await File.WriteAllTextAsync(_filePath, json);
        }
    }

}
