using System.Collections.Generic;

namespace SatisfactorySaveEditor.Model
{
    public class ResourceType
    {
        public string ItemPath { get; }
        public string Name { get; }
        public int Quantity { get; set; }

        public string Image => $"pack://application:,,,/Icon/{Name.Replace(' ', '_')}.png";

        public ResourceType(string itemPath, string name, int qty)
        {
            ItemPath = itemPath;
            Name = name;
            Quantity = qty;
        }
    }

    public static class ResourceTypes
    {
        public static readonly List<ResourceType> RESOURCES = new List<ResourceType>()
        {
            new ResourceType(string.Empty, "Choose Item", 0),
            new ResourceType("/Game/FactoryGame/Resource/Parts/CircuitBoardHighSpeed/Desc_CircuitBoardHighSpeed.Desc_CircuitBoardHighSpeed_C", "A.I. Limiter", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/AluminumPlate/Desc_AluminumPlate.Desc_AluminumPlate_C", "Alclad Aluminum Sheet", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Battery/Desc_Battery.Desc_Battery_C", "Battery", 100),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/OreBauxite/Desc_OreBauxite.Desc_OreBauxite_C", "Bauxite Ore", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/Beacon/BP_EquipmentDescriptorBeacon.BP_EquipmentDescriptorBeacon_C", "Beacon", 100),
            new ResourceType("/Game/FactoryGame/Resource/Environment/Berry/Desc_Berry.Desc_Berry_C", "Beryl Nut", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/BioFuel/Desc_Biofuel.Desc_Biofuel_C", "Biofuel", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/GenericBiomass/Desc_GenericBiomass.Desc_GenericBiomass_C", "Biomass", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/GunPowder/Desc_Gunpowder.Desc_Gunpowder_C", "Black Powder", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/JumpingStilts/BP_EquipmentDescriptorJumpingStilts.BP_EquipmentDescriptorJumpingStilts_C", "Blade Runners", 1),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Cable/Desc_Cable.Desc_Cable_C", "Cable", 100),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/OreGold/Desc_OreGold.Desc_OreGold_C", "Caterium Ore", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/GoldIngot/Desc_GoldIngot.Desc_GoldIngot_C", "Caterium Ingot", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/CartridgeStandard/Desc_CartridgeStandard.Desc_CartridgeStandard_C", "Cartridge", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/CircuitBoard/Desc_CircuitBoard.Desc_CircuitBoard_C", "Circuit Board", 100),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/Coal/Desc_Coal.Desc_Coal_C", "Coal", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/ColorCartridge/Desc_ColorCartridge.Desc_ColorCartridge_C", "Color Cartridge", 200),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/ColorGun/BP_EquipmentDescriptorColorGun.BP_EquipmentDescriptorColorGun_C", "Color Gun", 1),
            new ResourceType("/Game/FactoryGame/Resource/Parts/CompactedCoal/Desc_CompactedCoal.Desc_CompactedCoal_C", "Compacted Coal", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Computer/Desc_Computer.Desc_Computer_C", "Computer", 50),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Cement/Desc_Cement.Desc_Cement_C", "Concrete", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/CopperIngot/Desc_CopperIngot.Desc_CopperIngot_C", "Copper Ingot", 100),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/OreCopper/Desc_OreCopper.Desc_OreCopper_C", "Copper Ore", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/CopperSheet/Desc_CopperSheet.Desc_CopperSheet_C", "Copper Sheet", 200),
            new ResourceType("/Game/FactoryGame/Resource/Parts/CrystalOscillator/Desc_CrystalOscillator.Desc_CrystalOscillator_C", "Crystal Oscillator", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/ElectromagneticControlRod/Desc_ElectromagneticControlRod.Desc_ElectromagneticControlRod_C", "Electromagnetic Control Rod", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/SteelPlateReinforced/Desc_SteelPlateReinforced.Desc_SteelPlateReinforced_C", "Encased Industrial Beam", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/GenericBiomass/Desc_Fabric.Desc_Fabric_C", "Fabric", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Filter/Desc_Filter.Desc_Filter_C", "Filter", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/GenericBiomass/Desc_FlowerPetals.Desc_FlowerPetals_C", "Flower Petals", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Fuel/Desc_Fuel.Desc_Fuel_C", "Fuel", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/GasMask/BP_EquipmentDescriptorGasmask.BP_EquipmentDescriptorGasmask_C", "Gas Mask", 1),
            new ResourceType("/Game/FactoryGame/Resource/Parts/AluminumPlateReinforced/Desc_AluminumPlateReinforced.Desc_AluminumPlateReinforced_C", "Heat Sink", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/ModularFrameHeavy/Desc_ModularFrameHeavy.Desc_ModularFrameHeavy_C", "Heavy Modular Frame", 50),
            new ResourceType("/Game/FactoryGame/Resource/Parts/HighSpeedConnector/Desc_HighSpeedConnector.Desc_HighSpeedConnector_C", "High-Speed Connector", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/IronIngot/Desc_IronIngot.Desc_IronIngot_C", "Iron Ingot", 100),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/OreIron/Desc_OreIron.Desc_OreIron_C", "Iron Ore", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/IronPlate/Desc_IronPlate.Desc_IronPlate_C", "Iron Plate", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/IronRod/Desc_IronRod.Desc_IronRod_C", "Iron Rod", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/JetPack/BP_EquipmentDescriptorJetPack.BP_EquipmentDescriptorJetPack_C", "Jetpack", 1),
            new ResourceType("/Game/FactoryGame/Resource/Parts/GenericBiomass/Desc_Leaves.Desc_Leaves_C", "Leaves", 100),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/Stone/Desc_Stone.Desc_Stone_C", "Limestone", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/Medkit/Desc_Medkit.Desc_Medkit_C", "Medicinal Inhaler", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/ModularFrame/Desc_ModularFrame.Desc_ModularFrame_C", "Modular Frame", 50),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Motor/Desc_Motor.Desc_Motor_C", "Motor", 50),
            new ResourceType("/Game/FactoryGame/Resource/Parts/GenericBiomass/Desc_Mycelia.Desc_Mycelia_C", "Mycelia", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/NobeliskDetonator/BP_EquipmentDescriptorNobeliskDetonator.BP_EquipmentDescriptorNobeliskDetonator_C", "Nobelisk Detonator", 1),
            new ResourceType("/Game/FactoryGame/Resource/Parts/NobeliskExplosive/Desc_NobeliskExplosive.Desc_NobeliskExplosive_C", "Nobelisk", 50),
            new ResourceType("/Game/FactoryGame/Resource/Parts/NuclearWaste/Desc_NuclearWaste.Desc_NuclearWaste_C", "Nuclear Waste", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/GemstoneScanner/BP_EquipmentDescriptorObjectScanner.BP_EquipmentDescriptorObjectScanner_C", "Object Scanner", 1),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/CrudeOil/Desc_CrudeOil.Desc_CrudeOil_C", "Crude Oil", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/Beacon/Desc_Parachute.Desc_Parachute_C", "Parachute", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Plastic/Desc_Plastic.Desc_Plastic_C", "Plastic", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/PortableMiner/BP_ItemDescriptorPortableMiner.BP_ItemDescriptorPortableMiner_C", "Portable Miner", 1),
            new ResourceType("/Game/FactoryGame/Resource/Parts/ComputerQuantum/Desc_ComputerQuantum.Desc_ComputerQuantum_C", "Quantum Computer", 50),
            new ResourceType("/Game/FactoryGame/Resource/Parts/QuartzCrystal/Desc_QuartzCrystal.Desc_QuartzCrystal_C", "Quartz Crystal", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/HighSpeedWire/Desc_HighSpeedWire.Desc_HighSpeedWire_C", "Quickwire", 500),
            new ResourceType("/Game/FactoryGame/Resource/Parts/ModularFrameLightweight/Desc_ModularFrameLightweight.Desc_ModularFrameLightweight_C", "Radio Control Unit", 100),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/RawQuartz/Desc_RawQuartz.Desc_RawQuartz_C", "Raw Quartz", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/NailGun/Desc_RebarGunProjectile.Desc_RebarGunProjectile_C", "Rebar Gun", 1),
            new ResourceType("/Game/FactoryGame/Resource/Parts/IronPlateReinforced/Desc_IronPlateReinforced.Desc_IronPlateReinforced_C", "Reinforced Iron Plate", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/Rifle/BP_EquipmentDescriptorRifle.BP_EquipmentDescriptorRifle_C", "Rifle", 1),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Rotor/Desc_Rotor.Desc_Rotor_C", "Rotor", 50),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Rubber/Desc_Rubber.Desc_Rubber_C", "Rubber", 100),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/SAM/Desc_SAM.Desc_SAM_C", "S.A.M. Ore", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/IronScrew/Desc_IronScrew.Desc_IronScrew_C", "Screw", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Silica/Desc_Silica.Desc_Silica_C", "Silica", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/SpikedRebar/Desc_SpikedRebar.Desc_SpikedRebar_C", "Spiked Rebar", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Stator/Desc_Stator.Desc_Stator_C", "Stator", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/SteelPlate/Desc_SteelPlate.Desc_SteelPlate_C", "Steel Beam", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/SteelIngot/Desc_SteelIngot.Desc_SteelIngot_C", "Steel Ingot", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/SteelPipe/Desc_SteelPipe.Desc_SteelPipe_C", "Steel Pipe", 100),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/Sulfur/Desc_Sulfur.Desc_Sulfur_C", "Sulfur", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/ComputerSuper/Desc_ComputerSuper.Desc_ComputerSuper_C", "Supercomputer", 50),
            new ResourceType("/Game/FactoryGame/Resource/Parts/QuantumOscillator/Desc_QuantumOscillator.Desc_QuantumOscillator_C", "Superposition Oscillator", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Turbofuel/Desc_TurboFuel.Desc_TurboFuel_C", "Turbo Fuel", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/MotorLightweight/Desc_MotorLightweight.Desc_MotorLightweight_C", "Turbo Motor", 50),
            new ResourceType("/Game/FactoryGame/Resource/RawResources/OreUranium/Desc_OreUranium.Desc_OreUranium_C", "Uranium", 100),
            new ResourceType("/Game/FactoryGame/Resource/Parts/Wire/Desc_Wire.Desc_Wire_C", "Wire", 500),
            new ResourceType("/Game/FactoryGame/Resource/Parts/GenericBiomass/Desc_Wood.Desc_Wood_C", "Wood", 100),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/StunSpear/BP_EquipmentDescriptorStunSpear.BP_EquipmentDescriptorStunSpear_C", "Xeno-Basher", 1),
            new ResourceType("/Game/FactoryGame/Resource/Equipment/ShockShank/BP_EquipmentDescriptorShockShank.BP_EquipmentDescriptorShockShank_C", "Xeno-Zapper", 1),
        };
    }
}
