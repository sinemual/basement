using System;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

namespace Client.Data
{
    [Serializable]
    public class CameraSceneData
    {
        public Camera MainCamera;
        public CinemachineImpulseSource ShakeSource;
        
        [Header("VCs")] public VirtualCameraByType Cameras;
    }
}