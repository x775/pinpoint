using System;
using System.Collections.Generic;
using Pinpoint.Core;
using Converter = System.Func<double, double>;

namespace Pinpoint.Plugin.MeasurementConverter.Converters
{
    public class VolumeConverter : IConverter
    {
        /* In several instances, there are multiple values for the same unit. 
         * Thus, in order to ensure consistency, in any instance of possible 
         * ambiguity, the US standard has been chosen. In case of dry/fluid,
         * the most common has been chosen (e.g. gallon fluid above gallon dry).
         * 
         * Source: https://en.wikipedia.org/wiki/Conversion_of_units#Volume
         */
        public enum VolumeUnit
        {
            bl = 1, 
            cm3 = 2, 
            ft3 = 3,
            in3 = 4, 
            m3 = 5, 
            mi3 = 6,
            yd3 = 7, 
            cup = 8,
            cups = 8,
            oz = 9, 
            floz = 9,
            ounce = 10,
            ounces = 10,
            gal = 11, 
            gallon = 11, 
            gallons = 11,
            l = 12, 
            liter = 12,
            ml = 13,
            milliliter = 13, 
            milliliters = 13,
            pk = 14, 
            peck = 14,
            pecks = 14,
            pt = 15, 
            pint = 15, 
            pints = 15,
            qt = 16,
            quart = 16,
            quarts = 16,
            tbsp = 17,
            tablespoon = 17,
            tablespoons = 17,
            tsp = 18,
            teaspoon = 18,
            teaspoons = 18
        }

        private static readonly Dictionary<VolumeUnit, Converter> toCubicMeter = new Dictionary<VolumeUnit, Converter>()
        {
            [VolumeUnit.bl] = amount => amount * 0.119240471,
            [VolumeUnit.cm3] = amount => amount * 0.000001,
            [VolumeUnit.ft3] = amount => amount * 0.028316847,
            [VolumeUnit.in3] = amount => amount * 1.63871E-05,
            [VolumeUnit.m3] = amount => amount,
            [VolumeUnit.mi3] = amount => amount * 4168181825.44058,
            [VolumeUnit.yd3] = amount => amount * 0.764554858,
            [VolumeUnit.cup] = amount => amount * 0.000236588,
            [VolumeUnit.ounce] = amount => amount * 2.95735E-05,
            [VolumeUnit.gal] = amount => amount * 0.003785412,
            [VolumeUnit.l] = amount => amount * 0.001,
            [VolumeUnit.ml] = amount => amount * 0.000001,
            [VolumeUnit.pk] = amount => amount * 0.008809768,
            [VolumeUnit.pt] = amount => amount * 0.000473176,
            [VolumeUnit.qt] = amount => amount * 0.000946353,
            [VolumeUnit.tbsp] = amount => amount * 1.47868E-05,
            [VolumeUnit.tsp] = amount => amount * 4.92892E-06
        };

        private static readonly Dictionary<VolumeUnit, Converter> fromCubicMeter = new Dictionary<VolumeUnit, Converter>()
        {
            [VolumeUnit.bl] = amount => amount / 0.119240471,
            [VolumeUnit.cm3] = amount => amount / 0.000001,
            [VolumeUnit.ft3] = amount => amount / 0.028316847,
            [VolumeUnit.in3] = amount => amount / 1.63871E-05,
            [VolumeUnit.m3] = amount => amount,
            [VolumeUnit.mi3] = amount => amount / 4168181825.44058,
            [VolumeUnit.yd3] = amount => amount / 0.764554858,
            [VolumeUnit.cup] = amount => amount / 0.000236588,
            [VolumeUnit.ounce] = amount => amount / 2.95735E-05,
            [VolumeUnit.gal] = amount => amount / 0.003785412,
            [VolumeUnit.l] = amount => amount / 0.001,
            [VolumeUnit.ml] = amount => amount / 0.000001,
            [VolumeUnit.pk] = amount => amount / 0.008809768,
            [VolumeUnit.pt] = amount => amount / 0.000473176,
            [VolumeUnit.qt] = amount => amount / 0.000946353,
            [VolumeUnit.tbsp] = amount => amount / 1.47868E-05,
            [VolumeUnit.tsp] = amount => amount / 4.92892E-06
        };

        public double Convert(Enum fromUnit, Enum toUnit, double amount)
        {
            return fromCubicMeter[(VolumeUnit)toUnit](toCubicMeter[(VolumeUnit)fromUnit](amount));
        }
    }
}
