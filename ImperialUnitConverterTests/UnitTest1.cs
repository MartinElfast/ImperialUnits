using NUnit.Framework;
using ImperialUnits;

namespace ImperialUnitConverterTests
{
    [TestFixture]
    public class Tests
    {
        ImperialConverterTerminal p;
        Units units;        
        Unit yard;
        Unit inch;
        Unit fur;
        Unit emptyUnit;
        [OneTimeSetUp]
        public void Setup()
        {
            emptyUnit = new Unit(new BaseUnit("",1d,""),1d,"");
            p = new ImperialConverterTerminal();
            units = p.units;
            yard = p.units.GetUnitBySynonymOrDefault( "yd" );
            inch = p.units.GetUnitBySynonymOrDefault( "Inch" );
            fur = p.units.GetUnitBySynonymOrDefault( "fur" );
        }
        [Test]
        public void EmptyUnitWillExplode()
        {
            Assert.IsEmpty( emptyUnit.ToString() );
        }
        [Test]
        public void GetUnitFromSynYd()
        {
            Assert.AreEqual( yard, p.units.GetUnitBySynonymOrDefault( "yd" ) );
        }
        [Test]
        public void GetUnitFromSynInch()
        {
            Assert.AreEqual( inch, p.units.GetUnitBySynonymOrDefault( "inch" ) );
        }

        [Test]
        public void TestConvertThreeYardToInch()
        {
            Assert.AreEqual( 108d, inch.ConvertFrom( 3d, yard ) );
        }
        [Test]
        public void TestConvertPointEightyTwoFurToYard()
        {
            Assert.AreEqual( 180.39999999999998d, yard.ConvertFrom( 0.82d, fur ) );
        }
        [Test]
        public void TestConvertThreeYardToFeet()
        {
            Assert.AreEqual( 9d, p.units.GetUnitBySynonymOrDefault( "ft" ).ConvertFrom( 3d, yard ) );
        }
        [Test]
        public void AnInchIsAnInch()
        {
            Assert.AreEqual( 1d, units.GetUnitBySynonymOrDefault( "Inch" ).ConvertToBaseUnit( 1d ) );
        }

        [Test]
        public void TestConvertToBaseUnit()
        {
            Assert.AreEqual( 2d, units.GetUnitBySynonymOrDefault( "Inch" ).ConvertToBaseUnit( 2d ) );
            Assert.AreEqual( 36d, units.GetUnitBySynonymOrDefault( "yd" ).ConvertToBaseUnit( 1d ) );
            Assert.AreEqual( 12d, units.GetUnitBySynonymOrDefault( "ft" ).ConvertToBaseUnit( 1d ) );
            Assert.AreEqual( 6d, units.GetUnitBySynonymOrDefault( "ft" ).ConvertToBaseUnit( 0.5d ) );
            Assert.AreEqual( 7920d, units.GetUnitBySynonymOrDefault( "fur" ).ConvertToBaseUnit( 1d ) );
        }

        [Test]
        public void TestHandleInputOneYardIsThreeFeet()
        {
            Assert.AreEqual( 3d, p.HandleInput( "1 yd in ft" ) );
        }
        [Test]
        public void TestHandleInputSomeMore()
        {
            Assert.AreEqual( 0d, p.HandleInput( "1 mile in ft" ) ); //ska returnera 0 då det anger att något gick fel med parse, i release så skriver den typ av error till terminalen.
            Assert.AreEqual( 20d, p.HandleInput( "20 in in in" ) );
            Assert.AreEqual( 1000d, p.HandleInput( "1 in in thou" ) );
            Assert.AreEqual( 36000d, p.HandleInput( "1 yd in thou" ) );
        }
    }
}