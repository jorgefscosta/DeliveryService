using DeliveryService.API;
using DeliveryService.DAL.Models;
using DeliveryService.DL.Models;
using DeliveryService.DL.Repositories;
using DeliveryService.DL.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Linq;
using AutoMapper;
using DeliveryService.DL.Helpers;
using DeliveryService.DAL.Contexts;

namespace DeliveryService.DL.Tests.Services
{
    public class RouteServiceTests : IClassFixture<TestFixture<Startup>>
    {
        private Mock<IRelationshipRepository<SHIPS_TO>> Repository { get; }

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
                new SHIPS_TO{OriginId=1, Cost = 1,Time = 1, DestinyId=2},
                new SHIPS_TO{OriginId=1, Cost = 2,Time = 2, DestinyId=3},
                new SHIPS_TO{OriginId=2, Cost = 1,Time = 1, DestinyId=4},
                new SHIPS_TO{OriginId=3, Cost = 3,Time = 3, DestinyId=4}
            };

            Repository = new Mock<IRelationshipRepository<SHIPS_TO>>();

            Repository.Setup(x => x.GetAll())
                .Returns(shipsEntity);

            Repository.Setup(x => x.GetById(It.IsAny<int>(), It.IsAny<int>()))
                .Returns((int o, int d) => shipsEntity.Where(s => s.OriginId==o && s.DestinyId==d));

            Repository.Setup(x => x.Insert(It.IsAny<SHIPS_TO>()))
                .Callback((SHIPS_TO entity) => shipsEntity.Add(entity));

            Repository.Setup(x => x.Update(It.IsAny<SHIPS_TO>()))
                .Callback((SHIPS_TO entity) => shipsEntity[shipsEntity.FindIndex(x => x.OriginId== entity.OriginId && x.DestinyId== entity.DestinyId)] = entity);

            Repository.Setup(x => x.Delete(It.IsAny<SHIPS_TO>()))
            .Callback((SHIPS_TO entity) => shipsEntity.RemoveAt(shipsEntity.FindIndex(x => x.OriginId == entity.OriginId && x.DestinyId == entity.DestinyId)));

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
            Assert.Equal(4,entities.Count());
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
            Assert.Equal(1, l.OriginId);
            Assert.Equal(2, l.DestinyId);
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
            var entity = new ShipsToResponse
            {
                OriginId = 1,
                DestinyId = 4,
                Cost = 100,
                Time = 50
            };

            // Act
            Service.Create(entity);

            // Assert
            Repository.Verify(x => x.Insert(It.IsAny<SHIPS_TO>()), Times.Once);
            var entities = Service.Get();
            Assert.Equal(5, entities.Count());
        }


        [Fact]
        public void Can_Update_Entity()
        {
            // Arrange
            var entity = new ShipsToResponse
            {
                OriginId = 1,
                DestinyId = 3,
                Cost = 50,
                Time = 100
            };
            // Act
            Service.Update(entity);

            // Assert
            Repository.Verify(x => x.Update(It.IsAny<SHIPS_TO>()), Times.Once);
            var entityResult = Service.GetDirectRoute(entity.OriginId,entity.DestinyId).SingleOrDefault();
            Assert.Equal(50, entityResult.Cost);
            Assert.Equal(100, entityResult.Time);
            Assert.Equal(1, entityResult.OriginId);
            Assert.Equal(3, entityResult.DestinyId);
        }

        [Fact]
        public void Can_Remove_Entity()
        {
            // Arrange
            var entity = new ShipsToResponse
            {
                OriginId = 1,
                DestinyId = 3,
                Cost = 100,
                Time = 50
            };

            // Act
            Service.Remove(entity);

            // Assert
            Repository.Verify(x => x.Delete(It.IsAny<SHIPS_TO>()), Times.Once);
            var entityResult = Service.GetDirectRoute(entity.OriginId, entity.DestinyId);
            Assert.Empty(entityResult);
        }
    }
}
