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
    public class SettingsServiceTest
    {
        private ISettingsService _settings;
        private Mock<IPreferences> _preferences;
        private double _scale = .1;
        private double _defaultScale = .2;

        [SetUp]
        public void Setup()
        {
            _preferences = new Mock<IPreferences>();
            _settings = new SettingsService(_preferences.Object);
        }

        [Test]
        public void GetVoltScale_ReturnValue()
        {
            _preferences.Setup(p => p.Get<double>("VoltScale", It.IsAny<double>(), null)).Returns(_scale);

            var value = _settings.VoltScale;
            Assert.That(value, Is.EqualTo(_scale));
        }

        [Test]
        public void SetVoltScale_PreferencesSetCalled()
        {
            _settings.VoltScale = _defaultScale;
            _preferences.Verify(p => p.Set(It.IsAny<string>(), _defaultScale, null), Times.Once());
        }

        [Test]
        public void GetTmpScale_ReturnValue()
        {
            _preferences.Setup(p => p.Get<double>("TmpScale", It.IsAny<double>(), null)).Returns(_scale);

            var value = _settings.TmpScale;
            Assert.That(value, Is.EqualTo(_scale));
        }

        [Test]
        public void SetTmpScale_PreferencesSetCalled()
        {
            
            _settings.VoltScale = _defaultScale;
            _preferences.Verify(p => p.Set(It.IsAny<string>(), _defaultScale, null), Times.Once());
        }



    }
}
