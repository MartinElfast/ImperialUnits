using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ImperialUnits
{
    public class Unit : IUnit
    {
        /// <summary>
        /// Definitionen av en enhet, känner till sin basenhet (SI enhet) och sin omvandlingsfaktor till denna.
        /// Kan konvertera sig själv till andra enheter inom samma system av mått och samma dimension.
        /// Har en lista av namn och förkortningar på sig själv
        /// </summary>
        /// <param name="baseUnit">Standard enhet som alla beräkningar utgår ifrån</param>
        /// <param name="factor">Conversionsfaktor till standard enhet</param>
        /// <param name="synonyms">Alla namn, synonymer och förkortningar på den här enheten</param>
        public Unit( BaseUnit baseUnit, double factor, params string[] synonyms )
        {
            if( baseUnit is null )
            {
                throw new ArgumentNullException( "New Units must have a BaseUnit" );
            }
            else
            {
                BaseUnit = baseUnit;
            }
            if( factor is 0d )
            {
                throw new ArgumentNullException( "New Units must have a ConversionFactor (to BaseUnit) above 0 (zero)" );
            }
            else
            {
                ToBaseUnitFactor = factor;
            }
            if( synonyms is null )
            {
                throw new ArgumentNullException( "New Units must have at least one name/synonym" );
            }
            else
            {
                Synonyms.AddRange( synonyms );
            }
        }
        private double _toBaseUnitFactor = 0d;
        public double ToBaseUnitFactor
        {
            get { return _toBaseUnitFactor; }

            private set
            {
                if( value == 0d )
                {
                    throw new ArgumentNullException( "Can't init factor to 0 (zero)" );
                }
                _toBaseUnitFactor = value;
            }
        }
        public List<string> Synonyms { get; private set; } = new List<string>();
        public BaseUnit BaseUnit { get; set; }
        public bool ContainsSynonym( string other ) => Synonyms.Contains( other );
        public double ConvertToBaseUnit( double unitValue ) => unitValue * ToBaseUnitFactor;

        /// <summary>
        /// Konverterar värdet "value" till "this" Unit från given Unit "from"
        /// pga SoC så sköter den här bara konversion, men null checkar också, dock bör den endast anropas med verifierade parametrar.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from"></param>
        /// <returns>värdet av konversionen om det gick bra, eller 0 om något gick fel.</returns>
        public double ConvertFrom( double value, Unit from )
        {
            if( from is null )
            {
                throw new ArgumentNullException( "Has to provide a valid unit" );
            }
            if( !from.BaseUnit.UnitCategory.Equals( this.BaseUnit.UnitCategory ) )// kolla att båda enheterna tillhör samma system och kategori
            {
                throw new NotSupportedException( "Can't convert between these unit categories and/or systems" );
            }
            if( this.ToBaseUnitFactor == 0d ) //kan inte ske i dagsläget, la till den pga att vi använder division i beräkningen, men någon kan initiera Unit fel osv säkerställer framtiden.
            {
                throw new DivideByZeroException();
            }
            return ((from.ConvertToBaseUnit( value )) / this.ToBaseUnitFactor);

        }
        public override string ToString()
        {
            //early exit om det råkar finns ett felaktigt instansierad unit object.
            if( !Synonyms.Any() || string.IsNullOrEmpty(Synonyms.FirstOrDefault()) )
            {
                return "Undefined synonyms, check init config";
            }
            //Snygga till output lite iaf :)
            string result = $"{this.Synonyms.FirstOrDefault().Substring(0,1).ToUpper()+this.Synonyms.FirstOrDefault().Substring(1)}";//gör första bokstaven stor i första ordet i Synonyms Listan
            foreach( var item in Synonyms.Skip( 1 ) ) //hoppas över första ordet det är redan skrivet på raden ovan
            {
                result += $",\t{item}";
            }
            return result;
        }
    }
}
