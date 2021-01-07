using System;

namespace ImperialUnits
{
    public partial class ImperialConverterTerminal
    {
        public bool isRunning = true; //för ProgramLoop, see 
        public Units units = new Units();

        static void Main( string[] args )
        {            
            new ImperialConverterTerminal().ProgramLoop();
        }

        private void ProgramLoop()
        {
            PrintWelcomeMessage();
            while( isRunning )
            {
                string input = String.Empty;
                                
                while( string.IsNullOrWhiteSpace( input = Console.ReadLine().ToLower().Trim() ) )
                { // om man skrev endast whitespace eller ingen input ge följande meddelande.
                    Console.WriteLine( "Type 'help' to get more info or 'exit' to close application." );
                    Console.WriteLine();
                }
                HandleInput( input );
            }
            Console.ReadLine();
        }
    }
}
