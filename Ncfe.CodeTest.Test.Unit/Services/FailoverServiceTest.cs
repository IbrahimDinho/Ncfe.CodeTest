using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ncfe.CodeTest.Data;
using Ncfe.CodeTest.DataAccess;
using Ncfe.CodeTest.Services;
using Ncfe.CodeTest.Settings;
using System;
using System.Collections.Generic;

namespace Ncfe.CodeTest.Test.Unit.Services
{
    [TestClass]
    public class FailoverServiceTest
    {
        private Mock<IFailoverSettings> settingsMock;
        private Mock<IFailoverRepository> failoverRepositoryMock;
        private FailoverService failoverService;

        [TestInitialize]
        public void TestInitialize()
        {
            settingsMock = new Mock<IFailoverSettings>();
            failoverRepositoryMock = new Mock<IFailoverRepository>();

            failoverService = new FailoverService(
                settingsMock.Object,
                failoverRepositoryMock.Object);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsFailoverTriggered_FailoverDisabled_ReturnsFalse()
        {
            // Arrange
            settingsMock
                .Setup(s => s.IsFailoverModeEnabled())
                .Returns(false);

            // Act
            bool result = failoverService.IsFailoverTriggered();

            // Assert
            Assert.IsFalse(result);

            failoverRepositoryMock.Verify(
                r => r.GetFailOverEntries(),
                Times.Never);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsFailoverTriggered_NoFailedRequests_ReturnsFalse()
        {
            // Arrange
            settingsMock
                .Setup(s => s.IsFailoverModeEnabled())
                .Returns(true);

            failoverRepositoryMock
                .Setup(r => r.GetFailOverEntries())
                .Returns(new List<FailoverEntry>());

            // Act
            bool result = failoverService.IsFailoverTriggered();

            // Assert
            Assert.IsFalse(result);

            failoverRepositoryMock.Verify(
                r => r.GetFailOverEntries(),
                Times.Once);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsFailoverTriggered_FailuresEqualThreshold_ReturnsFalse()
        {
            // Arrange
            settingsMock
                .Setup(s => s.IsFailoverModeEnabled())
                .Returns(true);

            List<FailoverEntry> entries =
                CreateFailoverEntries(100, DateTime.Now.AddMinutes(-1));

            failoverRepositoryMock
                .Setup(r => r.GetFailOverEntries())
                .Returns(entries);

            // Act
            bool result = failoverService.IsFailoverTriggered();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsFailoverTriggered_FailuresExceedThreshold_ReturnsTrue()
        {
            // Arrange
            settingsMock
                .Setup(s => s.IsFailoverModeEnabled())
                .Returns(true);

            List<FailoverEntry> entries =
                CreateFailoverEntries(101, DateTime.Now.AddMinutes(-1));

            failoverRepositoryMock
                .Setup(r => r.GetFailOverEntries())
                .Returns(entries);

            // Act
            bool result = failoverService.IsFailoverTriggered();

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsFailoverTriggered_FailuresOutsideTimeWindow_ReturnsFalse()
        {
            // Arrange
            settingsMock
                .Setup(s => s.IsFailoverModeEnabled())
                .Returns(true);

            List<FailoverEntry> entries =
                CreateFailoverEntries(101, DateTime.Now.AddMinutes(-11));

            failoverRepositoryMock
                .Setup(r => r.GetFailOverEntries())
                .Returns(entries);

            // Act
            bool result = failoverService.IsFailoverTriggered();

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void IsFailoverTriggered_MixedFailuresOnlyCountsRecent_ReturnsFalse()
        {
            // Arrange
            settingsMock
                .Setup(s => s.IsFailoverModeEnabled())
                .Returns(true);

            List<FailoverEntry> entries =
                CreateFailoverEntries(100, DateTime.Now.AddMinutes(-1));

            entries.AddRange(
                CreateFailoverEntries(20, DateTime.Now.AddMinutes(-11)));

            failoverRepositoryMock
                .Setup(r => r.GetFailOverEntries())
                .Returns(entries);

            // Act
            bool result = failoverService.IsFailoverTriggered();

            // Assert
            Assert.IsFalse(result);
        }

        private List<FailoverEntry> CreateFailoverEntries(
            int count, DateTime dateTime)
        {
            List<FailoverEntry> entries = new List<FailoverEntry>();

            for (int i = 0; i < count; i++)
            {
                entries.Add(new FailoverEntry
                {
                    DateTime = dateTime
                });
            }

            return entries;
        }
    }
}
