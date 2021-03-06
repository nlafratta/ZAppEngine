﻿using System;
using System.Linq;
namespace Zirpl.AppEngine.Logging
{
    /// <summary>
    /// Provides instances of ILog objects for logging
    /// </summary>
    public static class LogManager
    {
        private static ILogFactory _logFactory;

#if !NET35CLIENT && !NET40CLIENT && !SILVERLIGHT
        /// <summary>
        /// Initializes the LogManager
        /// </summary>
        static LogManager()
        {
            _logFactory = new Zirpl.AppEngine.Logging.Common.CommonLogFactory();
        }
#endif

        /// <summary>
        /// Gets the default Log
        /// </summary>
        /// <returns></returns>
        public static ILog GetLog()
        {
            return GetLog(String.Empty);
        }

        /// <summary>
        /// Gets the ILog of the specified name
        /// </summary>
        /// <param name="name">Name of the log to get</param>
        /// <returns>The ILog</returns>
        public static ILog GetLog(String name)
        {
            return _logFactory == null ? new NullLog() : _logFactory.GetLog(name);
        }

        /// <summary>
        /// Gets the log for the specified type
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The log</returns>
        public static ILog GetLog(Type type)
        {
            if (type == null)
                return GetLog();
            else
                return GetLog(type.ToGenericTypeString());
        }

        /// <summary>
        /// Gets the log for the specified type. This is an extension method.
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <param name="obj">The object of the type consuming the log</param>
        /// <returns>The log.</returns>
        public static ILog GetLog<TLogConsumer>(this TLogConsumer obj)
        {
            return GetLog(typeof(TLogConsumer));
        }

        /// <summary>
        /// Gets the log for the specified type. This is an extension method.
        /// </summary>
        /// <typeparam name="TLogConsumer">The type of the log consumer.</typeparam>
        /// <returns>The log.</returns>
        public static ILog GetLog<TLogConsumer>()
        {
            return GetLog(typeof(TLogConsumer));
        }

        private static string ToGenericTypeString(this Type t)
        {
            if (!t.IsGenericType)
                return t.FullName;
            string genericTypeName = t.GetGenericTypeDefinition().FullName;
            genericTypeName = genericTypeName.Substring(0,
                                                        genericTypeName.IndexOf('`'));
            string genericArgs = String.Join(",",
                                             t.GetGenericArguments()
                                              .Select(ta => ToGenericTypeString(ta)).ToArray());
            return genericTypeName + "<" + genericArgs + ">";
        }
    }
}
