using System;
using Client.Data;
using UnityEngine;

[Serializable]
public struct CharacterProvider
{
    public CharacterType Type;
    public GameObject Head;
    public Transform ShotPoint;
}