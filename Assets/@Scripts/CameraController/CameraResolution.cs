using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace _Plugins__Runtime.Resolution
{
    public class CameraResolution : MonoBehaviour
    {
        private const float BaseSize = 6f;

        private const float MinSize = 8f;
        private const float MaxSize = 10f;

        [Header("Camera Zoom Action Properties")]
        [SerializeField] private Ease actionEase = Ease.OutQuad;
        [SerializeField] private float duration = 0.5f;
        
        #region Fields
        public static CameraResolution Instance { get; private set; }
        public Camera Camera => _camera;
        public Camera UICamera { get; private set; }
        private Camera _camera;
        private float _currentScale;
        private float _screenAspect;
        private Tweener _shakeTween;
        private readonly Vector3 _defaultPosition = new(0f, 0f, -10f);
        #endregion
        
        private void Awake()
        {
            Instance = this;
            TryGetComponent(out _camera);
            if (_camera.GetUniversalAdditionalCameraData().cameraStack.Count > 0)
            {
                UICamera = _camera.GetUniversalAdditionalCameraData().cameraStack[0];
            }
            _currentScale = BaseSize;
            Resolution();
        }
        
        private void Resolution()
        {
            _screenAspect = (float)Screen.width / Screen.height;
            _currentScale = _screenAspect switch
            {
                <= 0.38f => MaxSize,
                <= 0.44f => Mathf.Lerp(MaxSize, 17f, GetSize(0.38f, 0.44f)),
                <= 0.46f => Mathf.Lerp(MaxSize, 16f, GetSize(0.44f, 0.46f)),      // 새로운 구간 추가
                <= 0.47f => Mathf.Lerp(16f, 15.7f, GetSize(0.46f, 0.47f)),       // 0.46-0.47 구간
                <= 0.76f => Mathf.Lerp(15.7f, 14.3f, GetSize(0.47f, 0.76f)),
                _ => MinSize
            };
            Debug.Log($"Current SCALE : {_currentScale} / ASPECT : {_screenAspect}");
            SetSizeTween(_currentScale, null);
        }

        private float GetSize(float min, float max) => (_screenAspect - min) / (max - min);
        
        private void OnEnable()
        {
            ResolutionValidate();
        }

        private async void ResolutionValidate()
        {
#if UNITY_EDITOR
            while (true)
            {
                if (!Mathf.Approximately(_screenAspect, (float)Screen.width / Screen.height)) Resolution();
                await UniTask.Delay(1000);
            }
#endif
        }
        
        private void SetSizeTween(float newSize, Action onComplete)
        {
          
            _camera.DOKill(true);
            if (_camera.orthographic)
            {
                float targetSize = Mathf.Clamp(newSize, MinSize, MaxSize);
                _camera.DOOrthoSize(targetSize, duration).SetEase(actionEase).OnComplete(() => onComplete?.Invoke());
            }
        }
        
        public float GetCurrentCameraHeight() => _camera.orthographicSize * 2f;
        public float GetCurrentCameraWidth() => GetCurrentCameraHeight() * _camera.aspect;
    }
}