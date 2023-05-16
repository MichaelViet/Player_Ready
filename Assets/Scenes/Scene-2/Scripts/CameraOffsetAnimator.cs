using System.Collections;
using UnityEngine;
using Cinemachine;

public class CameraOffsetAnimator : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float duration = 20f;
    private Vector3 startPosition = new Vector3(257.6798f, 44.42f, 0f);
    private Vector3 endPosition = new Vector3(0f, 4f, 0f);
    public bool isAnimationPlayed = false;

    void Start()
    {

        if (isAnimationPlayed == false)
        {
            StartCoroutine(AnimateOffset());
        }
        else
        {
            LoadCameraPositionAndState();
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
    }

    void SaveCameraPositionAndState()
    {
        Vector3 cameraPosition = virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset;
        PlayerPrefs.SetFloat("CameraPositionX", cameraPosition.x);
        PlayerPrefs.SetFloat("CameraPositionY", cameraPosition.y);
        PlayerPrefs.SetFloat("CameraPositionZ", cameraPosition.z);
        PlayerPrefs.Save();
    }

    void LoadCameraPositionAndState()
    {
        if (PlayerPrefs.HasKey("CameraPositionX") && PlayerPrefs.HasKey("CameraPositionY") && PlayerPrefs.HasKey("CameraPositionZ") && PlayerPrefs.HasKey("isAnimationPlayed"))
        {
            float x = PlayerPrefs.GetFloat("CameraPositionX");
            float y = PlayerPrefs.GetFloat("CameraPositionY");
            float z = PlayerPrefs.GetFloat("CameraPositionZ");
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_TrackedObjectOffset = new Vector3(x, y, z);
        }
    }
}


