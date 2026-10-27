using Ncfe.CodeTest.Data;
using Ncfe.CodeTest.DataAccess;
using System;

namespace Ncfe.CodeTest.Services
{
    public class LearnerService : ILearnerService
    {
        private readonly IArchivedDataService archivedDataService;
        private readonly IFailoverService failoverService;
        private readonly ILearnerDataAccess learnerDataAccess;
        private readonly IFailoverLearnerDataAccess failoverLearnerDataAccess;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="archivedDataService">Used to retrieve learners from the archived data store.</param>
        /// <param name="failoverService">Used to decide whether requests should go to the failover data source.</param>
        /// <param name="learnerDataAccess">Used to load learners from the primary data source.</param>
        /// <param name="failoverLearnerDataAccess">Used to load learners from the failover data source.</param>
        public LearnerService(IArchivedDataService archivedDataService, IFailoverService failoverService,
            ILearnerDataAccess learnerDataAccess, IFailoverLearnerDataAccess failoverLearnerDataAccess)
        {
            this.archivedDataService = archivedDataService;
            this.failoverService = failoverService;
            this.learnerDataAccess = learnerDataAccess;
            this.failoverLearnerDataAccess = failoverLearnerDataAccess;
        }


        public Learner GetLearner(int learnerId, bool isLearnerArchived)
        {
            if (isLearnerArchived)
            {
                return archivedDataService.GetArchivedLearner(learnerId);
            }

            LearnerResponse learnerResponse = GetLearnerResponse(learnerId);

            if (learnerResponse.IsArchived)
            {
                return archivedDataService.GetArchivedLearner(learnerId);
            }

            return learnerResponse.Learner;
        }

        private LearnerResponse GetLearnerResponse(int learnerId)
        {
            LearnerResponse learnerResponse;

            if (failoverService.IsFailoverTriggered())
            {
                learnerResponse = failoverLearnerDataAccess.GetLearnerById(learnerId);
            }
            else
            {
                learnerResponse = learnerDataAccess.LoadLearner(learnerId);
            }

            if (learnerResponse == null)
            {
                throw new InvalidOperationException($"Failed to retrieve learner with ID {learnerId}.");
            }

            return learnerResponse;
        }

    }
}
