using System;
using System.Diagnostics;
using NLog;
using System.IO;
using System.Linq;
using NLog.Fluent;
using SatisfactorySaveParser.Metadata;
using SatisfactorySaveParser.PropertyTypes.Structs;

namespace SatisfactorySaveParser
{
    /// <summary>
    ///     Class representing a single saved object in a Satisfactory save
    ///     Engine class: FObjectBaseSaveHeader
    /// </summary>
    public abstract class SaveObject
    {
        /// <summary>
        ///     Forward slash separated path of the script/prefab of this object.
        ///     Can be an empty string.
        /// </summary>
        public string TypePath { get; set; }

        /// <summary>
        ///     Root object (?) of this object
        ///     Often some form of "Persistent_Level", can be an empty string
        /// </summary>
        public string RootObject { get; set; }

        /// <summary>
        ///     Unique (?) name of this object
        /// </summary>
        public string InstanceName { get; set; }

        /// <summary>
        ///     Main serialized data of the object
        /// </summary>
        public SerializedFields DataFields { get; set; }

        /// <summary>
        ///     meta data from the trailing bytes
        /// </summary>
        public SaveObjectMetaData MetaData { get; set; }
        
        
        /// <summary>
        /// the level name this object belongs to (introduced in U6)
        /// </summary>
        public string LevelName { get; set; }

        public SaveObject(string typePath, string rootObject, string instanceName)
        {
            TypePath = typePath;
            RootObject = rootObject;
            InstanceName = instanceName;
            MetaData = new SaveObjectMetaData();
        }

        protected SaveObject(BinaryReader reader)
        {
            TypePath = reader.ReadLengthPrefixedString();
            RootObject = reader.ReadLengthPrefixedString();
            InstanceName = reader.ReadLengthPrefixedString();
        }

        public virtual void SerializeHeader(BinaryWriter writer)
        {
            writer.WriteLengthPrefixedString(TypePath);
            writer.WriteLengthPrefixedString(RootObject);
            writer.WriteLengthPrefixedString(InstanceName);
        }

        public virtual void SerializeData(BinaryWriter writer, int buildVersion)
        {
            DataFields.Serialize(writer, buildVersion);
        }

        public virtual void ParseData(int length, BinaryReader reader, int buildVersion)
        {
            DataFields = SerializedFields.Parse(length, reader, buildVersion);

            // safely parse trailing data, there are some additional properties for some objects
            if (DataFields.TrailingData != null && DataFields.TrailingData.Length > 0)
            {

                var dataCopy = DataFields.TrailingData.ToArray();
                using (var trailingReader = new BinaryReader(new MemoryStream(dataCopy)))
                {
                    ParseTrailingAdditionalData(trailingReader);
                }
            }
        }
    
        private void ParseTrailingAdditionalData(BinaryReader reader)
        {
            switch (TypePath)
            {
                case "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk1/Build_ConveyorBeltMk1.Build_ConveyorBeltMk1_C":
                case "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk2/Build_ConveyorBeltMk2.Build_ConveyorBeltMk2_C":
                case "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk3/Build_ConveyorBeltMk3.Build_ConveyorBeltMk3_C":
                case "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk4/Build_ConveyorBeltMk4.Build_ConveyorBeltMk4_C":
                case "/Game/FactoryGame/Buildable/Factory/ConveyorBeltMk5/Build_ConveyorBeltMk5.Build_ConveyorBeltMk5_C":
                case "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk1/Build_ConveyorLiftMk1.Build_ConveyorLiftMk1_C":
                case "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk2/Build_ConveyorLiftMk2.Build_ConveyorLiftMk2_C":
                case "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk3/Build_ConveyorLiftMk3.Build_ConveyorLiftMk3_C":
                case "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk4/Build_ConveyorLiftMk4.Build_ConveyorLiftMk4_C":
                case "/Game/FactoryGame/Buildable/Factory/ConveyorLiftMk5/Build_ConveyorLiftMk5.Build_ConveyorLiftMk5_C":
                    MetaData = new ConveyorBeltLiftMetaData();
                    MetaData.ParseData(reader);
                    break;
                case "/Game/FactoryGame/Buildable/Factory/DroneStation/BP_DroneTransport.BP_DroneTransport_C":
                    var droneStuff = reader.ReadInt32();
                    break;
                case "/Game/FactoryGame/-Shared/Blueprint/BP_GameMode.BP_GameMode_C":
                    var mode = reader.ReadInt32();
                    break;
                case "/Game/FactoryGame/-Shared/Blueprint/BP_GameState.BP_GameState_C":
                    // player state children
                    var numPlayerStates = reader.ReadInt32();
                    for (int i = 0; i < numPlayerStates; i++)
                    {
                        var lvlname = reader.ReadLengthPrefixedString();
                        var pthname = reader.ReadLengthPrefixedString();
                    }

                    break;
                case "/Game/FactoryGame/Character/Player/BP_PlayerState.BP_PlayerState_C":
                    var playerType = reader.ReadByte();
                    switch (playerType)
                    {
                        case 248:
                            var eos = reader.ReadLengthPrefixedString();
                            var eosId = reader.ReadLengthPrefixedString();
                            break;
                        case 25:
                            break;
                        case 8:
                            var platformId = reader.ReadLengthPrefixedString();
                            break;
                        case 3:
                            break;
                    }

                    break;
                case "/Game/FactoryGame/-Shared/Blueprint/BP_CircuitSubsystem.BP_CircuitSubsystem_C":
                    var numCircuitSubsystems = reader.ReadInt32();
                    for (int i = 0; i < numCircuitSubsystems; i++)
                    {
                        var circuitId = reader.ReadInt32();
                        var lvlname = reader.ReadLengthPrefixedString();
                        var pthname = reader.ReadLengthPrefixedString();
                    }

                    break;
                case "/Game/FactoryGame/Buildable/Factory/PowerLine/Build_PowerLine.Build_PowerLine_C":
                case "/Game/FactoryGame/Events/Christmas/Buildings/PowerLineLights/Build_XmassLightsLine.Build_XmassLightsLine_C":

                    var lvlnameSource = reader.ReadLengthPrefixedString();
                    var instanceNameSource = reader.ReadLengthPrefixedString();
                    var lvlNameTarget = reader.ReadLengthPrefixedString();
                    var instanceNameTarget = reader.ReadLengthPrefixedString();

                    // can have 6 floats
                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        reader.ReadSingle();
                        reader.ReadSingle();
                        reader.ReadSingle();
                        reader.ReadSingle();
                        reader.ReadSingle();
                        reader.ReadSingle();
                    }

                    break;
                case "/Game/FactoryGame/Buildable/Vehicle/Train/Locomotive/BP_Locomotive.BP_Locomotive_C":
                case "/Game/FactoryGame/Buildable/Vehicle/Train/Wagon/BP_FreightWagon.BP_FreightWagon_C":
                    var padding = reader.ReadInt32();
                    while (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        var wagonLevelName = reader.ReadLengthPrefixedString();
                        var wagonInstanceName = reader.ReadLengthPrefixedString();
                    }

                    break;
                case "/Game/FactoryGame/Buildable/Vehicle/Tractor/BP_Tractor.BP_Tractor_C":
                case "/Game/FactoryGame/Buildable/Vehicle/Truck/BP_Truck.BP_Truck_C":
                case "/Game/FactoryGame/Buildable/Vehicle/Explorer/BP_Explorer.BP_Explorer_C":
                case "/Game/FactoryGame/Buildable/Vehicle/Cyberwagon/Testa_BP_WB.Testa_BP_WB_C":
                case "/Game/FactoryGame/Buildable/Vehicle/Golfcart/BP_Golfcart.BP_Golfcart_C":
                case "/Game/FactoryGame/Buildable/Vehicle/Golfcart/BP_GolfcartGold.BP_GolfcartGold_C":
                    var hasAdditionalData = reader.ReadInt32();

                    // conditionally can have more
                    if (reader.BaseStream.Position < reader.BaseStream.Length)
                    {
                        var parentName = reader.ReadLengthPrefixedString();
                        var position = reader.ReadVector3();
                        var rotation = reader.ReadVector3();

                        // Not sure what it is. Always seems to be 29 Bytes.
                        reader.ReadInt32();
                        reader.ReadInt32();
                        reader.ReadInt32();
                        reader.ReadInt32();
                        reader.ReadInt32();
                        reader.ReadInt32();
                        reader.ReadInt32();
                        reader.ReadByte();
                    }

                    break;
                default:
                    // there are types that we did not cover yet. skip parsing until we update this behavior.
                    reader.BaseStream.Position = reader.BaseStream.Length;
                    break;
            }
        }

        public override string ToString()
        {
            return TypePath;
        }
    }
}
