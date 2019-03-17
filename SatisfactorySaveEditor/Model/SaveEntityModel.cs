using System;
using SatisfactorySaveParser;

namespace SatisfactorySaveEditor.Model
{
    public class SaveEntityModel : SaveObjectModel
    {
        private int int4;
        private int int6;
        private string unknown5;
        private string parentObjectRoot;
        private string parentObjectName;

        public int Int4
        { 
            get => int4;
            set { Set(() => Int4, ref int4, value); }
        }

        public int Int6
        {
            get => int6;
            set { Set(() => Int6, ref int6, value); }
        }

        public string Unknown5
        {
            get => unknown5;
            set { Set(() => Unknown5, ref unknown5, value); }
        }

        public string ParentObjectRoot
        {
            get => parentObjectRoot;
            set { Set(() => ParentObjectRoot, ref parentObjectRoot, value); }
        }

        public string ParentObjectName
        {
            get => parentObjectName;
            set { Set(() => ParentObjectName, ref parentObjectName, value); }
        }

        public SaveEntityModel(string title, SerializedFields fields, string rootObject, SaveEntity ent) : base(title, fields, rootObject)
        {
            Int4 = ent.Int4;
            Int6 = ent.Int6;
            ParentObjectRoot = ent.ParentObjectRoot;
            ParentObjectName = ent.ParentObjectName;
            Unknown5 = BitConverter.ToString(ent.Unknown5).Replace("-","");
        }
    }
}
