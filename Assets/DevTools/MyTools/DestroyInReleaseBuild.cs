using UnityEngine;

namespace _Game.Debug
{
    public class DestroyInReleaseBuild : MonoBehaviour
    {
        private void Awake()
        {
            if (!UnityEngine.Debug.isDebugBuild && !Application.isEditor) 
                Destroy(gameObject);
        }
    }
}
