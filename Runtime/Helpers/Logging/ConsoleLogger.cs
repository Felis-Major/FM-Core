using FM.Runtime.Helpers.General;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace FM.Runtime.Helpers.Logging
{
    /// <summary>
    /// Extended console logging
    /// </summary>
    public static class ConsoleLogger
    {
        /// <summary>
        /// Type of log
        /// </summary>
        public enum LogType
        {
            /// <summary>
            /// Regular log
            /// </summary>
            Normal,

            /// <summary>
            /// Important log (will display a bigger text)
            /// </summary>
            Important,

            /// <summary>
            /// Discrete log (will display a smaller text)
            /// </summary>
            Discrete
        }

        /// <summary>
        /// Outputs a message to the console
        /// </summary>
        /// <param name="message">Message</param>
        public static void Log(object message)
        {
            Log(message, Color.white, LogType.Normal);
        }

        /// <summary>
        /// Ouputs a message to the console
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="color">Color of the message</param>
        public static void Log(object message, Color32 color)
        {
            Log(message, color, LogType.Normal);
        }

        /// <summary>
        /// Ouputs a message to the console
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="logType">Type of the log</param>
        public static void Log(object message, LogType logType)
        {
            Log(message, Color.white, logType);
        }

        /// <summary>
        /// Ouputs a message to the console
        /// </summary>
        /// <param name="message">Message</param>
        /// <param name="color">Color of the message</param>
        /// <param name="logType">Type of the log</param>
        public static void Log(object message, Color32 color, LogType logType)
        {
            // Find caller infos
            var callerInfo = ProfilingHelpers.GetCallerInfo();
            var callerType = callerInfo.DeclaringType.Name;
            var callerName = callerInfo.Name;

            // Add log option
            var messagePrefix = "";
            var messageSuffix = "";

            switch (logType)
            {
                case LogType.Important:
                    messagePrefix = "<b>";
                    messageSuffix= "</b>";
                    break;
                case LogType.Discrete:
                    messagePrefix = "<i>";
                    messageSuffix = "</i>";
                    break;
            }

            // Output log to the console
            string log = $"{messagePrefix}<color=#{color.ToHex()}><b>[{callerType}:{callerName}]</b> {message}</color>{messageSuffix}";
            Debug.Log(log);
        }
    }
}
