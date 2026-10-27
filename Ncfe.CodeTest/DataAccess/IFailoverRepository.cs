using Ncfe.CodeTest.Data;
using System.Collections.Generic;

namespace Ncfe.CodeTest.DataAccess
{
    public interface IFailoverRepository
    {
        /// <summary>
        /// Get the recorded number of failed requests.
        /// </summary>
        /// <returns>/// A list of <see cref="FailoverEntry"/> items, each with the time the failure occurred.</returns>
        List<FailoverEntry> GetFailOverEntries();
    }
}