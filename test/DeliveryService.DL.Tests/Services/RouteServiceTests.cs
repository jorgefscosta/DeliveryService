using DeliveryService.API;
using DeliveryService.DAL.Models;
using DeliveryService.DL.Models;
using DeliveryService.DL.Repositories;
using DeliveryService.DL.Services;
using Moq;
using System.Collections.Generic;
using Xunit;
using System.Linq;
using AutoMapper;
using DeliveryService.DL.Helpers;
using DeliveryService.DAL.Contexts;

namespace DeliveryService.DL.Tests.Services
{
    public class RouteServiceTests : IClassFixture<TestFixture<Startup>>
    {
        private Mock<IRelationshipRepository<SHIPS_TO,Warehouse,Warehouse>> Repository { get; }

        private IRouteService Service { get; }
       
        public RouteServiceTests(TestFixture<Startup> fixture)
        {
            var origin = new Warehouse() { Name = "A", Id = 1 };
            var destiny = new Warehouse() { Name = "D", Id = 2 };
            var routesEntity = new List<RouteResponse>
            {
                new RouteResponse{Origin=origin,Destiny=destiny,Hops=1,TotalCost=5, TotalTime=10},
                new RouteResponse{Origin=origin,Destiny=destiny,Hops=2,TotalCost=1, TotalTime=5},
                new RouteResponse{Origin=origin,Destiny=destiny,Hops=3,TotalCost=10, TotalTime=1},
            };

            var shipsEntity = new List<SHIPS_TO>
            {
                new SHIPS_TO{Cost = 1,Time = 1},
                new SHIPS_TO{Cost = 2,Time = 2},
                new SHIPS_TO{Cost = 3,Time = 3}
            };

            Repository = new Mock<IRelationshipRepository<SHIPS_TO,Warehouse,Warehouse>>();

            Repository.Setup(x => x.GetAll())
                .Returns(shipsEntity);

            Repository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int o, int d) => shipsEntity.Where(s => s.Cost==3));

            Repository.Setup(x => x.Insert(It.IsAny<SHIPS_TO>(), It.IsAny<Warehouse>(), It.IsAny<Warehouse>()))
                .Callback((SHIPS_TO entity, Warehouse from, Warehouse to) => shipsEntity.Add(entity));

            Repository.Setup(x => x.Update(It.IsAny<SHIPS_TO>(), It.IsAny<Warehouse>(), It.IsAny<Warehouse>()))
                .Callback((SHIPS_TO entity, Warehouse from, Warehouse to) => shipsEntity[shipsEntity.FindIndex(x => x.Cost == entity.Cost)] = entity);

            Repository.Setup(x => x.Delete(It.IsAny<SHIPS_TO>(), It.IsAny<Warehouse>(), It.IsAny<Warehouse>()))
            .Callback((SHIPS_TO entity, Warehouse from, Warehouse to) => shipsEntity.RemoveAt(shipsEntity.FindIndex(x => x.Cost == entity.Cost && x.Time == entity.Time)));

            var dataContext = (DataContext)fixture.Server.Host.Services.GetService(typeof(DataContext));
            var routeSetup = new RouteSetup("2", "20");
            var mapper = (IMapper)fixture.Server.Host.Services.GetService(typeof(IMapper));
            Service = new RouteService(dataContext, routeSetup,Repository.Object,mapper);
        }

        [Fact]
        public void Can_Get_All()
        {
            // Act
            var entities = Service.Get();
            // Assert
            Repository.Verify(x => x.GetAll(), Times.Once);
            Assert.Equal(3,entities.Count());
        }

        [Fact]
        public void Can_Get_Single()
        {
            // Arrange
            var originId = 1;
            var destinyId = 2;

            // Act
            var l = Service.GetDirectRoute(originId, destinyId).Single();

            // Assert
            Repository.Verify(x => x.GetById(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            Assert.Equal(3, l.Cost);
            Assert.Equal(3, l.Time);
        }

        [Fact]
        public void Can_Filter_Entities()
        {
            // Arrange
            var originName = "A";
            var destinyName = "D";
            var options = new RouteOptionsModel(2,null,false);

            // Act
            var filteredEntities = Service.Get(originName,destinyName,options);

            // Assert
            Assert.Equal(2, filteredEntities.Count());
        }

        [Fact]
        public void Can_Insert_Entity()
        {
            // Arrange
            var entity = new ShipsToRequest
            {
                Origin = new WarehouseResponse() { Id = 1 },
                Destiny = new WarehouseResponse() { Id = 4 },
                Cost = 100,
                Time = 50
            };

            // Act
            Service.Create(entity);

            // Assert
            Repository.Verify(x => x.Insert(It.IsAny<SHIPS_TO>(), It.IsAny<Warehouse>(), It.IsAny<Warehouse>()), Times.Once);
            var entities = Service.Get();
            Assert.Equal(4, entities.Count());
        }


        [Fact]
        public void Can_Update_Entity()
        {
            // Arrange
            var entity = new ShipsToRequest
            {
                Origin = new WarehouseResponse() { Id = 1 },
                Destiny = new WarehouseResponse() { Id = 3 },
                Cost = 3,
                Time = 30
            };
            // Act
            Service.Update(entity);

            // Assert
            Repository.Verify(x => x.Update(It.IsAny<SHIPS_TO>(), It.IsAny<Warehouse>(), It.IsAny<Warehouse>()), Times.Once);
            var entityResult = Service.GetDirectRoute(entity.Origin.Id,entity.Destiny.Id).SingleOrDefault();
            Assert.Equal(3, entityResult.Cost);
            Assert.Equal(30, entityResult.Time);
            var entities = Service.Get();
            Assert.Equal(3, entities.Count());
        }

        [Fact]
        public void Can_Remove_Entity()
        {
            // Arrange
            var entity = new ShipsToRequest
            {
                Origin = new WarehouseResponse() { Id = 1 },
                Destiny = new WarehouseResponse() { Id = 4 },
                Cost = 3,
                Time = 3
            };

            // Act
            Service.Remove(entity);

            // Assert
            Repository.Verify(x => x.Delete(It.IsAny<SHIPS_TO>(), It.IsAny<Warehouse>(), It.IsAny<Warehouse>()), Times.Once);
            var entityResult = Service.GetDirectRoute(entity.Origin.Id, entity.Destiny.Id);
            var entities = Service.Get();
            Assert.Empty(entityResult);
            Assert.Equal(2, entities.Count());
        }
    }
}
