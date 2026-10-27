using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ncfe.CodeTest.Data;
using Ncfe.CodeTest.DataAccess;
using Ncfe.CodeTest.Services;
using System;

namespace Ncfe.CodeTest.Test.Unit.Services
{
    [TestClass]
    public class LearnerServiceTest
    {
        private Mock<IArchivedDataService> archivedDataServiceMock;
        private Mock<IFailoverService> failoverServiceMock;
        private Mock<ILearnerDataAccess> learnerDataAccessMock;
        private LearnerService learnerService;
        private Mock<IFailoverLearnerDataAccess> failoverLearnerDataAccessMock;

        [TestInitialize]
        public void TestInitialize()
        {
            archivedDataServiceMock = new Mock<IArchivedDataService>();
            failoverServiceMock = new Mock<IFailoverService>();
            learnerDataAccessMock = new Mock<ILearnerDataAccess>();
            failoverLearnerDataAccessMock = new Mock<IFailoverLearnerDataAccess>();

            learnerService = new LearnerService(
                archivedDataServiceMock.Object,
                failoverServiceMock.Object,
                learnerDataAccessMock.Object,
                failoverLearnerDataAccessMock.Object);
        }


        [TestMethod]
        [TestCategory("Unit")]
        public void GetLearner_LearnerArchived_ReturnsArchivedLearner()
        {
            // Arrange
            int learnerId = 123;
            Learner expectedLearner = new Learner();

            archivedDataServiceMock
                .Setup(a => a.GetArchivedLearner(learnerId))
                .Returns(expectedLearner);

            // Act
            Learner result = learnerService.GetLearner(learnerId, true);

            // Assert
            Assert.AreSame(expectedLearner, result);

            archivedDataServiceMock.Verify(
                a => a.GetArchivedLearner(learnerId),
                Times.Once);

            learnerDataAccessMock.Verify(
                a => a.LoadLearner(It.IsAny<int>()),
                Times.Never);

            failoverServiceMock.Verify(
                f => f.IsFailoverTriggered(),
                Times.Never);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetLearner_FailoverNotTriggered_ReturnsLearnerFromPrimaryDataSource()
        {
            // Arrange
            int learnerId = 123;
            Learner expectedLearner = new Learner();

            LearnerResponse response = new LearnerResponse
            {
                Learner = expectedLearner,
                IsArchived = false
            };

            failoverServiceMock
                .Setup(f => f.IsFailoverTriggered())
                .Returns(false);

            learnerDataAccessMock
                .Setup(a => a.LoadLearner(learnerId))
                .Returns(response);

            // Act
            Learner result = learnerService.GetLearner(learnerId, false);

            // Assert
            Assert.AreSame(expectedLearner, result);

            failoverServiceMock.Verify(
                f => f.IsFailoverTriggered(),
                Times.Once);

            learnerDataAccessMock.Verify(
                a => a.LoadLearner(learnerId),
                Times.Once);

            archivedDataServiceMock.Verify(
                a => a.GetArchivedLearner(It.IsAny<int>()),
                Times.Never);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetLearner_ResponseIsArchived_ReturnsArchivedLearner()
        {
            // Arrange
            int learnerId = 123;
            Learner expectedLearner = new Learner();

            LearnerResponse response = new LearnerResponse
            {
                IsArchived = true
            };

            failoverServiceMock
                .Setup(f => f.IsFailoverTriggered())
                .Returns(false);

            learnerDataAccessMock
                .Setup(a => a.LoadLearner(learnerId))
                .Returns(response);

            archivedDataServiceMock
                .Setup(a => a.GetArchivedLearner(learnerId))
                .Returns(expectedLearner);

            // Act
            Learner result = learnerService.GetLearner(learnerId, false);

            // Assert
            Assert.AreSame(expectedLearner, result);

            learnerDataAccessMock.Verify(
                a => a.LoadLearner(learnerId),
                Times.Once);

            archivedDataServiceMock.Verify(
                a => a.GetArchivedLearner(learnerId),
                Times.Once);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetLearner_NullLearnerResponse_ThrowsInvalidOperationException()
        {
            // Arrange
            int learnerId = 123;

            failoverServiceMock
                .Setup(f => f.IsFailoverTriggered())
                .Returns(false);

            learnerDataAccessMock
                .Setup(a => a.LoadLearner(learnerId))
                .Returns((LearnerResponse)null);

            // Act
            InvalidOperationException exception =
                Assert.ThrowsException<InvalidOperationException>(() =>
                    learnerService.GetLearner(learnerId, false));

            // Assert
            Assert.AreEqual(
                $"Failed to retrieve learner with ID {learnerId}.",
                exception.Message);

            learnerDataAccessMock.Verify(
                a => a.LoadLearner(learnerId),
                Times.Once);

            archivedDataServiceMock.Verify(
                a => a.GetArchivedLearner(It.IsAny<int>()),
                Times.Never);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetLearner_FailoverTriggered_ReturnsLearnerFromFailoverDataSource()
        {
            // Arrange
            int learnerId = 123;
            Learner expectedLearner = new Learner();

            LearnerResponse response = new LearnerResponse
            {
                Learner = expectedLearner,
                IsArchived = false
            };

            failoverServiceMock
                .Setup(f => f.IsFailoverTriggered())
                .Returns(true);

            failoverLearnerDataAccessMock
                .Setup(f => f.GetLearnerById(learnerId))
                .Returns(response);

            // Act
            Learner result = learnerService.GetLearner(learnerId, false);

            // Assert
            Assert.AreSame(expectedLearner, result);

            failoverLearnerDataAccessMock.Verify(
                f => f.GetLearnerById(learnerId),
                Times.Once);

            learnerDataAccessMock.Verify(
                l => l.LoadLearner(It.IsAny<int>()),
                Times.Never);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetLearner_FailoverReturnsNull_ThrowsInvalidOperationException()
        {
            // Arrange
            int learnerId = 123;

            failoverServiceMock
                .Setup(f => f.IsFailoverTriggered())
                .Returns(true);

            failoverLearnerDataAccessMock
                .Setup(f => f.GetLearnerById(learnerId))
                .Returns((LearnerResponse)null);

            // Act
            InvalidOperationException exception =
                Assert.ThrowsException<InvalidOperationException>(() =>
                    learnerService.GetLearner(learnerId, false));

            // Assert
            Assert.AreEqual(
                $"Failed to retrieve learner with ID {learnerId}.",
                exception.Message);

            failoverLearnerDataAccessMock.Verify(
                f => f.GetLearnerById(learnerId),
                Times.Once);

            learnerDataAccessMock.Verify(
                l => l.LoadLearner(It.IsAny<int>()),
                Times.Never);
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetLearner_FailoverResponseIsArchived_ReturnsArchivedLearner()
        {
            // Arrange
            int learnerId = 123;
            Learner expectedLearner = new Learner();

            LearnerResponse response = new LearnerResponse
            {
                IsArchived = true
            };

            failoverServiceMock
                .Setup(f => f.IsFailoverTriggered())
                .Returns(true);

            failoverLearnerDataAccessMock
                .Setup(f => f.GetLearnerById(learnerId))
                .Returns(response);

            archivedDataServiceMock
                .Setup(a => a.GetArchivedLearner(learnerId))
                .Returns(expectedLearner);

            // Act
            Learner result = learnerService.GetLearner(learnerId, false);

            // Assert
            Assert.AreSame(expectedLearner, result);

            failoverLearnerDataAccessMock.Verify(
                f => f.GetLearnerById(learnerId),
                Times.Once);

            archivedDataServiceMock.Verify(
                a => a.GetArchivedLearner(learnerId),
                Times.Once);

            learnerDataAccessMock.Verify(
                l => l.LoadLearner(It.IsAny<int>()),
                Times.Never);
        }

    }
}
