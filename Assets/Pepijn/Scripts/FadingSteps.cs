using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class FadingSteps : MonoBehaviour
{
    public float duration = 1.0f;  // Duration of the fade effect
    // private Material material;
    // private Color initialColor;
    // private float elapsedTime;
    [SerializeField] private Image image;

    void Update() 
    {
        StartCoroutine(FadeSteps());
    }

    private IEnumerator FadeSteps() 
    {
        float elapsedTime = 0f;

        // Get the initial color of the image
        Color initialColor = image.color;

        // Target color with the alpha value set to 0 (transparent)
        Color targetColor = new(initialColor.r, initialColor.g, initialColor.b, 0f);

        while (elapsedTime < duration)
        {
            // Increase the elapsed time
            elapsedTime += Time.deltaTime;
            
            // Calculate the new color with interpolated alpha
            Color newColor = Color.Lerp(initialColor, targetColor, elapsedTime / duration);
            
            // Apply the new color to the image
            image.color = newColor;
            
            // Wait for the next frame
            yield return null;
        }

        // Ensure the final color is set to the target color
        image.color = targetColor;

        Destroy(gameObject);
    }

    // void Start()
    // {
    //     // Get the material of the object
    //     material = GetComponent<Renderer>().material;
    //     material.color = new Color(0.5f, 0.5f, 0.5f);
    //     initialColor = material.color;
    //     elapsedTime = 0f;
    // }

    // void Update()
    // {
    //     // Update the elapsed time
    //     elapsedTime += Time.deltaTime;

    //     // Calculate the new color by interpolating towards black
    //     Color newColor = Color.Lerp(initialColor, Color.black, elapsedTime / duration);

    //     // Set the new color
    //     material.color = newColor;

    //     // Optionally, stop the update when fully black
    //     if (Mathf.Approximately(newColor.r, 0f) && Mathf.Approximately(newColor.g, 0f) && Mathf.Approximately(newColor.b, 0f))
    //     {
    //         enabled = false; // Disable this script
    //         Destroy(gameObject);
    //     }
    // }

    // public void StartFadeToBlack(float newDuration)
    // {
    //     duration = newDuration;
    //     elapsedTime = 0f;
    //     enabled = true; // Ensure the script is enabled
    // }
}
