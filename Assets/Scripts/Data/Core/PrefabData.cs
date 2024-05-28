using System;
using System.Collections.Generic;
using Client.Data.Equip;
using UnityEngine;

namespace Client.Data
{
    [Serializable]
    public class PrefabData
    {
        public GameObject EmptyPrefab;
        public GameObject PlayerPrefab;
        public GameObject TapProgressBarPrefab;
        public GameObject HealthBarPrefab;
        public GameObject TntPickaxeExplosionSourcePrefab;

        [Header("Money")] 
        public GameObject EarnInfoPrefab;
        public GameObject DropMoneyPrefab;
        public ShotByCharacterType ShotPrefabs;

        [Header("VFXs")] 
        public MiningVfxByType MiningVfxPrefabs;
        public MiningAnimViewByType MiningAnimViewPrefabs;
        public GameObject PickUpItemVfxPrefab; // flash in corner of screen
        public GameObject HitVfxPrefab; // flash in corner of screen
        public GameObject ExplosionVfxPrefab;
        
        [Header("Pool Prefabs")] 
        public PoolPrefab PoolPrefabData;
        
        [Serializable]
        public class PoolPrefab : SerializedDictionary<GameObject, int> // pool depth
        {
        }
        
        
        [Serializable]
        public class MiningVfxByType : SerializedDictionary<BlockType, GameObject>
        {
        }
        
        [Serializable]
        public class MiningAnimViewByType : SerializedDictionary<EquipType, GameObject>
        {
        }
        
        [Serializable]
        public class ShotByCharacterType : SerializedDictionary<CharacterType, GameObject>
        {
        }
    }
}