using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Linq;
using UnityEngine;

namespace Dialogue
{
    public class DL_Speaker
    {
        // name -> name that we write in text assets when saying who is speaking
        // castName -> name that will apear in nametag box
        public string name, castName;
        public string displayName => castName != string.Empty ? castName : name;
        public Vector2 castPosition;
        public List<(int layer, string expression)> CastExpressions { get; set; }
        private const string CastNameID = " as ";
        private const string CastPositionID = " at ";
        private const string CastExpressionID = " [";
        private const char AxisDelimiter = ':';
        private const char LayerJoin = ',';
        private const char LayerDelimiter = ':';

        public DL_Speaker(string rawSpeaker)
        {
            string pattern = @$"{CastNameID}|{CastPositionID}|{CastExpressionID.Insert(CastExpressionID.Length - 1, @"\")}";
            MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

            // Populate with empty values to avoid NullPointExceptions
            castName = "";
            castPosition = Vector2.zero;
            CastExpressions = new List<(int layer, string expression)>();

            // Casting is optional, if there is none, name is speaker
            if (matches.Count < 1)
            {
                name = rawSpeaker;
                return;
            }
            // Find speaker name
            int index = matches[0].Index;
            name = rawSpeaker.Substring(0, index);

            // Find casting data
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                int startIndex = 0, endIndex = 0;

                // Find casting name
                if (match.Value == CastNameID)
                {
                    startIndex = match.Index + CastNameID.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;

                    castName = rawSpeaker.Substring(startIndex, endIndex - startIndex);
                }
                // Find casting position
                else if (match.Value == CastPositionID)
                {
                    startIndex = match.Index + CastPositionID.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    string pos = rawSpeaker.Substring(startIndex, endIndex - startIndex);

                    // Find x and y, if any
                    string[] axis = pos.Split(AxisDelimiter, System.StringSplitOptions.RemoveEmptyEntries);
                    float.TryParse(axis[0], NumberStyles.Float, new CultureInfo("en-US"), out castPosition.x);
                    if (axis.Length > 1)
                    {
                        float.TryParse(axis[1], NumberStyles.Float, new CultureInfo("en-US"), out castPosition.y);
                    }
                }
                // Find casting expressions
                else if (match.Value == CastExpressionID)
                {
                    startIndex = match.Index + CastExpressionID.Length;
                    endIndex = i < matches.Count - 1 ? matches[i + 1].Index : rawSpeaker.Length;
                    string expressions = rawSpeaker.Substring(startIndex, endIndex - (startIndex + 1));

                    CastExpressions = expressions.Split(LayerJoin)
                        .Select(x =>
                        {
                            var parts = x.Trim().Split(LayerDelimiter);
                            return (int.Parse(parts[0]), parts[1]);
                        }).ToList();
                }
            }
        }
    }
}
