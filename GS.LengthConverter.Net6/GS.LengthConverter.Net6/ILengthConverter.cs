namespace GS.LengthConverter.Net6;

public interface ILengthConverter
{
    decimal ConvertWithinSystem(decimal value, LengthUnit sourceMathLengthUnit, LengthUnit targetMathLengthUnit);

    decimal Convert(decimal value, LengthUnit sourceMathLengthUnit, LengthUnit targetMathLengthUnit);
}