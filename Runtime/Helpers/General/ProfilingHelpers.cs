using System.Diagnostics;
using System.Reflection;

namespace Daniell.Runtime.Helpers.General
{
    /// <summary>
    /// Helpers related to profiling
    /// </summary>
    public static class ProfilingHelpers
    {
        /// <summary>
        /// Get info on the caller of the current method
        /// </summary>
        /// <returns>Info about the caller of this method</returns>
        public static MethodBase GetCallerInfo()
        {
            var stackTrace = new StackTrace().GetFrame(3);
            return stackTrace.GetMethod();
        }
    }
}