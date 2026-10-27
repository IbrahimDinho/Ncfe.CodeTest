using Ncfe.CodeTest.Data;
using System.Collections.Generic;

namespace Ncfe.CodeTest.DataAccess
{
    public class FailoverRepository : IFailoverRepository
    {
        public List<FailoverEntry> GetFailOverEntries()
        {
            // return all from fail entries from database
            return new List<FailoverEntry>();
        }
    }
}
