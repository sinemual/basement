using UnityEngine;

namespace Client.Data
{
    public static class Animations
    {
        public static readonly int IsIdle = Animator.StringToHash("IsIdle");
        public static readonly int IsWalking = Animator.StringToHash("IsWalking");
        public static readonly int IsTakeDamage = Animator.StringToHash("IsTakeDamage");
        public static readonly int IsShoot = Animator.StringToHash("IsShoot");
        public static readonly int IsMoving = Animator.StringToHash("IsMoving");

        public static readonly string MovementSpeed = "MovementSpeed";

        //--------------States---------------
        public static readonly string CharacterStandIdleNum = "CharacterStandIdleNum";
    }
}