using System;
using System.Collections.Generic;
using Client;
using UnityEngine;

[Serializable]
public struct GlobalMapProvider
{
    public GameObject PlayerAvatar;
    public GameObject CurrentPointParticle;
    public List<GameObject> CameraPath;
    public List<LevelPoint> LevelPoints;
    public RendererByLocationType Locations;

    [Serializable]
    public class RendererByLocationType : SerializedDictionary<LocationType, Renderer>
    {
    }
}