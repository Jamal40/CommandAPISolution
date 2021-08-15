using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using CommandAPI.Models;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command testCommand;
        public CommandTests()
        {
            testCommand = new Command
            {
                HowTo = "Do something awesome",
                Platform = "xUnit",
                CommandLine = "dotnet test"
            };
        }
        [Fact]
        public void CanChangeHowTo()
        {

            testCommand.HowTo = "Execute Unit Test";

            Assert.Equal("Execute Unit Test", testCommand.HowTo);
        }

        [Fact]
        public void CanChangePlatform()
        {

            testCommand.Platform = "nUnit";

            Assert.Equal("nUnit", testCommand.Platform);
        }

        [Fact]
        public void CanChangeCommandLine()
        {
            testCommand.CommandLine = "dotnet run test";

            Assert.Equal("dotnet run test", testCommand.CommandLine);
        }

        public void Dispose()
        {
            testCommand = null;
        }
    }
}
