using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace ImperialUnits
{
    /// <summary>
    /// Håller alla registrerade enheter och deras konversionser (omvandlingsfaktorer).
    /// Initieras vid uppstart, med de enheter som önskas.
    /// </summary>
    public class Units
    {
        public List<Unit> Catalogue { get; private set; } = new List<Unit>();
        public List<BaseUnit> BaseUnits { get; private set; } = new List<BaseUnit>();
        public Units()
        {
            Init();
        }
        /// <summary>
        /// Exempel på hur man skulle kunna injecera en egen katalog med önskade enheter
        /// </summary>
        /// <param name="initConfig"></param>
        public Units( Dictionary<BaseUnit, List<Unit>> initConfig ) 
        {
            foreach( KeyValuePair<BaseUnit,List<Unit>> entry in initConfig )
            {
                BaseUnits.Add( entry.Key );
                Catalogue.AddRange( entry.Value );
            }
        }
        void Init() //Test
        {
            //Initiera alla basenheter först och spara i instansvariabler.
            BaseUnit Inch   = new BaseUnit( "ImperialLength", 1d, "inch", "in" );
            BaseUnit Ounce  = new BaseUnit( "ImperialWeight", 1d, "ounce", "oz");
            BaseUnit Meter  = new BaseUnit( "MetricLength", 1d, "meter", "metre", "m");
            
            //init katalogen med de enheter som önskas
            //om det här ska vara tänkt mer som ett omfattande bibliotek bör det här flyttas ut till config fil av något slag
            //Då skulle man kunna ange en config per instans av Katalogen.
            Catalogue = new List<Unit>()
            {
                //ImperialLength
                new Unit( Inch, 1d/1000d,   "thou", "th" ),
                new Unit( Inch, 1d,         "inch", "in" ),
                new Unit( Inch, 12d,        "feet", "foot", "ft" ),
                new Unit( Inch, 36d,        "yard", "yd" ),
                new Unit( Inch, 7920d,      "furlong", "fur" ),
                //ImperialWeight
                new Unit( Ounce, 1d,        "ounce", "oz"),
                new Unit( Ounce, 12d,       "pound", "lb"),
                //MetricLength
                new Unit( Meter, 1d,        "meter", "metre", "m")
                //osv...
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="synonym"></param>
        /// <returns>Unit som matchar input "synonym", eller null om enheten inte finns</returns>
        public Unit GetUnitBySynonymOrDefault( string synonym ) => Catalogue.FirstOrDefault( x => x.Synonyms.Contains( synonym.ToLower().Trim() ) );
    }
}

