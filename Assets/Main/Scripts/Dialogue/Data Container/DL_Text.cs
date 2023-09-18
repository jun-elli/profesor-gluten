using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine;

namespace Dialogue
{

    public class DL_Text
    {
        public List<Segment> segments;
        private const string SegmentPattern = @"\{[ca]\}|\{w[ca]\s\d*\.?\d*\}";

        // Constructor
        public DL_Text(string rawDialogue)
        {
            segments = RipSegments(rawDialogue);
        }

        private List<Segment> RipSegments(string rawDialogue)
        {
            MatchCollection matches = Regex.Matches(rawDialogue, SegmentPattern);
            List<Segment> result = new List<Segment>();

            int lastIndex = 0;

            // Find first segment or only segment
            Segment segment = new Segment();

            segment.text = matches.Count == 0 ? rawDialogue : rawDialogue.Substring(0, matches[0].Index);
            segment.signal = Segment.StartSignal.NONE;
            segment.signalDelay = 0;
            result.Add(segment);

            if (matches.Count == 0)
            {
                return result;
            }
            else
            {
                lastIndex = matches[0].Index;
            }

            // Find next segments' signal and text
            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];
                segment = new Segment();

                // Get signal //
                string signalMatch = match.Value.Substring(1, match.Length - 2);
                // we only want the letters cast as Enum
                string[] signalAndDelay = signalMatch.Split(" ");
                segment.signal = (Segment.StartSignal)Enum.Parse(typeof(Segment.StartSignal), signalAndDelay[0].ToUpper());

                // Get delay //
                if (signalAndDelay.Length > 1)
                {
                    float.TryParse(signalAndDelay[1], NumberStyles.Any, new CultureInfo("en-US"), out segment.signalDelay);
                }

                // Get text segment //
                // Get the next index to know where the segment ends
                int nextIndex = i + 1 > matches.Count ? matches[i + 1].Index : rawDialogue.Length;

                int textStartIndex = lastIndex + match.Length;
                int textEndIndex = nextIndex - (lastIndex + match.Length); // I don't understand why we subtract ()
                segment.text = rawDialogue.Substring(textStartIndex, textEndIndex);

                lastIndex = nextIndex;
                result.Add(segment);
            }
            return result;
        }

        // We divide our dialogue text in segments
        public struct Segment
        {
            public string text;
            public StartSignal signal;
            public float signalDelay;

            public enum StartSignal { NONE, C, A, WC, WA }

            public bool shouldAppend => (signal == StartSignal.A || signal == StartSignal.WA);

        }
    }
}
