using Ncfe.CodeTest.Data;
using Ncfe.CodeTest.DataAccess;
using Ncfe.CodeTest.Settings;
using System;

namespace Ncfe.CodeTest.Services
{
    public class FailoverService : IFailoverService
    {
        /// <summary>
        /// The number of failed requests within the window above which failover is triggered.
        /// </summary>
        private const int FAILURE_THRESHOLD = 100;

        /// <summary>
        /// How far back, in minutes, failed requests are counted.
        /// </summary>
        private const int FAIL_OVER_WINDOW_MINUTES = 10;


        private readonly IFailoverSettings settings;
        private readonly IFailoverRepository failoverRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="settings">Provides the failover configuration</param>
        /// <param name="failoverRepository">Provides the recorded failover entries used to count recent failures</param>
        public FailoverService(IFailoverSettings settings, IFailoverRepository failoverRepository)
        {
            this.settings = settings;
            this.failoverRepository = failoverRepository;
        }

        public bool IsFailoverTriggered()
        {
            if (settings.IsFailoverModeEnabled() == false)
            {
                return false;
            }

            DateTime cutoff = DateTime.Now.AddMinutes(-FAIL_OVER_WINDOW_MINUTES);
            int failedRequests = 0;

            foreach (FailoverEntry failoverEntry in failoverRepository.GetFailOverEntries())
            {
                if (failoverEntry.DateTime > cutoff)
                {
                    failedRequests++;
                }
            }

            return failedRequests > FAILURE_THRESHOLD;
        }
    }
}
