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
        public string displayName => isCastingName ? castName : name;
        public Vector2 castPosition;
        public List<(int layer, string expression)> CastExpressions { get; set; }
        private const string CastNameID = " as ";
        private const string CastPositionID = " at ";
        private const string CastExpressionID = " [";
        private const char AxisDelimiter = ':';
        private const char LayerJoin = ',';
        private const char LayerDelimiter = ':';

        // Character will appear on screen if name preceeded by EnterKeyword
        private const string EnterKeyword = "enter ";
        public bool shouldCharacterEnter = false;

        // Check for casting data
        public bool isCastingName => castName != string.Empty;
        public bool isCastingPosition = false;
        public bool isCastingExpressions => CastExpressions.Count > 0;

        public DL_Speaker(string rawSpeaker)
        {
            rawSpeaker = ProcessNameKeywords(rawSpeaker);

            string pattern = @$"{CastNameID}|{CastPositionID}|{CastExpressionID.Insert(CastExpressionID.Length - 1, @"\")}";
            MatchCollection matches = Regex.Matches(rawSpeaker, pattern);

            // Populate with empty values to avoid NullPointExceptions
            castName = "";
            castPosition = Vector2.zero;
            isCastingPosition = false;
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
                    isCastingPosition = true;
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
                            // If we have layer and expressiom
                            if (parts.Length == 2)
                            {
                                return (int.Parse(parts[0]), parts[1]);
                            } // If we only have expression, we default to 0
                            else
                            {
                                return (0, parts[0]);
                            }
                        }).ToList();
                }
            }
        }

        private string ProcessNameKeywords(string rawSpeaker)
        {
            if (rawSpeaker.StartsWith(EnterKeyword))
            {
                rawSpeaker = rawSpeaker.Substring(EnterKeyword.Length);
                shouldCharacterEnter = true;
            }
            return rawSpeaker;
        }
    }
}
