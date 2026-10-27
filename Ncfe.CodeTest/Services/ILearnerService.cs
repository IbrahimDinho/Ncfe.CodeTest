using Ncfe.CodeTest.Data;

namespace Ncfe.CodeTest.Services
{
    public interface ILearnerService
    {
        /// <summary>
        /// Retrieves a learner from the archived, primary, or failover data source
        /// depending on the learner's archived status and the current failover state.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner to retrieve.</param>
        /// <param name="isLearnerArchived">Indicates whether the learner should be retrieved directly from the archived data source.</param>
        /// <returns>The learner retrieved from the appropriate data source.</returns>
        Learner GetLearner(int learnerId, bool isLearnerArchived);
    }
}
