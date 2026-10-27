namespace Ncfe.CodeTest.Settings
{
    public interface IFailoverSettings
    {
        /// <summary>
        /// Gets whether failover mode is enabled in the application configuration.
        /// </summary>
        /// <returns>if failover mode is enabled; if the setting is missing or can't be read as a boolean.</returns>
        bool IsFailoverModeEnabled();
    }
}