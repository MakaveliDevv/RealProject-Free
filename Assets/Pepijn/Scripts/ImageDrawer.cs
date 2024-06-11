using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ImageDrawer : NetworkBehaviour
{
    public Color brushColor = Color.black;
    public int brushSize = 10;
    public Image targetImage;
    public int textureWidth = 1920;
    public int textureHeight = 1080;

    private Texture2D drawingTexture;
    private bool isDrawing = false;

    void Start()
    {
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
        if (targetImage != null)
        {
            targetImage.sprite = Sprite.Create(drawingTexture, new Rect(0, 0, drawingTexture.width, drawingTexture.height), new Vector2(0.5f, 0.5f));
        }
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

        if (isDrawing)
        {
            DrawServerRpc(Input.mousePosition);
        }
    }
    [ServerRpc]
    private void DrawServerRpc(Vector2 screenPosition)
    {
        if (IsServer)
        {
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(targetImage.rectTransform, screenPosition, null, out localPos);

            // Map local position to texture coordinates
            float x = (localPos.x + targetImage.rectTransform.rect.width / 2f) / targetImage.rectTransform.rect.width * textureWidth;
            float y = (localPos.y + targetImage.rectTransform.rect.height / 2f) / targetImage.rectTransform.rect.height * textureHeight;

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
                    drawingTexture.SetPixel(pixelX, pixelY, brushColor);
                }
            }
        }

        drawingTexture.Apply();
    }
}
