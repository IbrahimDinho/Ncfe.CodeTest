namespace Ncfe.CodeTest.Services
{
    public interface IFailoverService
    {
        /// <summary>
        /// Determines whether requests should be routed to the failover data source.
        /// Failover is triggered when failover mode is enabled and the number of failed
        /// requests in the recent time window exceeds the failure threshold.
        /// </summary>
        /// <returns>
        /// boolean if failover mode is enabled and recent failures exceed the threshold;
        /// </returns>
        bool IsFailoverTriggered();
    }
}
