using System;
using System.Collections.Generic;
using System.Linq;
using Client.Data.Equip;
using Client.DevTools.MyTools;
using Data;
using Data.Base;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Client.Data
{
    [CreateAssetMenu(menuName = "GameData/StaticData", fileName = "StaticData")]
    public class StaticData : BaseDataSO, IResetable
    {
        public Material HitMaterial;
        
        [FoldoutGroup("GlobalMap")]public Material PreviousLevelPointMaterial;
        [FoldoutGroup("GlobalMap")]public Material CurrentLevelPointMaterial;
        [FoldoutGroup("GlobalMap")]public Material NextLevelPointMaterial;
        [FoldoutGroup("GlobalMap")]public Material OpenedLocationMaterial;
        [FoldoutGroup("GlobalMap")]public Material ClosedLocationMaterial;
        
        [FoldoutGroup("Prefabs")] public PrefabData PrefabData;
        [FoldoutGroup("Levels")] public LevelsData LevelsData;

        public BlockTypeData BlocksData;
        public CharacterTypeData CharactersData;
        public ResourceTypeData ResourcesData;
        public ChestByLevelData ChestsData;
        public BuildingTypeData BuildingsData;
        public LocationNeededLevelData LocationNeededLevelData;
        
        [FoldoutGroup("Tags"), Tag] public string GroundTag;
        [FoldoutGroup("Tags"), Tag] public string PlayerTag;
        [FoldoutGroup("Tags"), Tag] public string DespawnTag;
        
        [FoldoutGroup("Layers")] public LayerMask RaycastMask;
        [FoldoutGroup("Layers")] public LayerMask PlayerMask;
        [FoldoutGroup("Layers")] public LayerMask TutorialMask;
        [FoldoutGroup("Layers")] public LayerMask IgnoreMask;
        [FoldoutGroup("Layers")] public LayerMask BlocksMask;

        [FoldoutGroup("Tutorial")] public Material TutorialMaterial;
        
        public List<ItemData> ItemDatabase;
        public List<GoalData> GameProgressGoals;
        public TutorialStepData Tutorials;

        public EquipByTypeData AllEquip;
        public RecipeEquipByTypeData EquipRecipes;
        public List<CraftRecipeData> ItemRecipes;
        
        public int GetPlayerLayer => Utility.ToLayer(PlayerMask);
        public int GetRaycastLayer => Utility.ToLayer(RaycastMask);
        public int GetIgnoreLayer => Utility.ToLayer(IgnoreMask);
        public int GetBlocksLayer => Utility.ToLayer(BlocksMask);
        
#if UNITY_EDITOR
        [Button]
        public void FillItemDatabase()
        {
            ItemDatabase = new List<ItemData>();
            ItemDatabase = Utility.GetAllInstances<ItemData>().ToList();
        }
        
        [Button]
        public void FillRecipesDatabase()
        {
            ItemRecipes = new List<CraftRecipeData>();
            ItemRecipes = Utility.GetAllInstances<CraftRecipeData>().ToList();
        }
        
        [Button]
        public void FillEquipRecipesDatabase()
        {
            foreach (EquipType equipType in (EquipType[])Enum.GetValues(typeof(EquipType)))
            foreach (var recipe in EquipRecipes[equipType].Value)
                foreach (var item in AllEquip[equipType].Value)
                            if (recipe.Level == item.Level)
                                recipe.GettedItem = item;
        }
#endif
        public void ResetData()
        {
            
        }
    }

    [Serializable]
    public class BlockTypeData : SerializedDictionary<BlockType, BlockDataList>
    {
    }
    
    [Serializable]
    public class BlockDataList
    {
        public List<BlockData> Levels;
    }
    
    [Serializable]
    public class TutorialStepData : SerializedDictionary<TutorialStep, TutorialData>
    {
    }
        
    [Serializable]
    public class CharacterTypeData : SerializedDictionary<CharacterType, CharacterData>
    {
    }
    
    [Serializable]
    public class EquipByTypeData : SerializedDictionary<EquipType, ListEquipItemData>
    {
    }

    [Serializable]
    public class ListEquipItemData
    {
        public List<EquipItemData> Value;
    }
    
    [Serializable]
    public class RecipeEquipByTypeData : SerializedDictionary<EquipType, ListRecipeEquipItemData>
    {
    }
    
    [Serializable]
    public class ListRecipeEquipItemData
    {
        public List<CraftRecipeData> Value;
    }

    [Serializable]
    public class ResourceTypeData : SerializedDictionary<ResourceType, ResourceItemData>
    {
    }
    
    [Serializable]
    public class ChestByLevelData : SerializedDictionary<LocationType, ChestItemData>
    {
    }
    
    [Serializable]
    public class BuildingTypeData : SerializedDictionary<BuildingType, ListBuildingData>
    {
    }
    
    [Serializable]
    public class ListBuildingData
    {
        public List<BuildingData> Value;
    }
        
    [Serializable]
    public class LocationNeededLevelData : SerializedDictionary<LocationType, int>
    {
    }
}