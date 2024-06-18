using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class ImageDrawer : NetworkBehaviour
{
    public Color brushColor = Color.black;
    public int brushSize = 10;
    public Image targetImage, worldSpaceImage, floorSpaceImage;
    public int textureWidth = 1920;
    public int textureHeight = 1080;

    public int ammo, individualStarAmmo;
    public float initialStarSize, stepSize;
    [SerializeField] int ammoNeeded;

    private Texture2D drawingTexture;
    private bool isDrawing = false;
    public bool starSelected;
    GameObject currentStar;

    [SerializeField] List<GameObject> starList = new List<GameObject>();

    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = false;

        // Create a new Texture2D
        drawingTexture = new Texture2D(textureWidth, textureHeight, TextureFormat.RGBA32, false);
        
        // Initialize the texture with a white background
        Color[] fillColorArray = drawingTexture.GetPixels();
        for (int i = 0; i < fillColorArray.Length; ++i)
        {
            fillColorArray[i] = Color.white;
        }
        drawingTexture.SetPixels(fillColorArray);
        drawingTexture.Apply();

        // Create a Sprite from the Texture2D and set it to the Image component
        worldSpaceImage.sprite = Sprite.Create(drawingTexture, new Rect(0, 0, drawingTexture.width, drawingTexture.height), new Vector2(0.5f, 0.5f));
        floorSpaceImage.sprite = Sprite.Create(drawingTexture, new Rect(0, 0, drawingTexture.width, drawingTexture.height), new Vector2(0.5f, 0.5f));
        

        GameObject[] stars = GameObject.FindGameObjectsWithTag("Star");
        foreach (GameObject star in stars)
        {
            starList.Add(star);
        }

        individualStarAmmo = ammoNeeded / starList.Count;
        ammo = individualStarAmmo;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }

        if ((isDrawing) && (ammo >= 0))
        {
            DrawServerRpc(Input.mousePosition);
        }
    }

    void ShrinkStars()
    {
        if (!starSelected)
        {
            currentStar = starList[0];
            initialStarSize = currentStar.transform.localScale.x;
            starSelected = true;
            stepSize = initialStarSize / individualStarAmmo;
        }

        float scaleChange = (stepSize * (individualStarAmmo - ammo));
        float newScale = initialStarSize - scaleChange;
        currentStar.transform.localScale = new Vector3(newScale, newScale, newScale);

        if (currentStar.transform.localScale.x <= 0)
        {
            starSelected = false;
            starList.RemoveAt(0);
            if (starList.Count > 0)
            {
                ammo = individualStarAmmo;
            }
        }
    }


    [ServerRpc]
    private void DrawServerRpc(Vector2 screenPosition)
    {
        if (IsServer)
        {
             // Convert world position to screen position
            Vector2 screenPosition2 = Camera.main.WorldToScreenPoint(Input.mousePosition);

            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetImage.rectTransform, screenPosition2, Camera.main, out localPos);

            // Map local position to texture coordinates
            Rect rect = targetImage.rectTransform.rect;
            float pivotOffsetX = rect.width * targetImage.rectTransform.pivot.x;
            float pivotOffsetY = rect.height * targetImage.rectTransform.pivot.y;
            float x = (localPos.x + pivotOffsetX) / rect.width * drawingTexture.width;
            float y = (localPos.y + pivotOffsetY) / rect.height * drawingTexture.height;

            // Vector2 localPos;
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(targetImage.rectTransform, screenPosition, null, out localPos);

            // Map local position to texture coordinates
            // Rect rect = targetImage.rectTransform.rect;
            // float x = (localPos.x - rect.x) / rect.width * drawingTexture.width;
            // float y = (localPos.y - rect.y) / rect.height * drawingTexture.height;


            DrawClientRpc(x, y);
        }
    }
    [ClientRpc]
    private void DrawClientRpc(float x, float y)
    {
        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                if (i * i + j * j <= brushSize * brushSize)
                {
                    int pixelX = Mathf.Clamp((int)x + i, 0, drawingTexture.width - 1);
                    int pixelY = Mathf.Clamp((int)y + j, 0, drawingTexture.height - 1);

                    Color currentColor = drawingTexture.GetPixel(pixelX, pixelY);

                    if (currentColor != brushColor)
                    {
                        // Only set the pixel if the color is different
                        drawingTexture.SetPixel(pixelX, pixelY, brushColor);
                        ammo--;
                        ShrinkStars();
                        //Debug.Log("drawing");
                    }
                    else
                    {
                        //Debug.Log("already black");
                    }
                }
                    
            }
        }

        drawingTexture.Apply();
    }
}
