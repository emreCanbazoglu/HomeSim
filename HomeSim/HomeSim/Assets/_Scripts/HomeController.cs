using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public enum ActionEnum
{
    OpenGarageDoor,
    CloseGarageDoor,
    OpenLights,
    CloseLights,
    OpenWindows,
    CloseWindows,
    ActivateCamera,
    DeactivateCamera,
}

public class HomeController : MonoBehaviour 
{
    static HomeController _instance;

    public static HomeController Instance { get { return _instance; } }

    IEnumerator _garageDoorRoutine;
    IEnumerator _lightsRoutine;
    IEnumerator _windowsRoutine;
    IEnumerator _cameraRoutine;

    public Transform GarageDoor;
    public float GarageDoorDuration;
    bool _isGarageDoorOpen;
    Tween _garageTween;

    public Transform WindowCoverCarrier;
    public float WindowDuration;
    bool _isWindowsOpen;
    Tween _windowTween;

    public List<tk2dSprite> LightSpriteList;
    public float LightDuration;
    bool _isLightsOpen;
    Tween[] _lightTweenList;

    public tk2dSpriteAnimator CameraSpriteAnimator;
    bool _isCameraActive;

    void Awake()
    {
        _instance = this;

        _lightTweenList = new Tween[5];
    }

    void OnDestroy()
    {
        _instance = null;
    }

    void Update()
    {
        CheckInput();
    }

    void CheckInput()
    {
        if(Input.GetKeyDown(KeyCode.G))
            SetGarageDoorOpen(!_isGarageDoorOpen);
        if (Input.GetKeyDown(KeyCode.L))
            SetLightsOpen(!_isLightsOpen);
        if (Input.GetKeyDown(KeyCode.W))
            SetWindowsOpen(!_isWindowsOpen);
        if (Input.GetKeyDown(KeyCode.C))
            SetCameraActive(!_isCameraActive);
    }

    public void ProcessAction(ActionEnum actionEnum)
    {
        switch(actionEnum)
        {
            case ActionEnum.OpenGarageDoor:
                SetGarageDoorOpen(true);
                break;
            case ActionEnum.CloseGarageDoor:
                SetGarageDoorOpen(false);
                break;
            case ActionEnum.OpenWindows:
                SetWindowsOpen(true);
                break;
            case ActionEnum.CloseWindows:
                SetWindowsOpen(false);
                break;
            case ActionEnum.OpenLights:
                SetLightsOpen(true);
                break;
            case ActionEnum.CloseLights:
                SetLightsOpen(false);
                break;
            case ActionEnum.ActivateCamera:
                SetCameraActive(true);
                break;
            case ActionEnum.DeactivateCamera:
                SetCameraActive(false);
                break;
        }
    }

    void SetGarageDoorOpen(bool isOpen)
    {
        if (isOpen == _isGarageDoorOpen)
            return;

        if (_garageDoorRoutine != null)
            StopCoroutine(_garageDoorRoutine);

        _garageDoorRoutine = GarageDoorProgress(isOpen);
        StartCoroutine(_garageDoorRoutine);
    }

    IEnumerator GarageDoorProgress(bool open)
    {
        _isGarageDoorOpen = open;

        if(_garageTween != null)
            _garageTween.Kill();

        if (open)
            _garageTween = GarageDoor.DOLocalMoveY(0.53f, GarageDoorDuration);
        else
            _garageTween = GarageDoor.DOLocalMoveY(-1.33f, GarageDoorDuration);

        yield return new WaitForSeconds(GarageDoorDuration);
    }

    void SetWindowsOpen(bool isOpen)
    {
        if (isOpen == _isWindowsOpen)
            return;

        if (_windowsRoutine != null)
            StopCoroutine(_windowsRoutine);

        _windowsRoutine = WindowsProgress(isOpen);
        StartCoroutine(_windowsRoutine);
    }

    IEnumerator WindowsProgress(bool isOpen)
    {
        _isWindowsOpen = isOpen;

        if (_windowTween != null)
            _windowTween.Kill();

        if (isOpen)
            _windowTween = WindowCoverCarrier.DOLocalMoveY(0.26f, WindowDuration);
        else
            _windowTween = WindowCoverCarrier.DOLocalMoveY(-0.26f, WindowDuration);

        yield return new WaitForSeconds(WindowDuration);
    }

    void SetLightsOpen(bool isOpen)
    {
        if (isOpen == _isLightsOpen)
            return;

        if (_lightsRoutine != null)
            StopCoroutine(_lightsRoutine);

        _lightsRoutine = LightsProgress(isOpen);
        StartCoroutine(_lightsRoutine);
    }

    IEnumerator LightsProgress(bool isOpen)
    {
        _isLightsOpen = isOpen;

        foreach (Tween t in _lightTweenList)
        {
            if (t != null)
                t.Kill();
        }

        for (int i = 0; i < LightSpriteList.Count; i++)
        {
            if (isOpen)
                _lightTweenList[i] = LightSpriteList[i].DOFade(1, LightDuration);
            else
                _lightTweenList[i] = LightSpriteList[i].DOFade(0, LightDuration);
        }

        yield return new WaitForSeconds(LightDuration);
    }

    void SetCameraActive(bool isActive)
    {
        if (isActive == _isCameraActive)
            return;

        _isCameraActive = isActive;

        if(isActive)
            CameraSpriteAnimator.Play();
        else
            CameraSpriteAnimator.Stop();
    }
}
