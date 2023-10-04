namespace GS.LengthConverter.Net6;

public class LengthConverter : ILengthConverter
{
    static LengthConverter()
    {
        _flattenUnits = _systems
            .SelectMany(x => x.Value.Select(y => (y.Key, x.Key)))
            .ToDictionary(x => x.Item1, y => y.Item2);
    }

    private static readonly Dictionary<LengthUnit, SystemOfUnits> _flattenUnits;

    private static readonly Dictionary<SystemOfUnits, Dictionary<LengthUnit, UnitOfLengthInfo>> _systems = new()
    {
        {
            SystemOfUnits.SI, new()
            {
                { LengthUnit.Millimeters, new() { Order = 0, Multiplier = 1m } },
                { LengthUnit.Meter, new() { Order = 1, Multiplier = 1000m } },
                { LengthUnit.Kilometer, new() { Order = 2, Multiplier = 1000m } },
            }
        },
        {
            SystemOfUnits.Imperial, new()
            {
                { LengthUnit.Inches, new() { Order = 0, Multiplier = 1m } },
                { LengthUnit.InternationalFeet, new() { Order = 1, Multiplier = 12m } },
                { LengthUnit.Yard, new() { Order = 2, Multiplier = 3m } },
                { LengthUnit.Mile, new() { Order = 3, Multiplier = 1760m } },
            }
        },
        {
            SystemOfUnits.UnitedStatesCustomaryUnits, new()
            {
                { LengthUnit.UsSurveyLink, new() { Order = -1, Multiplier = 1m } },
                { LengthUnit.UsSurveyFeet, new() { Order = 0, Multiplier = 1m } },
            }
        },
    };

    private static readonly Dictionary<LengthUnit, Dictionary<LengthUnit, decimal>> _interSystemUnits = new()
    {
        {
            LengthUnit.Millimeters, new()
            {
                { LengthUnit.Inches, 1 / 25.4m },
                { LengthUnit.UsSurveyFeet, 1m / (1000 * 1200m / 3937m) },
            }
        },
        {
            LengthUnit.Inches, new()
            {
                { LengthUnit.Millimeters, 25.4m },
                { LengthUnit.UsSurveyFeet, 25.4m / (1000 * 1200m / 3937m) },
            }
        },
        {
            LengthUnit.UsSurveyFeet, new()
            {
                { LengthUnit.Inches, 1000 * 1200m / 3937m / 25.4m },
                { LengthUnit.Millimeters, 1000 * 1200m / 3937m },
            }
        },
    };

    public decimal ConvertWithinSystem(decimal value,
        LengthUnit sourceMathLengthUnit,
        LengthUnit targetMathLengthUnit)
    {
        var sourceSystemOfUnits = _flattenUnits[sourceMathLengthUnit];
        var targetSystemOfUnits = _flattenUnits[targetMathLengthUnit];
        if (sourceSystemOfUnits != targetSystemOfUnits)
        {
            throw new ArgumentException("Source and target Length Unit Systems are different");
        }
        var sourceLengthUnit = _systems[sourceSystemOfUnits][sourceMathLengthUnit];
        var targetLengthUnit = _systems[targetSystemOfUnits][targetMathLengthUnit];
        if (sourceLengthUnit.Order == targetLengthUnit.Order)
        {
            return value;
        }

        var directionIsGrowing = sourceLengthUnit.Order < targetLengthUnit.Order;
        var filterExpression = directionIsGrowing
            ? new Func<KeyValuePair<LengthUnit, UnitOfLengthInfo>, bool>(x =>
                x.Value.Order > sourceLengthUnit.Order && x.Value.Order <= targetLengthUnit.Order)
            : new Func<KeyValuePair<LengthUnit, UnitOfLengthInfo>, bool>(x =>
                x.Value.Order > targetLengthUnit.Order && x.Value.Order <= sourceLengthUnit.Order);
        var units = _systems[sourceSystemOfUnits];
        var multipliers = units
            .Where(filterExpression)
            .Select(x => x.Value.Multiplier)
            .ToList();

        var resultMultipler = 1m;
        foreach (var multiplier in multipliers)
        {
            resultMultipler *= multiplier;
        }
        return directionIsGrowing ? value / resultMultipler : value * resultMultipler;
    }

    public decimal Convert(decimal value, LengthUnit sourceMathLengthUnit, LengthUnit targetMathLengthUnit)
    {
        var sourceSystemOfUnits = _flattenUnits[sourceMathLengthUnit];
        var targetSystemOfUnits = _flattenUnits[targetMathLengthUnit];

        if (sourceSystemOfUnits == targetSystemOfUnits)
        {
            return ConvertWithinSystem(value, sourceMathLengthUnit, targetMathLengthUnit);
        }
        var sourceUnits = _systems[sourceSystemOfUnits];
        var minimalLengthUnitOfSourceSystem = sourceUnits.Single(x => x.Value.Order == 0).Key;
        var valueInMinimalSourceUnit = ConvertWithinSystem(value, sourceMathLengthUnit, minimalLengthUnitOfSourceSystem);

        var targetUnits = _systems[targetSystemOfUnits];
        var minimalLengthUnitOfTargetSystem = targetUnits.Single(x => x.Value.Order == 0).Key;
        var targetSystemMultiplier = _interSystemUnits[minimalLengthUnitOfSourceSystem][minimalLengthUnitOfTargetSystem];
        var valueInMinimalTargetUnit = valueInMinimalSourceUnit * targetSystemMultiplier;

        return ConvertWithinSystem(valueInMinimalTargetUnit, minimalLengthUnitOfTargetSystem, targetMathLengthUnit);
    }
}
