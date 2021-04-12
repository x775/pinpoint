using System;
using System.Collections.Generic;
using Pinpoint.Core;
using Converter = System.Func<double, double>;

namespace Pinpoint.Plugin.MeasurementConverter.Converters
{
    public class TimeConverter : IConverter
    {
        public enum TimeUnit
        {
            ns = 1,
            nanosecond = 1,
            nanoseconds = 1,
            µs = 2,
            microsecond = 2,
            microseconds = 2,
            s = 3,
            sec = 3,
            secs = 3,
            second = 3,
            seconds = 3,
            min = 4,
            mins = 4,
            minute = 4,
            minutes = 4,
            hr = 5, 
            hrs = 5,
            hour = 5,
            hours = 5,
            day = 6,
            days = 6,
            week = 7, 
            weeks = 7,
            month = 8,
            months = 8,
            yr = 9,
            yrs = 9,
            year = 9,
            years = 9
        }

        private static readonly Dictionary<TimeUnit, Converter> toSeconds = new Dictionary<TimeUnit, Converter>()
        {
            [TimeUnit.ns] = amount => amount * 1e-09,
            [TimeUnit.µs] = amount => amount * 1e-06,
            [TimeUnit.s] = amount => amount,
            [TimeUnit.min] = amount => amount * 60,
            [TimeUnit.hr] = amount => amount * 3600,
            [TimeUnit.day] = amount => amount * 86400,
            [TimeUnit.week] = amount => amount * 604800,
            [TimeUnit.month] = amount => amount * 604800,
            [TimeUnit.yr] = amount => amount * 31104000
        };

        private static readonly Dictionary<TimeUnit, Converter> fromSeconds = new Dictionary<TimeUnit, Converter>()
        {
            [TimeUnit.ns] = amount => amount / 1e-09,
            [TimeUnit.µs] = amount => amount / 1e-06,
            [TimeUnit.s] = amount => amount,
            [TimeUnit.min] = amount => amount / 60,
            [TimeUnit.hr] = amount => amount / 3600,
            [TimeUnit.day] = amount => amount / 86400,
            [TimeUnit.week] = amount => amount / 604800,
            [TimeUnit.month] = amount => amount / 604800,
            [TimeUnit.yr] = amount => amount / 31104000
        };

        public double Convert(Enum fromUnit, Enum toUnit, double amount)
        {
            return fromSeconds[(TimeUnit)toUnit](toSeconds[(TimeUnit)fromUnit](amount));
        }
        public Type Unit { get; } = typeof(TimeUnit);
    }
}
