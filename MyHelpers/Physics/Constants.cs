using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pavlo.MyHelpers.Physics
{
    /// <summary>
    /// Some physics constants. All consatnts in IS.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// Vacuum permeability
        /// </summary>
        public static readonly double mu0 = 4 * Math.PI * 1e-7;
        /// <summary>
        /// Speed of light
        /// </summary>
        public static readonly double c = 299792458;
        /// <summary>
        /// Vacuum permittivity
        /// </summary>
        public static readonly double eps0=1/(Constants.mu0*Constants.c*Constants.c);
    }
}
