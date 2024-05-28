using UnityEngine;


public class SingletonScriptableObject<T> : ScriptableObject where T : ScriptableObject
{
    public static T Instance;

    public void CreateInstance()
    {
        Instance = this as T;
    }
}
