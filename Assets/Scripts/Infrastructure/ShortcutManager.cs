using System;
using Client.Data.Core;
using UnityEngine;

public class ShortcutManager : MonoBehaviour
{
#if UNITY_EDITOR
    private SharedData _data;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
                _data.ResetData();

        if (Input.GetKeyDown(KeyCode.Alpha1)) Time.timeScale = Math.Abs(Time.timeScale - 1.0f) < 0.01 ? 0.0f : 1.0f;

        if (Input.GetKeyDown(KeyCode.Alpha2)) Time.timeScale = 2.0f;

        if (Input.GetKeyDown(KeyCode.Alpha3)) Time.timeScale = 3.0f;
    }
#endif
}