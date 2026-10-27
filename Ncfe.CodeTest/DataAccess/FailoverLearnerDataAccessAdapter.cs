using Ncfe.CodeTest.Data;

namespace Ncfe.CodeTest.DataAccess
{
    public class FailoverLearnerDataAccessAdapter : IFailoverLearnerDataAccess
    {
        /// <inheritdoc />
        public LearnerResponse GetLearnerById(int learnerId)
        {
            return FailoverLearnerDataAccess.GetLearnerById(learnerId);
        }
    }
}
