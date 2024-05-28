using System;
using Client.Data;
using Data.Base;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(menuName = "GameData/WeaponData", fileName = "WeaponData")]
    [Serializable]
    public class WeaponData : BaseDataSO
    {
        public float ReloadTime;
        public float ShootingIntervalTime;
        public float Range;
        public float Damage;
        public int BulletAmount;
    }
}