using System.Diagnostics;
using System.Reflection;

namespace FM.Runtime.Helpers.General
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
            MethodBase callerID = null;

            for (int i = 0; i < 3; i++)
            {
                var stackFrame = new StackTrace().GetFrame(i);

                if (stackFrame != null)
                {
                    callerID = stackFrame.GetMethod();
                }
                else
                {
                    return callerID;
                }
            }

            return callerID;
        }
    }
}