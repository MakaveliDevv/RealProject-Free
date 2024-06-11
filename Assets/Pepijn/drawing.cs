using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleDrawing : MonoBehaviour
{
    public RawImage drawingArea; // The RawImage component we will draw on
    public Camera uiCamera; // The camera rendering the UI
    public int brushSize = 5; // Brush size that can be set from the inspector

    private Texture2D texture;
    private Color[] clearPixels;
    private Vector2 previousPos;

    private bool isDrawing = false;

    void Start()
    {
        InitializeTexture();
    }

    void Update()
    {
        Draw();
    }

    void InitializeTexture()
    {
        RectTransform rt = drawingArea.GetComponent<RectTransform>();

        // Create a new texture with the size of the drawing area
        texture = new Texture2D((int)rt.rect.width, (int)rt.rect.height);
        clearPixels = new Color[texture.width * texture.height];
        for (int i = 0; i < clearPixels.Length; i++)
        {
            clearPixels[i] = Color.white; // Clear color
        }

        texture.SetPixels(clearPixels);
        texture.Apply();

        drawingArea.texture = texture;
    }

    void Draw()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isDrawing = true;
            Vector2 mousePos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingArea.rectTransform, mousePos, uiCamera, out previousPos);
        }

        if (Input.GetMouseButton(0) && isDrawing)
        {
            Vector2 mousePos = Input.mousePosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(drawingArea.rectTransform, mousePos, uiCamera, out Vector2 localPoint);

            // Draw a line from the previous position to the current position
            DrawLine(previousPos, localPoint, Color.black);
            previousPos = localPoint;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDrawing = false;
        }
    }

    void DrawLine(Vector2 start, Vector2 end, Color color)
    {
        int x0 = (int)start.x;
        int y0 = (int)start.y;
        int x1 = (int)end.x;
        int y1 = (int)end.y;

        int dx = Mathf.Abs(x1 - x0);
        int dy = Mathf.Abs(y1 - y0);
        int sx = x0 < x1 ? 1 : -1;
        int sy = y0 < y1 ? 1 : -1;
        int err = dx - dy;

        while (true)
        {
            DrawBrush(x0, y0, color);

            if (x0 == x1 && y0 == y1)
                break;
            int e2 = 2 * err;
            if (e2 > -dy)
            {
                err -= dy;
                x0 += sx;
            }
            if (e2 < dx)
            {
                err += dx;
                y0 += sy;
            }
        }

        texture.Apply();
    }

    void DrawBrush(int x, int y, Color color)
    {
        for (int i = -brushSize; i <= brushSize; i++)
        {
            for (int j = -brushSize; j <= brushSize; j++)
            {
                if (x + i >= 0 && x + i < texture.width && y + j >= 0 && y + j < texture.height)
                {
                    texture.SetPixel(x + i, y + j, color);
                }
            }
        }
    }

    public void ClearCanvas()
    {
        texture.SetPixels(clearPixels);
        texture.Apply();
    }
}
