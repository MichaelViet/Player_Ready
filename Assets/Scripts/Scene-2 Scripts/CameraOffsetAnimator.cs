using System.Collections;
using UnityEngine;
using Cinemachine;
using System;

public class CameraOffsetAnimator : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float duration = 20f;
    private Vector3 startPosition = new Vector3(257.6f, 44.42f, 0f);
    private Vector3 endPosition = new Vector3(0f, 3f, 0f);
    public bool isAnimationPlayed = false;
    public static event Action OnAnimationEnd;
    public bool isCameraAnimating = false;

    void Start()
    {
        if (isAnimationPlayed == false)
        {
            isCameraAnimating = true;  // встановити isCameraAnimating на true
            StartCoroutine(AnimateOffset());
        }
    }

    public IEnumerator AnimateOffset()
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = Vector3.Lerp(startPosition, endPosition, Mathf.SmoothStep(0.0f, 1.0f, t));
            yield return null;
        }
        isAnimationPlayed = true;
        isCameraAnimating = false;  // встановити isCameraAnimating на false після завершення анімації
        OnAnimationEnd?.Invoke();
    }
}