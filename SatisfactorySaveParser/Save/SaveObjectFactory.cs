using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NLog;

namespace SatisfactorySaveParser.Save
{
    public static class SaveObjectFactory
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private static readonly Dictionary<string, Type> objectTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(SaveObjectClassAttribute), false))
                .ToDictionary(t => ((SaveObjectClassAttribute)t.GetCustomAttribute(typeof(SaveObjectClassAttribute), false)).Type, t => t);
        private static readonly List<string> missingTypes = new List<string>();

        /// <summary>
        ///     Attempt to instantiate the matching class for this type, or return a dynamic object when missing
        /// </summary>
        /// <param name="kind"></param>
        /// <param name="className"></param>
        /// <returns></returns>
        public static SaveObject CreateFromClass(SaveObjectKind kind, string className)
        {
            if (!objectTypes.TryGetValue(className, out Type type))
            {
                if (!missingTypes.Contains(className))
                {
                    log.Warn($"Missing class for {kind} {className}");
                    missingTypes.Add(className);
                }
            }

            switch (kind)
            {
                case SaveObjectKind.Actor:
                    return Instantiate<SaveActor>(type, className);
                case SaveObjectKind.Component:
                    return Instantiate<SaveComponent>(type, className);
                default:
                    throw new NotImplementedException($"Unknown SaveObject kind {kind}");
            }
        }

        private static T Instantiate<T>(Type type, string className) where T : SaveObject, new()
        {
            if (type == null)
                return new T() { TypePath = className };

            if (!typeof(T).IsAssignableFrom(type))
                throw new InvalidOperationException($"Attempting to instantiate {typeof(T)} from a non {typeof(T)} derived class");

            var obj = (T)Activator.CreateInstance(type);
            obj.TypePath = className;

            return obj;
        }
    }
}
