using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BleTempMonitor.Extensions
{
    public static class General
    {
        public static string GetTempAlias(this Guid id)
        {
            var ids = id.ToString().Split('-');
            return ids[ids.Length - 1];
        }
    }
}
