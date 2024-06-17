using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnimationStart : MonoBehaviour
{
    private GameObject cutscene;
    public Image fadeImage; // The image to fade in
    public string nextSceneName; // Name of the next scene to load

    public float waitTimeBeforeFade = 8f; // Time to wait before starting the fade
    public float fadeDuration = 2f; // Duration of the fade
    public GameObject UIelement;
    public PlayerInteraction _PlayerInteraction;
    private bool animStart;
    public bool playerInRange;

    void Start()
    {
        cutscene = GameObject.Find("Animation");
        cutscene.SetActive(false);
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0); // Ensure image starts transparent
        fadeImage.gameObject.SetActive(false); // Disable the image initially
    }

    private void Update() 
    {
        if(_PlayerInteraction != null)
        {
            if(_PlayerInteraction._Interactable.objectPickedup && playerInRange)
                _PlayerInteraction._Interactable.interactionUI.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider collider)
    {
        if(collider.CompareTag("Player"))
        {
            Debug.Log("Collision with player detected");
            if(collider.TryGetComponent<PlayerInteraction>(out var interaction))
            {
                // Reference to the player interaction to gain access to the interactable object
                _PlayerInteraction = interaction;
                playerInRange = true;

                if(!animStart) 
                {
                    // Check if the object is released
                    if(_PlayerInteraction._Interactable.objectReleased)
                    {
                        // Start animation
                        cutscene.SetActive(true);
                        StartCoroutine(HandleCutscene()); 
                        UIelement.SetActive(false);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider collider) 
    {
        if(collider.CompareTag("Player"))
        {
            playerInRange = false;
            UIelement.SetActive(false);
        }
    }

    private IEnumerator HandleCutscene()
    {
        yield return new WaitForSeconds(waitTimeBeforeFade); // Wait for the specified time

        animStart = true;
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
