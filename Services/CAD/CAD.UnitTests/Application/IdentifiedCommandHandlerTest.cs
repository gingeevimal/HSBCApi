using System;

namespace UnitTest.Ordering.Application
{
    using AutoMapper;
    using CAD.UnitTests;
    using global::Ordering.API.Controllers;
    using global::Ordering.Application.Contracts.Persistence;
    using global::Ordering.Application.Features.Orders.Commands.CheckoutOrder;
    using global::Ordering.Application.Features.Orders.Commands.DeleteOrder;
    using global::Ordering.Application.Features.Orders.Commands.UpdateOrder;
    using global::Ordering.Application.Mappings;
    using global::Ordering.Domain.Entities;
    using global::Ordering.Infrastructure.Persistence;
    using global::Ordering.Infrastructure.Repositories;
    using MediatR;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Moq;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Xunit;

    public class IdentifiedCommandHandlerTest
    {
        private readonly Mock<ICAdocumentRepository> _CAdocumentRepository;
        private readonly Mock<IMediator> _mediator;
        private readonly IMapper _mapper;
        private readonly Mock<ILogger<InsertCAdocumentCommandHandler>> _loggerInserteMock;
        private readonly Mock<ILogger<UpdateCAdocumentCommandHandler>> _loggerUpdateMock;
        private readonly Mock<ILogger<DeleteCAdocumentCommandHandler>> _loggerDeleteMock;
        protected readonly HSBCContext _dbContext;
       
        public IdentifiedCommandHandlerTest()
        {
            _CAdocumentRepository = new Mock<ICAdocumentRepository>();
            _dbContext = new CAdocumentMOQ().cadocumentsContext;
            _mediator = new Mock<IMediator>();
            _loggerInserteMock = new Mock<ILogger<InsertCAdocumentCommandHandler>>();
            _loggerUpdateMock = new Mock<ILogger<UpdateCAdocumentCommandHandler>>();
            _loggerDeleteMock = new Mock<ILogger<DeleteCAdocumentCommandHandler>>();
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        #region CRUD Testing

        [Fact]
        public void InsertCAdocumentCommandHandler_Test()
        {
            // Arrange
            var command = InsertCAdocumentCommandVM();
            var validator = new InsertCAdocumentCommandValidator();
            var isValid=validator.ValidateAsync(command);
            //var cmd = new InsertCAdocumentCommand(1, "sym", "name", null, null, null, 1, "description",
                        //null, "https://google.com", null, null, null, new[] { 1, 2 });
            _mediator.Setup(x => x.Send(It.IsAny<IRequest<int>>(), default(System.Threading.CancellationToken)))
               .Returns(Task.FromResult(0));
            var mockRepo = new CAdocumentRepository(_dbContext);

            //Act

            var handler = new InsertCAdocumentCommandHandler(mockRepo, _mapper, _loggerInserteMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var test = handler.Handle(command, cltToken);

            //Assert
            Assert.Equal(3, test.Result);
        } 
        
        [Fact]
        public void UpdateCAdocumentCommandHandler_Test()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();
            var command = UpdateCAdocumentCommandVM();

            _mediator.Setup(x => x.Send(It.IsAny<IRequest<int>>(), default(System.Threading.CancellationToken)))
               .Returns(Task.FromResult(0));

           
            var mockSet = new Mock<DbSet<CAdocument>>();

            var mockRepo = new CAdocumentRepository(_dbContext);

            //Act
           
            var handler = new UpdateCAdocumentCommandHandler(mockRepo, _mapper, _loggerUpdateMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var test=handler.Handle(command, cltToken);

            //Assert
            var result=_dbContext.cadocuments.SingleOrDefault(x => x.id==2);
            Assert.Equal("test", result.department);
        }
        [Fact]
        public void DeleteCAdocumentCommandHandler_Test()
        {
            // Arrange
            var fakeGuid = Guid.NewGuid();
            var command = DeleteCAdocumentCommandVM();
            _mediator.Setup(x => x.Send(It.IsAny<IRequest<int>>(), default(System.Threading.CancellationToken)))
               .Returns(Task.FromResult(0));

           
            var mockSet = new Mock<DbSet<CAdocument>>();

            var mockRepo = new CAdocumentRepository(_dbContext);

            //Act
           
            var handler = new DeleteCAdocumentCommandHandler(mockRepo, _mapper, _loggerDeleteMock.Object);
            var cltToken = new System.Threading.CancellationToken();
            var test=handler.Handle(command, cltToken);

            //Assert
            var result=_dbContext.cadocuments.SingleOrDefault(x => x.id==2);
            Assert.Null(result);
        }
        #endregion

        #region Validation Testing

        [Fact]
        public void InsertCAdocumentValidationHandler_Test()
        {
            // Arrange
            var command = InsertCAdocumentCommandVM();
            var validator = new InsertCAdocumentCommandValidator();
            var isValid = validator.ValidateAsync(command);

            //Assert
            Assert.True(isValid.Result.IsValid);
        }
        [Fact]
        public void UpdateCAdocumentValidationHandler_Test()
        {
            // Arrange
            var command = UpdateCAdocumentCommandVM();
            var validator = new UpdateCAdocumentCommandValidator();
            var isValid = validator.ValidateAsync(command);
            //Assert
            Assert.True(isValid.Result.IsValid);
        }
        #endregion


        private static InsertCAdocumentCommand InsertCAdocumentCommandVM()
        {
            return new InsertCAdocumentCommand { department = "test", ordertype="sd" };
        }
        private static UpdateCAdocumentCommand UpdateCAdocumentCommandVM()
        {
            return new UpdateCAdocumentCommand { Id=2,department = "test", ordertype = "sd" };
        }
        private static DeleteCAdocumentCommand DeleteCAdocumentCommandVM()
        {
            return new DeleteCAdocumentCommand { Id=2 };
        }













        //public void Handler_sends_command_when_order_no_exists()
        //{
        //    // Arrange
        //    var fakeGuid = Guid.NewGuid();
        //    //var fakeOrderCmd = FakeOrderRequest();
        //    var data = FakeOrderRequest1();

        //    //_CAdocumentRepository.Setup(x => x.ExistAsync(It.IsAny<Guid>()))
        //    //   .Returns(Task.FromResult(false));
        //    var command = FakeOrderRequest();

        //    _mediator.Setup(x => x.Send(It.IsAny<IRequest<int>>(), default(System.Threading.CancellationToken)))
        //       .Returns(Task.FromResult(0));

        //    //_CAdocumentRepository.Setup(orderRepo => orderRepo.AddAsync())
        //    // .Returns(Task.FromResult<Order>(FakeOrder()));
        //    var mockSet = new Mock<DbSet<CAdocument>>();

        //    //var mockSet = new Mock<HSBCContext>();
        //    //// mockContext.Setup(c => c.cadocuments).Returns(mockSet.Object);
        //    //mockSet.Setup(p => p.cadocuments).Returns(DbContextMock.GetQueryableMockDbSet<>(data));


        //    //CAdocumentMOQ context = new CAdocumentMOQ();
        //    // var mockRepo = new CAdocumentRepository(_dbContext);
        //    var mockRepo = new CAdocumentRepository(_dbContext);
        //    //var blogs = mockRepo.GetOrdersByUserName("test");

        //    //mockRepo = new Mock<ICAdocumentRepository>();
        //    // mockRepo.Setup(repo => repo.AddAsync(It.IsAny<CAdocument>())).Verifiable();

        //    //Act

        //    var handler = new InsertCAdocumentCommandHandler(mockRepo, _mapper, _logger.Object);
        //    var cltToken = new System.Threading.CancellationToken();
        //    var test = handler.Handle(command, cltToken);

        //    //var result = await handler.Handle(fakeOrderCmd, cltToken);
        //    //var result = await handler.Handle(fakeOrderCmd, cltToken);

        //    //Assert
        //    // Assert.True(test.Equals(3));
        //    Assert.Equal(3, test.Result);
        //    _mediator.Verify(x => x.Send(It.IsAny<IRequest<bool>>(), default(System.Threading.CancellationToken)), Times.Once());
        //}
    //    private static List<CAdocument> FakeOrderRequest1()
    //    {
    //        List<CAdocument> Cats = new List<CAdocument>
    //{
    //     new CAdocument() {department = "Sylvester", avaloqid = "ghg"}
    //};
    //        return Cats;
    //    }

    }

}
