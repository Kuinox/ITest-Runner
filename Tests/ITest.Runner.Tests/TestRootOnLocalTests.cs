using FluentAssertions;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ITest.Runner.Tests
{
    [TestFixture]
    public class TestRootOnLocalTests
    {
        [Test]
        public void local_Tests_run_Fix1()
        {
            LocalTests.Fix1.ResetCallMemory();
            XDocument d = TestRoot.UnattendedRun( Assembly.GetExecutingAssembly(), t => t == typeof( LocalTests.Fix1 ) );
            d.Root.Attribute( "ErrorCount" ).Value.Should().Be( "0" );
            LocalTests.Fix1.Test01CallCount.Should().Be( 1 );
            LocalTests.Fix1.Test02CallCount.Should().Be( 2 );
            LocalTests.Fix1.Test02Messages.Should().BeEquivalentTo( "Hip", "Hop" );

            XDocument d2 = TestRoot.UnattendedRun( Assembly.GetExecutingAssembly(), t => t == typeof( LocalTests.Fix1 ) );
            d2.Root.Attribute( "ErrorCount" ).Value.Should().Be( "0" );
            LocalTests.Fix1.Test01CallCount.Should().Be( 2 );
            LocalTests.Fix1.Test02CallCount.Should().Be( 4 );
            LocalTests.Fix1.Test02Messages.Should().BeEquivalentTo( "Hip", "Hop", "Hip", "Hop" );

            d2.Save( SUTHelper.GetCleanResultFilePath( "Local-Fix1" ) );
        }

        [Test]
        public void local_Tests_run_BuggySetUp()
        {
            XDocument d = TestRoot.UnattendedRun( Assembly.GetExecutingAssembly(), t => t == typeof( LocalTests.BuggySetUp ) );
            d.Root.Attribute( "ErrorCount" ).Value.Should().Be( "1" );
            d.Save( SUTHelper.GetCleanResultFilePath( "Local-BuggySetUp" ) );
        }


        [Test]
        public void local_Tests_run_OneBugguyTestAmong4()
        {
            XDocument d = TestRoot.UnattendedRun( Assembly.GetExecutingAssembly(), t => t == typeof( LocalTests.OneBugguyTestAmong4 ) );

            d.Root.Attribute( "ErrorCount" ).Value.Should().Be( "1" );

            d.Root.Elements( "A" ).Elements( "F" ).Elements( "FSetup" ).Single()
                .Attribute( "RunCount" ).Value.Should().Be( "4" );

            d.Root.Elements( "A" ).Elements( "F" ).Elements( "FTearDown" ).Single()
                .Attribute( "RunCount" ).Value.Should().Be( "4" );

            d.Save( SUTHelper.GetCleanResultFilePath( "Local-OneBugguyTestAmong4" ) );
        }
    }
}
