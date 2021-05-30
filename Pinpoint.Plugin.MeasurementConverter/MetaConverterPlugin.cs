using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pinpoint.Core;
using Pinpoint.Core.Results;

namespace Pinpoint.Plugin.MetaConverter
{
    public class MetaConverterPlugin : IPlugin
    {
        // Match left- and right-hand side of any conversion.
        private static readonly Regex Pattern = new Regex(@"^(\d+) ?(\w*){1}( (to|in) )?(\w*)?");
        private static Match _match;
        

        public PluginMeta Meta { get; set; } = new PluginMeta("Meta Converter", PluginPriority.Highest);
        public PluginSettings UserSettings { get; set; } = new PluginSettings();
        public bool TryLoad() => true;

        public void Unload()
        {
        }

        private static Type[] typeCandidates = default;

        private static double? MatchAndConvert(string fromString, string toString, double value)
        {
            foreach (var type in typeCandidates)
            {
                var converter = (IConverter)Activator.CreateInstance(type)!;
                if (Enum.TryParse(converter.Unit, fromString.ToLower(), out var fromEnum))
                {
                    if (Enum.TryParse(converter.Unit, toString.ToLower(), out var toEnum))
                    {
                        ((IConverter)Activator.CreateInstance(type)!)!.Convert((Enum)fromEnum!, (Enum)toEnum!, value);
                    }
                }
            }

            return null;
        }

        private static Tuple<string, double> ConvertQuery(Query query)
        {
            var types = typeof(MetaConverterPlugin).Assembly.GetTypes();
            typeCandidates = types.Where(t => t.IsClass && t.GetInterface(nameof(IConverter)) != null).ToArray();

            // match.Groups[0].Value holds the entire matched expression.
            var value = double.Parse(_match.Groups[1].Value, CultureInfo.InvariantCulture.NumberFormat);
            var fromUnit = _match.Groups[2].Value;
            var toUnit = _match.Groups[5].Value;

            var result = MatchAndConvert(fromUnit, toUnit, value);

            if (result != null)
            {
                return new Tuple<string, double>(toUnit, Math.Round((double)result, 5));
            }

            return null;
        }

        public async Task<bool> Activate(Query query)
        {
            _match = Pattern.Match(query.RawQuery);
            return _match != default && _match.Success;
        }

        public async IAsyncEnumerable<AbstractQueryResult> Process(Query query)
        {
            var tuple = ConvertQuery(query);
            yield return new ConversionResult(tuple.Item1, tuple.Item2);
        }
    }
}