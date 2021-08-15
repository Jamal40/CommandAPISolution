using AutoMapper;
using CommandAPI.Controllers;
using CommandAPI.Data;
using CommandAPI.DTOs;
using CommandAPI.Models;
using CommandAPI.Profiles;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace CommandAPI.Tests
{
    public class CommandsControllerTests : IDisposable
    {
        Mock<ICommandAPIRepo> mockRepo;
        CommandsProfiles realProfiles;
        MapperConfiguration configs;
        IMapper mapper;

        public CommandsControllerTests()
        {
            mockRepo = new Mock<ICommandAPIRepo>();
            realProfiles = new CommandsProfiles();
            realProfiles = new CommandsProfiles();
            configs = new MapperConfiguration(cfg => cfg.AddProfile(realProfiles));
            mapper = new Mapper(configs);
        }

        public void Dispose()
        {
            mockRepo = null;
            mapper = null;
            configs = null;
            realProfiles = null;
        }

        [Fact]
        public void GetAllCommands_ReturnsZeroItems_WhenDBIsEmpty()
        {
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(0));

            var controller = new CommandsController(mockRepo.Object, mapper);

            var res = controller.GetAllCommands();

            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void GetAllCommands_ReturnsZeroItems_WhenDBHasOneResource()
        {
            mockRepo.Setup(repo => repo.GetAllCommands()).Returns(GetCommands(1));

            var controller = new CommandsController(mockRepo.Object, mapper);

            var res = controller.GetAllCommands();

            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void GetCommandById_Returns404NotFound_WhenNonExistentIdProvided()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);
            var controller = new CommandsController(mockRepo.Object, mapper);

            var res = controller.GetCommandById(0);

            Assert.IsType<NotFoundResult>(res.Result);
        }

        [Fact]
        public void GetCommandById_Returns200OK_WhenValidIdProvided()
        {
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(() => new Command
            {
                Id = 1,
                HowTo = "Mock",
                CommandLine = "Mock",
                Platform = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);

            var res = controller.GetCommandById(1);

            Assert.IsType<OkObjectResult>(res.Result);
        }

        [Fact]
        public void GetCommandById_Returns200OKWithObjectOfTypeCommandReadDto_WhenValidIdProvided()
        {
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(() => new Command
            {
                Id = 1,
                HowTo = "Mock",
                CommandLine = "Mock",
                Platform = "Mock"
            });
            var controller = new CommandsController(mockRepo.Object, mapper);

            var res = controller.GetCommandById(1);

            Assert.IsType<ActionResult<CommandReadDTO>>(res);
        }

        [Fact]
        public void CreateCommand_CorrectObject_WhenValidObjectIsProvided()
        {
            //Arrange
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.CreateCommand(new CommandCreateDTO { });
            //Assert
            Assert.IsType<ActionResult<CommandReadDTO>>(result);
        }

        [Fact]
        public void CreateCommand_Returns201Created_WhenValidObjectIsProvided()
        {
            var controller = new CommandsController(mockRepo.Object, mapper);
            //Act
            var result = controller.CreateCommand(new CommandCreateDTO { });
            //Assert
            Assert.IsType<CreatedAtActionResult>(result.Result);
        }

        [Fact]
        public void UpdateCommand_Return204NoContent_WhenValidObjectIsProvided()
        {
            mockRepo.Setup(repo => repo.GetCommandById(1)).Returns(() => new Command
            {
                Id = 1,
                HowTo = "Mock",
                CommandLine = "Mock",
                Platform = "Mock"
            });

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.UpdateCommand(1, new CommandUpdateDTO { });

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void UpdateCommand_Returns404NotFound_WhenNonExistentIdIsPrivided()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0)).Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.UpdateCommand(0, new CommandUpdateDTO { });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void PartialUpdateCommand_Returns404NotFound_WhenNonExistentIdIsProvided()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0))
                                       .Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.PartialUpdateCommand(0,
                new JsonPatchDocument<CommandUpdateDTO> { });

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns204NoContent_WhenValidIdIsProvided()
        {
            mockRepo.Setup(repo => repo.GetCommandById(1))
                                        .Returns(() => new Command
                                        {
                                            Id = 1,
                                            HowTo = "Mock",
                                            CommandLine = "Mock",
                                            Platform = "Mock"
                                        });

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.DeleteCommand(1);

            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteCommand_Returns404NotFound_WhenNonExistentIdIsProvided()
        {
            mockRepo.Setup(repo => repo.GetCommandById(0))
                           .Returns(() => null);

            var controller = new CommandsController(mockRepo.Object, mapper);

            var result = controller.DeleteCommand(0);

            Assert.IsType<NotFoundResult>(result);
        }

        private List<Command> GetCommands(int number)
        {
            var commands = new List<Command>();
            if (number > 0)
            {
                commands.Add(new Command
                {
                    Id = 0,
                    HowTo = "How to generate a migration",
                    CommandLine = "dotnet ef migrations add <Name of Migration>",
                    Platform = ".Net Core EF"
                });
            }

            return commands;
        }
    }
}
