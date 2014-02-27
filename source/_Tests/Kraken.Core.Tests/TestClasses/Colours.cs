using System.ComponentModel;
using Kraken.Core;

namespace Kraken.Core.Tests.TestClasses
{
    public enum Colour
    {
        Unknown = 0,

        [EnumCode("R")]
        [Description("Don't wave this at a bull")]
        Red = 1,

        [EnumCode("O")]
        [Description("Traffic lights are this colour")]
        Orange = 2,

        [EnumCode("P")]
        Pink = 3,

        Green = 4,
        Blue = 5,
        Yellow = 6,
        Purple = 7,
    }
}
