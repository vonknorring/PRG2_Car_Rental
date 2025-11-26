using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PRG_MAUI_Car_Register.Model
{
    public class MC : Vehicle
    {
        public int EngineCapacity { get; set; } = 600; 

        public override string GetDescription()
        {
            return $"MC: {RegistrationNumber}, {Manufacturer} {Model}, {Year}, Motor: {EngineCapacity}cc";
        }
    }
}
