using SatisfactorySaveParser.Fields;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace SatisfactorySaveParser
{
    public abstract class SaveEntry
    {
        public string Str1 { get; set; }
        public string Str2 { get; set; }
        public string Str3 { get; set; }

        public SerializedFields DataFields { get; set; }

        public virtual void ParseData(int length, BinaryReader reader)
        {
            DataFields = SerializedFields.Parse(length, reader);
        }

        public override string ToString()
        {
            return Str1;
        }
    }
}
