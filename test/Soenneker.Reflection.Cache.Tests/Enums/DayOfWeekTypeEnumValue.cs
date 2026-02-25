using Soenneker.Gen.EnumValues;

namespace Soenneker.Reflection.Cache.Tests.Enums;

[EnumValue<string>]
public partial class DayOfWeekTypeEnumValue
{
    public static readonly DayOfWeekTypeEnumValue Sunday = new(nameof(Sunday));
    public static readonly DayOfWeekTypeEnumValue Monday = new(nameof(Monday));
    public static readonly DayOfWeekTypeEnumValue Tuesday = new(nameof(Tuesday));
    public static readonly DayOfWeekTypeEnumValue Wednesday = new(nameof(Wednesday));
    public static readonly DayOfWeekTypeEnumValue Thursday = new(nameof(Thursday));
    public static readonly DayOfWeekTypeEnumValue Friday = new(nameof(Friday));
    public static readonly DayOfWeekTypeEnumValue Saturday = new(nameof(Saturday));
}