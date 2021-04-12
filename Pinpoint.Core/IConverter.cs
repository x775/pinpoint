using System;
using System.Collections.Generic;
using System.Text;

namespace Pinpoint.Core
{
    public interface IConverter
    {
        double Convert(Enum fromUnit, Enum toUnit, double amount);
        Type Unit { get; }
    }
}
