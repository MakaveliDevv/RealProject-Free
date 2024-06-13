using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimationStart : MonoBehaviour
{
    private GameObject cutscene;
    public GameObject textGameObj;
    public Image fadeImage; // The image to fade in
    public string nextSceneName; // Name of the next scene to load

    public float waitTimeBeforeFade = 8f; // Time to wait before starting the fade
    public float fadeDuration = 2f; // Duration of the fade

    void Start()
    {
        cutscene = GameObject.Find("Animation");
        cutscene.SetActive(false);
        textGameObj.SetActive(false);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0); // Ensure image starts transparent
        fadeImage.gameObject.SetActive(false); // Disable the image initially
    }

    void Update()
    {
        // Empty Update method - no need to remove
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.name == "FirstPersonController")
        {
            textGameObj.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                cutscene.SetActive(true);
                textGameObj.SetActive(false);
                StartCoroutine(HandleCutscene());
            }
        }
    }

    private IEnumerator HandleCutscene()
    {
        yield return new WaitForSeconds(waitTimeBeforeFade); // Wait for the specified time

        fadeImage.gameObject.SetActive(true); // Enable the image

        float elapsedTime = 0f;
        Color startColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
        Color endColor = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = Color.Lerp(startColor, endColor, elapsedTime / fadeDuration);
            yield return null;
        }

        fadeImage.color = endColor; // Ensure the color is set to the end color

        SceneManager.LoadScene(nextSceneName); // Load the next scene
    }
}
