using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Client;
using Client.Data;
using DG.Tweening;
using UnityEngine;
using CameraType = Client.Data.CameraType;

public class CameraService
{
    private CameraSceneData _cameraSceneData;
    private ICoroutineRunner _coroutineRunner;

    private CameraType _currentCameraType;
    private Transform _currentFollowTarget;
    private Transform _currentLookAtTarget;

    private float _lastDelta;

    public CameraService(CameraSceneData cameraSceneData, ICoroutineRunner coroutineRunner)
    {
        _cameraSceneData = cameraSceneData;
        _coroutineRunner = coroutineRunner;
    }

    public CameraSceneData CameraSceneData => _cameraSceneData;

    public void SetCamera(CameraType cameraType, Transform followT, Transform lookAtT)
    {
        _currentCameraType = cameraType;
        _currentFollowTarget = followT;
        _currentLookAtTarget = lookAtT;

        foreach (var camera in _cameraSceneData.Cameras)
            camera.Value.gameObject.SetActive(false);

        _cameraSceneData.Cameras[cameraType].gameObject.SetActive(true);
        _cameraSceneData.Cameras[cameraType].Follow = _currentFollowTarget;
        _cameraSceneData.Cameras[cameraType].LookAt = _currentLookAtTarget;
    }

    public void SetCameraAtTime(CameraType cameraType, Transform followT, Transform lookAtT, float time)
    {
        _coroutineRunner.StartCoroutine(SetCameraAtTimeCoroutine(cameraType, followT, lookAtT, time));
    }

    public void Shake() => _cameraSceneData.ShakeSource.GenerateImpulse(_cameraSceneData.MainCamera.transform.forward);

    public Camera GetCamera() => _cameraSceneData.MainCamera;

    public CinemachineVirtualCamera GetVCByType(CameraType type) => _cameraSceneData.Cameras[type];

    public CinemachineVirtualCamera GetCurrentVC() => _cameraSceneData.Cameras[_currentCameraType];

    private IEnumerator SetCameraAtTimeCoroutine(CameraType cameraType, Transform followT, Transform lookAtT, float time)
    {
        yield return new WaitForSeconds(time);
        SetCamera(cameraType, followT, lookAtT);
    }
}