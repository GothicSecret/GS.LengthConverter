using GS.LengthConverter.Net6;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace LengthConverter_UnitTests;

[TestFixture]
public class LengthConverter_UnitTests
{
    private LengthConverter _converter;

    [SetUp]
    public void SetUp()
    {
        _converter = new();
    }

    [Test]
    public void ConvertSameSystem_ToUpperUnit_Should_ConvertCorrectly()
    {
        var sourceValue = 123.456m;

        var testCases = new List<TestCaseInfo>()
        {
            new TestCaseInfo(LengthUnit.Millimeters, LengthUnit.Meter, sourceValue, 0.123456m),
            new TestCaseInfo(LengthUnit.Millimeters, LengthUnit.Kilometer, sourceValue, 0.000123456m),
            new TestCaseInfo(LengthUnit.Meter, LengthUnit.Kilometer, sourceValue, 0.123456m),
            new TestCaseInfo(LengthUnit.Inches, LengthUnit.InternationalFeet, sourceValue, 10.288m),
            new TestCaseInfo(LengthUnit.Inches, LengthUnit.Yard, sourceValue, 3.4293333333333333333333333333m),
            new TestCaseInfo(LengthUnit.Inches, LengthUnit.Mile, sourceValue, 0.0019484848484848484848484848m),
            new TestCaseInfo(LengthUnit.InternationalFeet, LengthUnit.Yard, sourceValue, 41.152m),
            new TestCaseInfo(LengthUnit.InternationalFeet, LengthUnit.Mile, sourceValue, 0.0233818181818181818181818182m),
            new TestCaseInfo(LengthUnit.Yard, LengthUnit.Mile, sourceValue, 0.0701454545454545454545454545m),
        };

        foreach (var testCase in testCases)
        {
            Assert.AreEqual(testCase.ExpectedValue, _converter
                .ConvertWithinSystem(testCase.SoureValue, testCase.SourceSystemOfUnits, testCase.TargetSystemOfUnits),
            $"{testCase.SourceSystemOfUnits} to {testCase.TargetSystemOfUnits} conversion is incorrect");
        }
    }

    [Test]
    public void ConvertSameSystem_ToLowerUnit_Should_ConvertCorrectly()
    {
        var sourceValue = 123.456m;

        var testCases = new List<TestCaseInfo>()
        {
            new TestCaseInfo(LengthUnit.Meter, LengthUnit.Millimeters, sourceValue, 123456m),
            new TestCaseInfo(LengthUnit.Kilometer, LengthUnit.Millimeters, sourceValue, 123456000m),
            new TestCaseInfo(LengthUnit.Kilometer, LengthUnit.Meter, sourceValue, 123456),
            new TestCaseInfo(LengthUnit.InternationalFeet, LengthUnit.Inches, sourceValue, 1481.472m),
            new TestCaseInfo(LengthUnit.Yard, LengthUnit.Inches, sourceValue, 4444.416m),
            new TestCaseInfo(LengthUnit.Mile, LengthUnit.Inches, sourceValue, 7822172.16m),
            new TestCaseInfo(LengthUnit.Yard, LengthUnit.InternationalFeet, sourceValue, 370.368m),
            new TestCaseInfo(LengthUnit.Mile, LengthUnit.InternationalFeet, sourceValue, 651847.68m),
            new TestCaseInfo(LengthUnit.Mile, LengthUnit.Yard, sourceValue, 217282.56m),
        };

        foreach (var testCase in testCases)
        {
            Assert.AreEqual(testCase.ExpectedValue, _converter
                .ConvertWithinSystem(testCase.SoureValue, testCase.SourceSystemOfUnits, testCase.TargetSystemOfUnits),
            $"{testCase.SourceSystemOfUnits} to {testCase.TargetSystemOfUnits} conversion is incorrect");
        }
    }

    [Test]
    public void ConvertToOtherSystem_Should_ConvertCorrectly()
    {
        var sourceValue = 123.456m;

        var testCases = new List<TestCaseInfo>()
        {
            new TestCaseInfo(LengthUnit.Millimeters, LengthUnit.Inches, sourceValue, 4.8604724409448818897637795249m),
            new TestCaseInfo(LengthUnit.Millimeters, LengthUnit.UsSurveyFeet, sourceValue, 0.4050385599999999999999999959m),
            new TestCaseInfo(LengthUnit.Inches, LengthUnit.Millimeters, sourceValue, 3135.7824m),
            new TestCaseInfo(LengthUnit.Inches, LengthUnit.UsSurveyFeet, sourceValue, 10.287979424000000000000000004m),
            new TestCaseInfo(LengthUnit.UsSurveyFeet, LengthUnit.Inches, sourceValue, 1481.4749629499258998517997036m),
            new TestCaseInfo(LengthUnit.UsSurveyFeet, LengthUnit.Millimeters, sourceValue, 37629.464058928117856235712471m),
        };

        foreach (var testCase in testCases)
        {
            Assert.AreEqual(testCase.ExpectedValue, _converter
                .Convert(testCase.SoureValue, testCase.SourceSystemOfUnits, testCase.TargetSystemOfUnits),
            $"{testCase.SourceSystemOfUnits} to {testCase.TargetSystemOfUnits} conversion is incorrect");
        }
    }
}
