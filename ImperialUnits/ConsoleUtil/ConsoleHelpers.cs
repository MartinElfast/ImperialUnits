using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ImperialUnits
{
    public partial class ImperialConverterTerminal
    {//ursäkta om formatteringen här med precompiler statementsen det ser hemskt ut, men NUnit stödjer inte tester av privata instansmetoder. Det var ett effektivt sätt att testa input i det här fallet, utan att kompromissa med programmets integritet :)
#if (DEBUG) 
        public double HandleInput( string input )
        {
            double testReturnValue = 0;
#elif (RELEASE)
        internal void HandleInput( string input )
        {
#endif
            //Håller det enkelt och tvingar användaren att följa instruktionerna att input ska delas upp med mellanslag. 
            //Kandidat för att förbättra det här (långsiktigt, om vi ska stanna i console UI) kan vara att använda Regex och substring för att leta efter kända ord/förkortningar.
            string[] splitInput = input.Split(" ");

            if( splitInput.Length == 1 ) //Kolla om det enda ordet i input är ett kännt kommando
            {
                if( TryParseCommand( input ) ) ; //kör sina egna kommandon, returnerar false om inget kommndo kunde tolkas ur input, och om input bara var ett ord 
                else
                {
                    Console.WriteLine( "Command not recognized, type 'help' to see instructions." );
                }
            }
            else if( splitInput.Length == 4 ) //Finns 4 ord i input, igen förenklar allt och det är ett krav att separera input med mellanrum, så jag baserar resterande logik på det här antagandet.
            {
                if( TryParseConversion( splitInput, out double result, out Unit fromUnit, out Unit toUnit ) ) //Försök tolka att varje del av input är korrekt, enligt nuvarande krav, 
                {
                    double validInputResult = toUnit.ConvertFrom( result, fromUnit );

                    PrintResult( validInputResult, toUnit );
#if(DEBUG)
                    testReturnValue = validInputResult;
#endif
                }
            }
            else
            {
                Console.WriteLine( "Input not recognized type 'help' to see instructions." );
            }
#if(DEBUG)
            return testReturnValue; //only for tests, metoden har inget return värde när man bygger med "Release".
#endif
        }
        internal bool TryParseCommand( string input )
        {
            if( Enum.TryParse( input, out ConsoleCommand consoleCommand ) )
            {
                switch( consoleCommand )
                {
                    case ConsoleCommand.help: PrintInstructions(); return true;
                    case ConsoleCommand.list: PrintConversionsList(); return true;
                    case ConsoleCommand.exit: PrintExitMessage(); isRunning = false; return true;
                    default: return false; //kan logiskt sett inte ske.. men om någon gör fel när de implmeneterar ett nytt command i framtiden bör den vara här.
                }
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// Ska bara precis som namnet tyder, tolka input (SoC). 
        /// Om vi vill ändra vilka krav input har 
        /// </summary>
        /// <param name="splitInput"></param>
        /// <param name="fromUnit"></param>
        /// <param name="toUnit"></param>
        /// <returns>true om input är validerad, samt returnerar alla verfierade värden (Antal FrånEnhet och TillEnhet)
        /// false om någon input inte stämmer.</returns>
        private bool TryParseConversion( string[] splitInput, out double result, out Unit fromUnit, out Unit toUnit )
        {
            result = 0d;
            fromUnit = null; //återställ out parameterar då nedan logik beror på deras state :/ var en avvägning
            toUnit = null; //samma som ovan

            if( !double.TryParse( splitInput[0], out result ) ) //första värdet ska vara numeriskt
            {
                Console.WriteLine( "Input must begin with numeric value, type 'help' for instructions." );
                return false;
            }

            fromUnit = units.GetUnitBySynonymOrDefault( splitInput[1] );
            toUnit = units.GetUnitBySynonymOrDefault( splitInput[3] );

            if( fromUnit is null || toUnit is null ) //om någon av enheter är null så är de inte supportade/finns inte i vårt register.
            {
                Console.WriteLine( "Unsupported unit, type 'list' to find all supported units" );
                return false;
            }

            //kolla att båda in av samma kategori
            if( fromUnit.BaseUnit.UnitCategory.Equals( toUnit.BaseUnit.UnitCategory ) ) //hade räckt med att bara jämföra BaseUnit men hela syftet med UnitCategory är att skilja på enheter
            {
                return true;
            }
            else
            {
                Console.WriteLine( "Unsupported combination of units, can only convert from length to length or weight to weight, type 'list' to find all supported units" );
                return false;
            }
        }
        internal void PrintResult( double result, Unit toUnit )
        {
            Console.WriteLine( $"Conversion is {result} {toUnit.Synonyms.FirstOrDefault()}." );
        }
        internal void PrintWelcomeMessage()
        {
            Console.WriteLine( "Welcome to the imperial units converter" );
            PrintInstructions();
            Console.WriteLine( "Enter a conversion and press enter" );
        }
        internal static void PrintExitMessage()
        {
            Console.WriteLine();
            Console.WriteLine( "Thank you, please come again :)" );
        }
        internal void PrintConversionsList()
        {
            Console.WriteLine();
            Console.WriteLine( "List of supported units: " );

            foreach( Unit unit in units.Catalogue )
            {
                Console.WriteLine( unit.BaseUnit.UnitCategory + "\t" + unit.ToString() );
            }
        }
        internal void PrintInstructions()
        {
            Console.WriteLine();
            Console.WriteLine( "type 'help' to view this message again." );
            Console.WriteLine( "type 'list' to list possible conversions." );
            Console.WriteLine( "type 'exit' to close the application." );
            Console.WriteLine();
            Console.WriteLine( "Usage: [value]_[from]_in_[to]" );
            Console.WriteLine( "Separate each part of the input with spaces, in place of underscores above." );
            Console.WriteLine( "Example: 3 foot in yd" );
            Console.WriteLine();
        }
    }
}
