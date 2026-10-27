using Ncfe.CodeTest.Data;

namespace Ncfe.CodeTest.DataAccess
{
    public class LearnerDataAccess : ILearnerDataAccess
    {
        public LearnerResponse LoadLearner(int learnerId)
        {
            // rettrieve learner from 3rd party webservice
            return new LearnerResponse();
        }
    }
}
