using System.Collections.Generic;
using System.Linq;

namespace EV3ModelLib
{

    public static class IdentificationHelper
    {
        static IdentificationHelper()
        {
            IdentificationCustomValues.ToList().ForEach(kvp => IdentificationValues[kvp.Key] = kvp.Value);
        }

        readonly static Dictionary<string, Dictionary<string, string>> IdentificationCustomValues = new Dictionary<string, Dictionary<string, string>>()
        {
            //-- first element is layer - normally "1.", daisy chain can change this
            ["OneInputPort"] = new Dictionary<string, string>() { ["1.1"] = "1", ["1.2"] = "2", ["1.3"] = "3", ["1.4"] = "4" },
            ["OneOutputPort"] = new Dictionary<string, string>() { ["1.A"] = "A", ["1.B"] = "B", ["1.C"] = "C", ["1.D"] = "D" },
            ["TwoOutputPorts"] = new Dictionary<string, string>() { ["1.A+B"] = "A+B", ["1.A+C"] = "A+C", ["1.A+D"] = "A+D", ["1.B+A"] = "B+A", ["1.B+C"] = "B+C", ["1.B+D"] = "B+D", ["1.C+A"] = "C+A", ["1.C+B"] = "C+B", ["1.C+D"] = "C+D", ["1.D+A"] = "D+A", ["1.D+B"] = "D+B", ["1.D+C"] = "D+C" },
        };

        public static readonly Dictionary<string, Dictionary<string, string>> IdentificationValues = new Dictionary<string, Dictionary<string, string>>
        {
            {
                "BrakeAtEnd",
                new Dictionary<string, string> { { "True", "Brake" }, { "False", "Coast" } }
            },
            {
                "Color",
                new Dictionary<string, string> { { "0", "NoColor" }, { "1", "Black" }, { "2", "Blue" }, { "3", "Green" }, { "4", "Yellow" }, { "5", "Red" }, { "6", "White" }, { "7", "Brown" } }
            },
            {
                "ComparisonType",
                new Dictionary<string, string> { { "0", "Equal" }, { "1", "NotEqual" }, { "2", "Greater" }, { "3", "GreaterEqual" }, { "4", "Less" }, { "5", "LessEqual" } }
            },
            {
                "ComparisonType2",
                new Dictionary<string, string> { { "0", "Equal" }, { "1", "NotEqual" } }
            },
            {
                "InvertColor",
                new Dictionary<string, string> { { "True", "White" }, { "False", "Black" } }
            },
            {
                "LEDColor",
                new Dictionary<string, string> { { "0", "Green" }, { "1", "Orange" }, { "2", "Red" } }
            },
            {
                "LEDPulse",
                new Dictionary<string, string> { { "0", "Solid" }, { "1", "Flash" }, { "2", "Pulse" } }
            },
            {
                "MeasuringMode",
                new Dictionary<string, string> { { "0", "Ping" }, { "1", "Continuous" } }
            },
            {
                "NXTBrickButtons",
                new Dictionary<string, string> { { "0", "Button0" }, { "1", "Button1" }, { "2", "Button2" }, { "3", "Button3" } }
            },
            {
                "OutputBrickButton",
                new Dictionary<string, string> { { "1", "Left" }, { "2", "Center" }, { "3", "Right" }, { "4", "Up" }, { "5", "Down" } }
            },
            {
                "OutputBrickButtonID",
                new Dictionary<string, string> { { "0", "None" }, { "1", "Left" }, { "2", "Center" }, { "3", "Right" }, { "4", "Up" }, { "5", "Down" } }
            },
            {
                "OutputButtonID",
                new Dictionary<string, string>
                {
                    {
                        "0",
                        "Zero"
                    },
                    {
                        "1",
                        "One"
                    },
                    {
                        "2",
                        "Two"
                    },
                    {
                        "3",
                        "Three"
                    },
                    {
                        "4",
                        "Four"
                    },
                    {
                        "5",
                        "Five"
                    },
                    {
                        "6",
                        "Six"
                    },
                    {
                        "7",
                        "Seven"
                    },
                    {
                        "8",
                        "Eight"
                    },
                    {
                        "9",
                        "Nine"
                    },
                    {
                        "10",
                        "Ten"
                    },
                    {
                        "11",
                        "Eleven"
                    }
                }
            },
            {
                "PlayType",
                new Dictionary<string, string> { { "0", "WaitForCompletion" }, { "1", "PlayOnce" }, { "2", "Repeat" } }
            },
            {
                "RateUnit",
                new Dictionary<string, string> { { "0", "PerSecond" }, { "1", "SecondsBetweenSamples" } }
            },
            {
                "SetOfChannels",
                new Dictionary<string, string> { { "1", "Channel1" }, { "2", "Channel2" }, { "3", "Channel3" }, { "4", "Channel4" } }
            },
            {
                "TouchState",
                new Dictionary<string, string> { { "0", "Released" }, { "1", "Pushed" }, { "2", "Bumped" } }
            },
            {
                "WaitForChange",
                new Dictionary<string, string> { { "0", "Up" }, { "1", "Down" }, { "2", "Both" } }
            }
        };

    }
}
