using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

using DAPManSWebReports.Infrastructure.Models;

namespace DAPManSWebReports.Infrastructure.Test.ModelsTest
{
    public class DataSourceTest
    {
        [Fact]
        public void CanCreateDataSource()
        {
            // Act - действие
            var dataSource = new DataSource();

            // Assert - утверждение
            Assert.NotNull(dataSource);
        }

        [Fact]
        public void CanSetAndGetProperties()
        {
            // Arrange - подготовка
            var expectedName        = "TestName";
            var expectedDescription = "TestDescription";
            var expectedProvider    = "TestProvider";
            var expectedType        = "TestType";
            var expectedServer      = "TestServer";
            var expectedDataBase    = "TestDataBase";
            var expectedDbUser      = "TestUser";
            var expectedDbPassword  = "TestPassword";
            var expectedSystem      = 1;
            var expectedLastUpdate  = DateTime.Now;
            var expectedLastUser    = "TestLastUser";
            var expectedId          = 123;

            var dataSource = new DataSource
            {
                Name        = expectedName,
                Description = expectedDescription,
                Provider    = expectedProvider,
                Type        = expectedType,
                Server      = expectedServer,
                DataBase    = expectedDataBase,
                DbUser      = expectedDbUser,
                DbPassword  = expectedDbPassword,
                System      = expectedSystem,
                LastUpdate  = expectedLastUpdate,
                LastUser    = expectedLastUser,
                Id          = expectedId
            };

            // Assert
            Assert.Equal(expectedName, dataSource.Name);
            Assert.Equal(expectedDescription, dataSource.Description);
            Assert.Equal(expectedProvider, dataSource.Provider);
            Assert.Equal(expectedType, dataSource.Type);
            Assert.Equal(expectedServer, dataSource.Server);
            Assert.Equal(expectedDataBase, dataSource.DataBase);
            Assert.Equal(expectedDbUser, dataSource.DbUser);
            Assert.Equal(expectedDbPassword, dataSource.DbPassword);
            Assert.Equal(expectedSystem, dataSource.System);
            Assert.Equal(expectedLastUpdate, dataSource.LastUpdate);
            Assert.Equal(expectedLastUser, dataSource.LastUser);
            Assert.Equal(expectedId, dataSource.Id);
        }
    }
}
