using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_MAUI_Car_Register.Model
{
    public class Car : Vehicle
    {
        public int Doors { get; set; } = 4;

        public override string GetDescription()
        {
            return $"Bil: {RegistrationNumber}, {Manufacturer} {Model}, {Year}, Dörrar: {Doors}";
        }
    }
}
