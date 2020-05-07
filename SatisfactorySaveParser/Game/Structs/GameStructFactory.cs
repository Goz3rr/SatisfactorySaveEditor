using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using NLog;

namespace SatisfactorySaveParser.Game.Structs
{
    public static class GameStructFactory
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private static readonly HashSet<string> missingTypes = new HashSet<string>();
        private static readonly Dictionary<string, Type> structTypes = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsDefined(typeof(GameStructAttribute), false))
                .SelectMany(t => t.GetCustomAttributes<GameStructAttribute>(false).Select(attr => new { Attribute = attr, Type = t }))
                .ToDictionary(x => x.Attribute.StructName, x => x.Type);

        public static GameStruct CreateFromType(string structName)
        {
            if (!structTypes.TryGetValue(structName, out Type type))
            {
                if (!missingTypes.Contains(structName))
                {
                    log.Warn($"Missing class for struct {structName}");
                    missingTypes.Add(structName);
                }

                return new DynamicGameStruct(structName);
            }

            var obj = (GameStruct)Activator.CreateInstance(type);

            return obj;
        }
    }
}
