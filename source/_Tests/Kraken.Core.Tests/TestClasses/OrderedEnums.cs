

using Kraken.Core;

namespace Kraken.Core.Tests
{
    public enum SortHigherEnum
    {
        ValueFirstLogicalSecond = 0,
        [EnumSortOrder(-1)]
        ValueSecondLogicalFirst = 1
    }

    public enum SortLowerEnum
    {
        [EnumSortOrder(10)]
        ValueFirstLogicalSecond = 0,
        ValueSecondLogicalFirst = 1,
    }

    public enum NoSortEnum
    {
        First = 0,
        Second = 1,
        Third = 2
    }

    public enum ValueUnorderedEnum
    {
        First = 0,
        Third = 3,
        Second = 2
    }
}
