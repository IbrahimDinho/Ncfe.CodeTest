using System;
using System.Configuration;

namespace Ncfe.CodeTest.Settings
{
    public class FailoverSettings : IFailoverSettings
    {
        public bool IsFailoverModeEnabled()
        {
            string value = ConfigurationManager.AppSettings["IsFailoverModeEnabled"];

            if (value == null)
            {
                throw new ConfigurationErrorsException("IsFailoverModeEnabled is missing from AppSettings.");
            }

            return string.Equals(value, "true", StringComparison.OrdinalIgnoreCase);

        }
    }
}
