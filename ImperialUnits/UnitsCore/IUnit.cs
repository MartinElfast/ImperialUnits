using System.Collections.Generic;

namespace ImperialUnits
{
    /// <summary>
    /// Se summary på Unit class
    /// </summary>
    public interface IUnit
    {
        public BaseUnit BaseUnit { get; set; }
        public List<string> Synonyms { get; }
        public abstract double ToBaseUnitFactor { get; }
        public double ConvertFrom( double value, Unit unitFrom );
        public double ConvertToBaseUnit( double unitValue );
    }
}