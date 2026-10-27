using Ncfe.CodeTest.Data;

namespace Ncfe.CodeTest.DataAccess
{
    public interface ILearnerDataAccess
    {
        /// <summary>
        /// Loads a learner from the primary data source.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner to load.</param>
        /// <returns>
        /// A <see cref="LearnerResponse"/> containing the learner and whether the learner
        /// has been archived.
        /// </returns>
        LearnerResponse LoadLearner(int learnerId);
    }
}