using System;
using System.Reflection;
using NLog;

namespace SatisfactorySaveParser.Save.Properties
{
    public abstract class SerializedProperty
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public string PropertyName { get; set; }
        public abstract string PropertyType { get; }
        public int Index { get; set; }

        public abstract Type BackingType { get; }
        public abstract object BackingObject { get; }

        public abstract int SerializedLength { get; }

        public virtual void AssignToProperty(SaveObject saveObject, PropertyInfo info)
        {
            if (info.PropertyType != BackingType)
            {
                log.Warn($"Attempted to assign {PropertyType} {PropertyName} to incompatible backing type {info.PropertyType.Name}");
                saveObject.DynamicProperties.Add(this);
            }

            info.SetValue(saveObject, BackingObject);

            //saveObject.DynamicProperties.Add(this);
        }
    }
}
