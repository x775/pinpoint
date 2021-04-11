using System;
using System.Collections.Generic;
using Pinpoint.Core;
using Converter = System.Func<double, double>;

namespace Pinpoint.Plugin.MetaConverter.Converters
{
    public class MetricConverter : IConverter
    {
        public enum MetricUnit
        {
            nm,
            micrometer,
            mm,
            cm,
            m,
            km,
            @in,
            ft,
            yd,
            mi
        }

        private static readonly Dictionary<MetricUnit, Converter> toCentimeters = new Dictionary<MetricUnit, Converter>()
        {
            [MetricUnit.nm] = amount => amount / 100000000,
            [MetricUnit.micrometer] = amount => amount / 10000,
            [MetricUnit.mm] = amount => amount / 10,
            [MetricUnit.cm] = amount => amount,
            [MetricUnit.m] = amount => amount * 100,
            [MetricUnit.km] = amount => amount * 100000,
            [MetricUnit.@in] = amount => amount * 2.54,
            [MetricUnit.ft] = amount => amount * 30.48,
            [MetricUnit.yd] = amount => amount * 91.44,
            [MetricUnit.mi] = amount => amount * 160934.4
        };

        private static readonly Dictionary<MetricUnit, Converter> fromCentimeters = new Dictionary<MetricUnit, Converter>()
        {
            [MetricUnit.nm] = amount => amount * 100000000,
            [MetricUnit.micrometer] = amount => amount * 10000,
            [MetricUnit.mm] = amount => amount * 10,
            [MetricUnit.cm] = amount => amount,
            [MetricUnit.m] = amount => amount / 100,
            [MetricUnit.km] = amount => amount / 100000,
            [MetricUnit.@in] = amount => amount / 2.54,
            [MetricUnit.ft] = amount => amount / 30.48,
            [MetricUnit.yd] = amount => amount / 91.44,
            [MetricUnit.mi] = amount => amount / 160934.4
        };

        public double Convert(Enum fromUnit, Enum toUnit, double amount)
        {
            return fromCentimeters[(MetricUnit)toUnit](toCentimeters[(MetricUnit)fromUnit](amount));
        }
    }
}
