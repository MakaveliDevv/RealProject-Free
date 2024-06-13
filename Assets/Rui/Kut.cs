using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.VFX;


public class VFXFade : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 1.0f;
    private Renderer vfxRenderer;
    private MaterialPropertyBlock propertyBlock;

    private void Start()
    {
        // Get the renderer component of the VFX
        vfxRenderer = GetComponent<Renderer>();
        propertyBlock = new MaterialPropertyBlock();

        // Start the fade-in effect
        StartFade(true);
    }

    // Start the fade effect based on the given direction (true for fade-in, false for fade-out)
    private void StartFade(bool fadeIn)
    {
        // Calculate the time interval for each frame
        float fadeInterval = fadeDuration / 100.0f;

        // Start fading
        StartCoroutine(FadeRoutine(fadeInterval, fadeIn));
    }

    // Coroutine to handle fade-in and fade-out
    private IEnumerator FadeRoutine(float fadeInterval, bool fadeIn)
    {
        float elapsedTime = 0.0f;

        // Set the start and end alpha values based on the fade direction
        float startAlpha = fadeIn ? 0.0f : 1.0f;
        float endAlpha = fadeIn ? 1.0f : 0.0f;

        // Fade from startAlpha to endAlpha
        while ((fadeIn && elapsedTime < fadeDuration) || (!fadeIn && elapsedTime < fadeDuration))
        {
            // Calculate the alpha value based on the elapsed time
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);

            // Set the alpha value in the material property block
            propertyBlock.SetFloat("_Alpha", alpha);

            // Apply the material property block to the renderer
            vfxRenderer.SetPropertyBlock(propertyBlock);

            // Increment the elapsed time
            elapsedTime += fadeInterval;

            // Wait for the next frame
            yield return new WaitForSeconds(fadeInterval);
        }

        // Ensure the VFX reaches the target alpha value
        propertyBlock.SetFloat("_Alpha", endAlpha);
        vfxRenderer.SetPropertyBlock(propertyBlock);
    }

    // Public methods to start the fade-in and fade-out effects
    public void StartFadeIn()
    {
        StartFade(true);
    }

    public void StartFadeOut()
    {
        StartFade(false);
    }
}
