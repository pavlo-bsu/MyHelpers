using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pavlo.MyHelpers
{
    public class Misc
    {
        /// <summary>
        /// Return the directory of the current executable file
        /// </summary>
        /// <returns></returns>
        public static string GetDirectoryOfCurrentExecutableFile()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
    }
}
