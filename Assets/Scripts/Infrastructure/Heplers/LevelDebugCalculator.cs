#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using Client.Data;
using Client.Data.Equip;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class LevelDebugCalculator : MonoBehaviour
{
    public bool isDrawExpForEachEntity;
    public StaticData data;
    private (int, int) _blockLevelCounter;

    private void OnDrawGizmos()
    {
        if (isDrawExpForEachEntity)
        {
            var editorStyle = EditorStyles.whiteBoldLabel;
            editorStyle.fontSize = 4;

            var blocks = transform.GetComponentsInChildren<BlockMonoProvider>();

            foreach (var block in blocks)
            foreach (var loot in data.BlocksData[block.Value.Type].Levels[block.Value.Level].Loot)
                if (loot.ItemData is ExperienceData)
                {
                    float blockExp = loot.Amount * (loot.Chance / 100.0f) * data.BlocksData[block.Value.Type].Levels[block.Value.Level].ExpMultiplierForLevelComplete;
                    if (isDrawExpForEachEntity)
                        Handles.Label(block.transform.position, $"{blockExp}", EditorStyles.whiteLabel);
                }
        }
    }

    [Button]
    private void CalculateLevel()
    {
        CalculateBlocks();
        CalculateExperience();
        CalculateResources();
        CalculateMobs();
    }

    private void CalculateExperience()
    {
        float allExp = 0;

        //blocks
        Dictionary<BlockType, float> expBlockCounters = new Dictionary<BlockType, float>();
        foreach (BlockType block in (BlockType[])Enum.GetValues(typeof(BlockType)))
            expBlockCounters.Add(block, 0.0f);

        var blocks = transform.GetComponentsInChildren<BlockMonoProvider>();

        foreach (var block in blocks)
        foreach (var loot in data.BlocksData[block.Value.Type].Levels[block.Value.Level].Loot)
        {
            if (loot.ItemData is ExperienceData expData)
            {
                float blockExp = loot.Amount * (loot.Chance / 100.0f) * data.BlocksData[block.Value.Type].Levels[block.Value.Level].ExpMultiplierForLevelComplete;
                expBlockCounters[block.Value.Type] += blockExp;
                allExp += blockExp;
            }
        }

        //mobs
        Dictionary<CharacterType, float> expMobsCounters = new Dictionary<CharacterType, float>();
        foreach (CharacterType character in (CharacterType[])Enum.GetValues(typeof(CharacterType)))
            expMobsCounters.Add(character, 0.0f);

        var spawnPoints = transform.GetComponentsInChildren<SpawnPointDataMonoProvider>();

        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.Value.Prefab.TryGetComponent(out CharacterMonoProvider characterMonoProvider))
            {
                var charType = characterMonoProvider.Value.Type;
                foreach (var loot in data.CharactersData[charType].Loot)
                {
                    if (loot.ItemData is ExperienceData expData)
                    {
                        float charExp = loot.Amount * (loot.Chance / 100.0f);
                        expMobsCounters[charType] += charExp * (spawnPoint.Value.Chance / 100.0f);
                        allExp += charExp * (spawnPoint.Value.Chance / 100.0f);
                    }
                }
            }
        }

        string blockExpa = "";
        foreach (var expCounter in expBlockCounters)
            blockExpa += $"{expCounter.Key}: {expCounter.Value} | ";

        string mobExpa = "";
        foreach (var expCounter in expMobsCounters)
            mobExpa += $"{expCounter.Key}: {expCounter.Value} | ";

        Debug.Log($"EXPERIENCE (ALL: {allExp}): {blockExpa}\n{mobExpa}");
    }
    
    private void CalculateBlocks()
    {
        int allBlocks = 0;

        Dictionary<BlockType, BlockLevelCounter> blockLevelCounters = new Dictionary<BlockType, BlockLevelCounter>();
        foreach (BlockType block in (BlockType[])Enum.GetValues(typeof(BlockType)))
        {
                blockLevelCounters.Add(block, new BlockLevelCounter());
            for (int i = 0; i < 5; i++)
                blockLevelCounters[block].Value.Add(i, 0);
        }

        var blocks = transform.GetComponentsInChildren<BlockMonoProvider>();

        foreach (var block in blocks)
        {
            //blockCounters[block.Value.Type]++;
            blockLevelCounters[block.Value.Type].Value[block.Value.Level]++;
            allBlocks++;
        }
        
        string blocksLevelStr = "";
        foreach (var block in blockLevelCounters)
        {
            blocksLevelStr += $"{block.Key}:";
            for (int i = 0; i < 5; i++)
                blocksLevelStr += $" lvl {i}:{blockLevelCounters[block.Key].Value[i]} |";
            blocksLevelStr += $"\n----------------\n";
        }
            
        Debug.Log($"BLOCKS (ALL: {allBlocks}):\n{blocksLevelStr}");
    }

    private class BlockLevelCounter
    {
        public Dictionary<int, int> Value = new Dictionary<int, int>();
    }

    private void CalculateResources()
    {
        //blocks
        Dictionary<ResourceType, float> resBlockCounters = new Dictionary<ResourceType, float>();
        foreach (ResourceType res in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
            resBlockCounters.Add(res, 0.0f);

        var blocks = transform.GetComponentsInChildren<BlockMonoProvider>();

        foreach (var block in blocks)
        foreach (var loot in data.BlocksData[block.Value.Type].Levels[block.Value.Level].Loot)
        {
            if (loot.ItemData is ResourceItemData resData)
            {
                float amount = loot.Amount * (loot.Chance / 100.0f);
                resBlockCounters[resData.Type] += amount;
            }
        }

        //mobs
        Dictionary<ResourceType, float> resMobsCounters = new Dictionary<ResourceType, float>();
        foreach (ResourceType character in (ResourceType[])Enum.GetValues(typeof(ResourceType)))
            resMobsCounters.Add(character, 0.0f);

        var spawnPoints = transform.GetComponentsInChildren<SpawnPointDataMonoProvider>();

        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.Value.Prefab.TryGetComponent(out CharacterMonoProvider characterMonoProvider))
            {
                var charType = characterMonoProvider.Value.Type;
                foreach (var loot in data.CharactersData[charType].Loot)
                {
                    if (loot.ItemData is ResourceItemData resData)
                    {
                        float amount = loot.Amount * (loot.Chance / 100.0f);
                        resMobsCounters[resData.Type] += amount * (spawnPoint.Value.Chance / 100.0f);
                    }
                }
            }
        }

        string blockResources = "";
        foreach (var recCounter in resBlockCounters)
            blockResources += $"{recCounter.Key}: {recCounter.Value} | ";

        string mobResources = "";
        foreach (var recCounter in resMobsCounters)
            mobResources += $"{recCounter.Key}: {recCounter.Value} | ";

        Debug.Log($"RESOURCES: BLOCKS:{blockResources}\n MOBS:{mobResources}");
    }

    private void CalculateMobs()
    {
        //mobs
        Dictionary<CharacterType, float> mobsCounters = new Dictionary<CharacterType, float>();
        foreach (CharacterType character in (CharacterType[])Enum.GetValues(typeof(CharacterType)))
            mobsCounters.Add(character, 0);

        var spawnPoints = transform.GetComponentsInChildren<SpawnPointDataMonoProvider>();

        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.Value.Prefab.TryGetComponent(out CharacterMonoProvider characterMonoProvider))
            {
                var charType = characterMonoProvider.Value.Type;
                foreach (var loot in data.CharactersData[charType].Loot)
                    if (loot.ItemData is ExperienceData expData)
                        mobsCounters[charType] += 1 * (spawnPoint.Value.Chance / 100.0f);
            }
        }

        string mobs = "";
        foreach (var mob in mobsCounters)
            mobs += $"{mob.Key}: {mob.Value} | ";

        Debug.Log($"MOBS: \n{mobs}");
    }
}
#endif