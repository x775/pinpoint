using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Pinpoint.Core;
using Pinpoint.Core.Results;
using Pinpoint.Plugin.MetaConverter.Converters

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

        private static Tuple<string, double> ConvertQuery(Query query)
        {
            // match.Groups[0].Value holds the entire matched expression.
            var value = _match.Groups[1].Value;
            var fromUnit = _match.Groups[2].Value;
            var toUnit = _match.Groups[5].Value;

            // Handle non specified toUnit.
            if (_match.Groups[3].Value == "" || toUnit == "")
            {
                toUnit = "m";
            }

            var conversion = UnitConverter.ConvertTo(fromUnit, toUnit, Convert.ToDouble(value));
            return new Tuple<string, double>(toUnit, Math.Round(conversion, 5));
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

        public async Task<bool> Activate(Query query)
        {
            _match = Pattern.Match(query.RawQuery);
            return _match != default && _match.Success;
        }

        public async IAsyncEnumerable<AbstractQueryResult> IPlugin.Process(Query query)
        {
            var tuple = ConvertQuery(query);
            yield return new ConversionResult(tuple.Item1, tuple.Item2);
        }
    }
}