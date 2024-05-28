using System;
using Cinemachine;
using Client;
using Client.Data;

[Serializable]
public class VirtualCameraByType : SerializedDictionary<CameraType, CinemachineVirtualCamera>
{
}