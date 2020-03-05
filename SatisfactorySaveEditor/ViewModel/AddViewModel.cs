using System;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using SatisfactorySaveEditor.Model;
using SatisfactorySaveEditor.Util;
using SatisfactorySaveParser.PropertyTypes;

namespace SatisfactorySaveEditor.ViewModel
{
    public class AddViewModel : ViewModelBase
    {
        public enum AddTypeEnum
        {
            Array,
            Bool,
            Byte,
            Enum,
            Float,
            Int,
            Map,
            Name,
            Object,
            String,
            Struct,
            Text,
            Interface,
            Int64
        }

        private AddTypeEnum type = AddTypeEnum.Array;
        private AddTypeEnum arrayType = AddTypeEnum.Bool;
        private string name;

        public SaveObjectModel ObjectModel { get; set; }
        public RelayCommand<Window> OkCommand { get; }
        public RelayCommand<Window> CancelCommand { get; }

        public AddViewModel()
        {
            OkCommand = new RelayCommand<Window>(Ok);
            CancelCommand = new RelayCommand<Window>(Cancel);
        }

        public string Name
        {
            get => name;
            set
            {
                Set(() => Name, ref name, value);
                RaisePropertyChanged(() => CanConfirm);
            }
        }
        public AddTypeEnum Type
        {
            get => type;
            set
            {
                Set(() => Type, ref type, value);
                RaisePropertyChanged(() => IsArray);
                RaisePropertyChanged(() => CanConfirm);
            }
        }
        public AddTypeEnum ArrayType
        {
            get => arrayType;
            set
            {
                Set(() => ArrayType, ref arrayType, value);
                RaisePropertyChanged(() => CanConfirm);
            }
        }

        public Visibility IsArray => type == AddTypeEnum.Array ? Visibility.Visible : Visibility.Collapsed;

        public bool CanConfirm
        {
            get
            {
                if (type != AddTypeEnum.Array) return !string.IsNullOrWhiteSpace(Name);
                
                return arrayType != AddTypeEnum.Array && !string.IsNullOrWhiteSpace(Name);
            }
        }

        public static SerializedProperty CreateProperty(AddTypeEnum type, string name)
        {
            switch (type)
            {
                case AddTypeEnum.Array:
                    return new ArrayProperty(name);
                case AddTypeEnum.Bool:
                    return new BoolProperty(name);
                case AddTypeEnum.Byte:
                    return new ByteProperty(name);
                case AddTypeEnum.Enum:
                    return new EnumProperty(name);
                case AddTypeEnum.Float:
                    return new FloatProperty(name);
                case AddTypeEnum.Int:
                    return  new IntProperty(name);
                case AddTypeEnum.Map:
                    return new MapProperty(name);
                case AddTypeEnum.Name:
                    return new NameProperty(name);
                case AddTypeEnum.Object:
                    return new ObjectProperty(name);
                case AddTypeEnum.String:
                    return new StrProperty(name);
                case AddTypeEnum.Struct:
                    return new StructProperty(name);
                case AddTypeEnum.Text:
                    return new TextProperty(name);
                case AddTypeEnum.Interface:
                    return new InterfaceProperty(name);
                case AddTypeEnum.Int64:
                    return new Int64Property(name);
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static string FromAddTypeEnum(AddTypeEnum type)
        {
            switch (type)
            {
                case AddTypeEnum.Array:
                    return "ArrayProperty";
                case AddTypeEnum.Bool:
                    return "BoolProperty";
                case AddTypeEnum.Byte:
                    return "ByteProperty";
                case AddTypeEnum.Enum:
                    return "EnumProperty";
                case AddTypeEnum.Float:
                    return "FloatProperty";
                case AddTypeEnum.Int:
                    return "IntProperty";
                case AddTypeEnum.Map:
                    return "MapProperty";
                case AddTypeEnum.Name:
                    return "NameProperty";
                case AddTypeEnum.Object:
                    return "ObjectProperty";
                case AddTypeEnum.String:
                    return "StrProperty";
                case AddTypeEnum.Struct:
                    return "StructProperty";
                case AddTypeEnum.Text:
                    return "TextProperty";
                case AddTypeEnum.Interface:
                    return "InterfaceProperty";
                case AddTypeEnum.Int64:
                    return "Int64Property";
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        public static AddTypeEnum FromStringType(string stringType)
        {
            switch (stringType)
            {
                case "ArrayProperty":
                    return AddTypeEnum.Array;
                case "BoolProperty":
                    return AddTypeEnum.Bool;
                case "ByteProperty":
                    return AddTypeEnum.Byte;
                case "EnumProperty":
                    return AddTypeEnum.Enum;
                case "FloatProperty":
                    return AddTypeEnum.Float;
                case "IntProperty":
                    return AddTypeEnum.Int;
                case "MapProperty":
                    return AddTypeEnum.Map;
                case "NameProperty":
                    return AddTypeEnum.Name;
                case "ObjectProperty":
                    return AddTypeEnum.Object;
                case "StrProperty":
                    return AddTypeEnum.String;
                case "StructProperty":
                    return AddTypeEnum.Struct;
                case "TextProperty":
                    return AddTypeEnum.Text;
                case "InterfaceProperty":
                    return AddTypeEnum.Interface;
                case "Int64Property":
                    return AddTypeEnum.Int64;
                default:
                    throw new ArgumentOutOfRangeException(nameof(stringType), stringType, null);
            }
        }

        private void Cancel(Window obj)
        {
            obj.Close();
        }

        private void Ok(Window obj)
        {
            var property = CreateProperty(type, name);
            if (type == AddTypeEnum.Array) ((ArrayProperty) property).Type = FromAddTypeEnum(arrayType);
            ObjectModel.Fields.Add(PropertyViewModelMapper.Convert(property));

            obj.Close();
        }
    }
}
