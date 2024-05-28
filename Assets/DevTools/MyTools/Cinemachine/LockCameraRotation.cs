using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Client
{
    /// <summary>
    /// An add-on module for Cinemachine Virtual Camera that locks the camera's Y co-ordinate
    /// </summary>
    [SaveDuringPlay] [AddComponentMenu("")] // Hide in menu
    public class LockCameraRotation : CinemachineExtension
    {
        public float m_XPosition = 10;
        public float m_YPosition = 10;
        public float m_ZPosition = 10;
 
        protected override void PostPipelineStageCallback(
            CinemachineVirtualCameraBase vcam,
            CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
        {
            if (stage == CinemachineCore.Stage.Finalize)
            {
                var pos = state.RawPosition;
                pos.x = m_XPosition;
                pos.y = m_YPosition;
                pos.z = m_ZPosition;
                state.RawPosition = pos;
            }
        }
    }
}
