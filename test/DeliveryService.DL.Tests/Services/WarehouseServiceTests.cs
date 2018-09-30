using DeliveryService.API;
using DeliveryService.DAL.Models;
using DeliveryService.DL.Repositories;
using DeliveryService.DL.Services;
using System;
using System.Collections.Generic;
using Xunit;
using Moq;
using AutoMapper;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Linq;
using DeliveryService.DL.Models;

namespace DeliveryService.DL.Tests.Services
{
    public class WarehouseServiceTests : IClassFixture<TestFixture<Startup>>
    {
        private Mock<IBaseRepository<Warehouse>> Repository { get; }

        private IWarehouseService Service { get; }

        public WarehouseServiceTests(TestFixture<Startup> fixture)
        {
            var entity = new List<Warehouse>
            {
                new Warehouse
                {
                    Id = 1,
                    Name = "Warehouse"
                }
            };

            Repository = new Mock<IBaseRepository<Warehouse>>();

            Repository.Setup(x => x.GetAll())
                .Returns(entity);

            Repository.Setup(x => x.GetById(It.IsAny<int>()))
                .Returns((int id) => entity.Find(s => s.Id == id));

            Repository.Setup(x => x.Insert(It.IsAny<Warehouse>()))
                .Callback((Warehouse label) => entity.Add(label));

            Repository.Setup(x => x.Update(It.IsAny<Warehouse>()))
                .Callback((Warehouse label) => entity[entity.FindIndex(x => x.Id == label.Id)] = label);

            Repository.Setup(x => x.Delete(It.IsAny<int>()))
            .Callback((int id) => entity.RemoveAt(entity.FindIndex(x => x.Id == id)));

            var mapper = (IMapper)fixture.Server.Host.Services.GetService(typeof(IMapper));
            var baseService = new BaseService<Warehouse>(Repository.Object);

            Service = new WarehouseService(baseService, mapper);
        }

        [Fact]
        public void Can_Get_All()
        {
            // Act
            var entities = Service.Get();
            // Assert
            Repository.Verify(x => x.GetAll(), Times.Once);
            Assert.Single(entities);
        }

        [Fact]
        public void Can_Get_Single()
        {
            // Arrange
            var testId = 1;

            // Act
            var l = Service.GetById(testId);

            // Assert
            Repository.Verify(x => x.GetById(testId), Times.Once);
            Assert.Equal("Warehouse", l.Name);
        }

        [Fact]
        public void Can_Insert_Entity()
        {
            // Arrange
            var entity = new WarehouseResponse
            {
                Id = 2,
                Name = "Warehouse 2"
            };

            // Act
            Service.Add(entity);
            
            // Assert
            Repository.Verify(x => x.Insert(It.IsAny<Warehouse>()), Times.Once);
            var entities = Service.Get();
            Assert.Equal(2, entities.Count());
        }


        [Fact]
        public void Can_Update_Entity()
        {
            // Arrange
            var entity = new WarehouseResponse
            {
                Id = 1,
                Name = "New Name"
            };
            // Act
            Service.Update(entity);

            // Assert
            Repository.Verify(x => x.Update(It.IsAny<Warehouse>()), Times.Once);
            var entityResult = Service.GetById(1);
            Assert.Equal("New Name", entityResult.Name);
        }
        
        [Fact]
        public void Can_Remove_Entity()
        {
            // Arrange
            var warehouseId = 1;

            // Act
            Service.Remove(warehouseId);

            // Assert
            Repository.Verify(x => x.Delete(It.IsAny<int>()), Times.Once);
            var entityResult = Service.GetById(1);
            Assert.Null(entityResult);
        }

    }
}