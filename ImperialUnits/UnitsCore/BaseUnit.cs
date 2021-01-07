using System;
using System.Collections.Generic;
using System.Text;

namespace ImperialUnits
{
    /// <summary>
    /// Basenheten för varje kategori av enheter, endast enheter med samma basenhet kan konverteras för tillfället.
    /// tanken är att om man i framtiden ska kunna tex kombinera olika enheter ( v = m/s )
    /// respektive enheters system för konversion osv. borde kunna kvarstå.
    /// </summary>
    public class BaseUnit
    {
        public BaseUnit( string unitCategory, double factor, params string[] synonyms )
        {
            UnitCategory = unitCategory;
            ToBaseUnitFactor = factor;
            if( synonyms is null )
            {
                throw new ArgumentNullException( "New Units must have at least one name" );
            }
            else
            {
                Synonyms.AddRange( synonyms );
            }
        }
        /// <summary>
        /// Kombination av system (Metric, imperial osv) och dimension (Längd, vikt osv)
        /// hann inte längre, kanske borde utökas till enskilda klasser (System och Dimension) med metoder för att tilllåta översättningar mellan kategorier. 
        /// </summary>
        public string UnitCategory { get; }
        public double ToBaseUnitFactor { get; }
        public List<string> Synonyms { get; } = new List<string>();
    }
}
