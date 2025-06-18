using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    
    private float backgroundWidth;
    private Camera cam;
    private Transform[] backgroundPieces;
    
    private void Start()
    {
        cam = Camera.main;

        if (cam == null)
        {
            Debug.LogError("camera not found");
            return;
        }
        
        backgroundPieces = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            backgroundPieces[i] = transform.GetChild(i);
        }
        
        if (backgroundPieces.Length > 0)
        {
            SpriteRenderer sr = backgroundPieces[0].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                backgroundWidth = sr.bounds.size.x;
            }
        }
    }
    
    private void Update()
    {
        CheckAndRepositionBackgrounds();
    }
    
    private void CheckAndRepositionBackgrounds()
    {
        float cameraLeftEdge = cam.transform.position.x - (cam.orthographicSize * cam.aspect);
        float cameraRightEdge = cam.transform.position.x + (cam.orthographicSize * cam.aspect);
        float repositionBuffer = backgroundWidth * 0.5f;
        
        foreach (Transform piece in backgroundPieces)
        {
            // 왼쪽으로 벗어나면 오른쪽 끝 재배치
            if (piece.position.x + backgroundWidth * 0.5f < cameraLeftEdge - repositionBuffer)
            {
                float rightmostX = GetRightmostPosition();
                piece.position = new Vector3(rightmostX + backgroundWidth - 0.1f, piece.position.y, piece.position.z);
            }
            
            // 오른쪽으로 벗어나면 왼쪽 끝 재배치
            else if (piece.position.x - backgroundWidth * 0.5f > cameraRightEdge + repositionBuffer)
            {
                float leftmostX = GetLeftmostPosition();
                piece.position = new Vector3(leftmostX - backgroundWidth + 0.1f, piece.position.y, piece.position.z);
            }
        }
    }
    
    private float GetRightmostPosition()
    {
        float rightmost = float.MinValue;
        foreach (Transform piece in backgroundPieces)
        {
            if (piece.position.x > rightmost)
                rightmost = piece.position.x;
        }
        return rightmost;
    }
    
    
    private float GetLeftmostPosition()
    {
        float leftmost = float.MaxValue;
        foreach (Transform piece in backgroundPieces)
        {
            if (piece.position.x < leftmost)
                leftmost = piece.position.x;
        }
        return leftmost;
    }
}