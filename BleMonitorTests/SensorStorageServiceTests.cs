using BleTempMonitor.Extensions;
using BleTempMonitor.Models;
using BleTempMonitor.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitorTests
{
    [TestFixture]
    public class SensorStorageServiceTests
    {
        private Mock<ISensorDatabaseService> _db;
        private SensorStorageService _storage;

        [SetUp]
        public void Setup()
        {
            _db = new Mock<ISensorDatabaseService>();
            _storage = new SensorStorageService(_db.Object);
        }

        [Test]
        public void Call_AddOrUpdate_GuidExistsAliasMatches_SaveItemAsyncNotCalled()
        {
            var model = new SensorModel()
            {
                Guid = new Guid(),
                Alias = "a",
                ID = 1
            };
            var guid = model.Guid;
            var alias = "a";

            var result = _storage.AddOrUpdate(guid, alias);

            _db.Setup(d => d.GetItemAsync(guid)).Returns(Task.FromResult(model));

            _db.Verify(d => d.SaveItemAsync(model), Times.Never);

        }

        [Test]
        public void Call_AddOrUpdate_GuidExistsAliasNotMatched_SaveItemAsyncCalled()
        {
            var model = new SensorModel()
            {
                Guid = new Guid(),
                Alias = string.Empty,
                ID = 1
            };
            var guid = model.Guid;
            var alias = "a";

            var result = _storage.AddOrUpdate(guid, alias);

            _db.Setup(d => d.GetItemAsync(guid)).Returns(Task.FromResult(model));

            _db.Verify(d => d.SaveItemAsync(It.IsAny<SensorModel>()));

        }

        [Test]
        public void Call_AddOrUpdate_GuidDoesNotExist_SaveItemAsyncCalled()
        {
            var guid = Guid.NewGuid();

            var model = new SensorModel()
            {
                Guid = guid,
                Alias = guid.GetTempAlias(),
                ID = 0
            };
            var alias = "";

            var result = _storage.AddOrUpdate(guid, alias);

            _db.Setup(d => d.GetItemAsync(guid)).Returns(Task.FromResult<SensorModel?>(null));

            _db.Verify(d => d.SaveItemAsync(It.IsAny<SensorModel>()));

        }


        [Test]
        public void Call_AddOrUpdate_GuidDoesNotExist_GetItemAsyncCalled()
        {
            var guid = Guid.NewGuid();

            var model = new SensorModel()
            {
                Guid = Guid.NewGuid(),
                Alias = guid.GetTempAlias(),
                ID = 1
            };
            var alias = "";

            var result = _storage.AddOrUpdate(guid, alias);

            _db.Setup(d => d.GetItemAsync(guid)).Returns(Task.FromResult<SensorModel?>(null));

            _db.Setup(d => d.SaveItemAsync(model));

            _db.Verify(d => d.GetItemAsync(guid), Times.Exactly(2));

        }

        [Test]
        public void Call_AddOrUpdate_GuidDoesNotExist_ReturnSensorModel()
        {
            var guid = Guid.NewGuid();

            var model = new SensorModel()
            {
                Guid = Guid.NewGuid(),
                Alias = guid.GetTempAlias(),
                ID = 1
            };
            var alias = "";


            _db.SetupSequence(d => d.GetItemAsync(It.Is<Guid>(g => g == guid)))
                .Returns(Task.FromResult<SensorModel?>(null))
                .Returns(Task.FromResult<SensorModel?>(model));

            _db.Setup(d => d.SaveItemAsync(model));

            var result = _storage.AddOrUpdate(guid, alias);

            Assert.That(result.Result, Is.EqualTo(model));

        }


        [Test]
        public void CallGetAlias_AnyInt_GetItemAliasAsyncCalled()
        {
            var id = 1;

            var result = _storage.GetAlias(id);

            _db.Verify(db => db.GetItemAliasAsync(It.IsAny<int>()));
        }

        [Test]
        public void CallGetAlias_AnyInt_ReturnString()
        {
            var id = 1;
            var alias = "a";

            _db.Setup(db => db.GetItemAliasAsync(It.IsAny<int>())).Returns(Task.FromResult<string>(alias));

            var result = _storage.GetAlias(id);


            Assert.That(result.Result, Is.EqualTo(alias));
        }

        [Test]
        public void CallGetAlias_AnyGuid_GetItemAliasAsyncCalled()
        {
            var id = Guid.NewGuid();

            var result = _storage.GetAlias(id);

            _db.Verify(db => db.GetItemAliasAsync(It.IsAny<Guid>()));
        }

        [Test]
        public void CallGetAlias_AnyGuid_ReturnString()
        {
            var id = Guid.NewGuid();
            var alias = "a";

            _db.Setup(db => db.GetItemAliasAsync(It.IsAny<Guid>())).Returns(Task.FromResult<string>(alias));

            var result = _storage.GetAlias(id);


            Assert.That(result.Result, Is.EqualTo(alias));
        }

        [Test]
        public void CallClearData_ClearLogItemTableCalled()
        {
    
            var result = _storage.ClearLogData();

            _db.Verify(db => db.ClearLogItemTable());

        }

        [Test]
        public void CallInsertData_InsertLogItemAsyncCalled()
        {
            int sensorId = 1;
            double voltage = 2.2;
            double temperature = 3.3;
            var logItem = new LogItem(voltage, temperature, sensorId);

            var result = _storage.InsertLogData(sensorId, voltage, temperature);

            _db.Verify(db => db.InsertLogItemAsync(It.IsAny<LogItem>()));
        }

        [Test]
        public void CallListLogData_GetLogItemsAsync_CallGetLogItemsAsync()
        {

            var result = _storage.ListLogData();

            _db.Verify(db => db.GetLogItemsAsync());

        }

        [Test]
        public void CallListLogData_GetLogItemsAsync_ReturnListOfLogItem()
        {
            var list = new List<LogItem>();

            _db.Setup(db => db.GetLogItemsAsync()).Returns(Task.FromResult(list));
            
            
            var result = _storage.ListLogData();

            Assert.That(result.Result, Is.EqualTo(list));


        }
    }
}
