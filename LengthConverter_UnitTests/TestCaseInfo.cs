using GS.LengthConverter.Net6;

namespace LengthConverter_UnitTests;

public class TestCaseInfo
{
    public TestCaseInfo(LengthUnit sourceSystemOfUnits,
        LengthUnit targetSystemOfUnits,
        decimal soureValue,
        decimal expectedValue)
    {
        SourceSystemOfUnits = sourceSystemOfUnits;
        TargetSystemOfUnits = targetSystemOfUnits;
        SoureValue = soureValue;
        ExpectedValue = expectedValue;
    }

    public LengthUnit SourceSystemOfUnits { get; set; }

    public LengthUnit TargetSystemOfUnits { get; set; }

    public decimal SoureValue { get; set; }

    public decimal ExpectedValue { get; set; }
}
