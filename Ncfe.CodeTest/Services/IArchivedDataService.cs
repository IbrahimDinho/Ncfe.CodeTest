using Ncfe.CodeTest.Data;

namespace Ncfe.CodeTest.Services
{
    public interface IArchivedDataService
    {
        /// <summary>
        /// Gets a learner from the archived data store.
        /// </summary>
        /// <param name="learnerId">The unique identifier of the learner to retrieve.</param>
        /// <returns>The archived <see cref="Learner"/> with the given Id.</returns>
        Learner GetArchivedLearner(int learnerId);
    }
}