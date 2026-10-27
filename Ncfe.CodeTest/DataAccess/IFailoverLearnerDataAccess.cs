using Ncfe.CodeTest.Data;

namespace Ncfe.CodeTest.DataAccess
{
    public interface IFailoverLearnerDataAccess
    {
        /// <summary>
        /// Gets a learner from the failover data source.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner to retrieve.</param>
        /// <returns>
        /// A <see cref="LearnerResponse"/> containing the learner and whether the learner is archived.
        /// </returns>
        LearnerResponse GetLearnerById(int learnerId);
    }
}