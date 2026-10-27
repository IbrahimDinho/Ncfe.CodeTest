using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ncfe.CodeTest.Data;
using Ncfe.CodeTest.Services;

namespace Ncfe.CodeTest.Test.Unit.Services
{
    [TestClass]
    public class ArchivedDataServiceTest
    {
        private ArchivedDataService archivedDataService;

        [TestInitialize]
        public void TestInitialize()
        {
            archivedDataService = new ArchivedDataService();
        }

        [TestMethod]
        [TestCategory("Unit")]
        public void GetArchivedLearner_ValidLearnerId_ReturnsLearner()
        {
            // Arrange
            int learnerId = 123;

            // Act
            Learner result = archivedDataService.GetArchivedLearner(learnerId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(Learner));
        }
    }
}
