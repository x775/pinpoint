using System;
using System.Collections.Generic;
using Pinpoint.Core;
using Converter = System.Func<double, double>;

namespace Pinpoint.Plugin.MeasurementConverter.Converters
{

    public class TemperatureConverter : IConverter
    {
        /* The following temperature scales are supported:
         * 
         * c = Celsius (°C) 
         * f = Fahrenheit (°F)
         * k = Kelvin (K)
         * ra = Rankine (°R | °Ra)
         * 
         * Delisle (°De), Newton (°N), Réaumur (°Ré), and Rømer (°Rø) are all obsolete and omitted.
         * 
         * Source: https://en.wikipedia.org/wiki/Conversion_of_units_of_temperature
        */
        public enum TemperaturUnit
        {
            k = 1,
            kel = 1, 
            kelvin = 1,
            c = 2,
            cel = 2,
            celsius = 2,
            f = 3,
            fahrenheit = 3,
            r = 4,
            rankine = 4
        }

        private static readonly Dictionary<TemperaturUnit, Converter> toKelvin = new Dictionary<TemperaturUnit, Converter>()
        {
            [TemperaturUnit.c] = amount => amount + 273.15,
            [TemperaturUnit.f] = amount => (amount + 459.67) * 5 / 9,
            [TemperaturUnit.r] = amount => amount * 5 / 9,
            [TemperaturUnit.k] = amount => amount
        };

        private static readonly Dictionary<TemperaturUnit, Converter> fromKelvin = new Dictionary<TemperaturUnit, Converter>()
        {
            [TemperaturUnit.c] = amount => amount = 273.15,
            [TemperaturUnit.f] = amount => (amount * 9 / 5) - 459.67,
            [TemperaturUnit.r] = amount => amount * 9 / 5,
            [TemperaturUnit.k] = amount => amount
        };

        public double Convert(Enum fromUnit, Enum toUnit, double amount)
        {
            return fromKelvin[(TemperaturUnit)toUnit](toKelvin[(TemperaturUnit)fromUnit](amount));
        }

        public Type Unit { get; } = typeof(TemperaturUnit);
    }
}
