using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class yet : MonoBehaviour
{
    public static yet instance;
    public CinemachineVirtualCamera virtualCamera; // The Cinemachine Virtual Camera
    public Image targetImage; // The Image component on the Canvas
    public float targetFOV = 60f; // The target FOV for the camera
    public float fovChangeDuration = 2f; // Duration of the FOV change process
    public float fadeDuration = 2f; // Duration of the fade-out process
    public GameObject footsteps;
    bool zoomedOut;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        StartCoroutine(FadeImageAndChangeFOV());
    }

    private IEnumerator FadeImageAndChangeFOV()
    {
        // Fade out the image
        Color initialColor = targetImage.color;
        float elapsedTimeFade = 0f;

        while (elapsedTimeFade < fadeDuration)
        {
            elapsedTimeFade += Time.deltaTime;
            float alpha = Mathf.Lerp(initialColor.a, 0f, elapsedTimeFade / fadeDuration);
            targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, alpha);
            yield return null;
        }

        targetImage.color = new Color(initialColor.r, initialColor.g, initialColor.b, 0f); // Ensure alpha is 0 at the end

        // Change FOV of the camera
        float initialFOV = virtualCamera.m_Lens.FieldOfView;
        float elapsedTimeFOV = 0f;

        while (elapsedTimeFOV < fovChangeDuration)
        {
            elapsedTimeFOV += Time.deltaTime;
            float t = elapsedTimeFOV / fovChangeDuration;
            float smoothT = Mathf.SmoothStep(0f, 1f, t); // SmoothStep for smoother interpolation
            float currentFOV = Mathf.Lerp(initialFOV, targetFOV, smoothT);
            virtualCamera.m_Lens.FieldOfView = currentFOV;
            yield return null;
        }

        virtualCamera.m_Lens.FieldOfView = targetFOV; // Ensure FOV is set to the target value at the end
        zoomedOut = true;
        footsteps.SetActive(true);
    }
}
