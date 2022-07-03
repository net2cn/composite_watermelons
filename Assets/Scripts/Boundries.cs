using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundries : MonoBehaviour
{
    public PhysicsMaterial2D BoundriesPhysicsMaterial;
    public float LostHeightPercentage = 0.7f;
    public float WarnHeightPercentage = 0.1f;
    public float LostLineWidth = 0.01f;
    public Color LostLineColor = new Color(255, 0, 0);
    public Material LoseLineMaterial;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 lDCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0f, Camera.main.nearClipPlane));
        Vector2 rUCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
        Vector2[] colliderPoints;

        GameObject bounderies = new GameObject("Bounderies");

        EdgeCollider2D upperEdge = new GameObject("UpperEdge").AddComponent<EdgeCollider2D>();
        upperEdge.transform.SetParent(bounderies.transform);
        colliderPoints = upperEdge.points;
        colliderPoints[0] = new Vector2(lDCorner.x, rUCorner.y);
        colliderPoints[1] = new Vector2(rUCorner.x, rUCorner.y);
        upperEdge.points = colliderPoints;
        upperEdge.sharedMaterial= BoundriesPhysicsMaterial;
        
        EdgeCollider2D lowerEdge = new GameObject("LowerEdge").AddComponent<EdgeCollider2D>();
        lowerEdge.transform.SetParent(bounderies.transform);
        colliderPoints = lowerEdge.points;
        colliderPoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderPoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        lowerEdge.points = colliderPoints;
        lowerEdge.sharedMaterial = BoundriesPhysicsMaterial;

        EdgeCollider2D leftEdge = new GameObject("LeftEdge").AddComponent<EdgeCollider2D>();
        leftEdge.transform.SetParent(bounderies.transform);
        colliderPoints = leftEdge.points;
        colliderPoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderPoints[1] = new Vector2(lDCorner.x, rUCorner.y);
        leftEdge.points = colliderPoints;
        leftEdge.sharedMaterial = BoundriesPhysicsMaterial;

        EdgeCollider2D rightEdge = new GameObject("RightEdge").AddComponent<EdgeCollider2D>();
        rightEdge.transform.SetParent(bounderies.transform);
        colliderPoints = rightEdge.points;
        colliderPoints[0] = new Vector2(rUCorner.x, rUCorner.y);
        colliderPoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        rightEdge.points = colliderPoints;
        rightEdge.sharedMaterial = BoundriesPhysicsMaterial;

        EdgeCollider2D gameLoseEdge = new GameObject("GameLoseEdge").AddComponent<EdgeCollider2D>();
        gameLoseEdge.transform.SetParent(bounderies.transform);
        colliderPoints = gameLoseEdge.points;
        colliderPoints[0] = new Vector2(lDCorner.x, rUCorner.y * LostHeightPercentage);
        colliderPoints[1] = new Vector2(rUCorner.x, rUCorner.y * LostHeightPercentage);
        gameLoseEdge.points = colliderPoints;
        gameLoseEdge.isTrigger = true;
        gameLoseEdge.gameObject.AddComponent<GameLose>();

        LineRenderer gameLoseLine = gameLoseEdge.gameObject.AddComponent<LineRenderer>();
        gameLoseLine.positionCount = 2;
        gameLoseLine.widthMultiplier = rUCorner.y * LostLineWidth;
        gameLoseLine.material = LoseLineMaterial;
        gameLoseLine.startColor = LostLineColor;
        gameLoseLine.endColor = LostLineColor;
        gameLoseLine.SetPosition(0, new Vector3(lDCorner.x, rUCorner.y * LostHeightPercentage));
        gameLoseLine.SetPosition(1, new Vector3(rUCorner.x, rUCorner.y * LostHeightPercentage));

        BoxCollider2D gameWarnBox = new GameObject("GameWarnBox").AddComponent<BoxCollider2D>();
        gameWarnBox.transform.SetParent(bounderies.transform);
        gameWarnBox.transform.position = new Vector3(0, rUCorner.y * LostHeightPercentage, 0);
        var colliderSize = gameWarnBox.size;
        colliderSize.x = rUCorner.x - lDCorner.x;
        colliderSize.y = rUCorner.y * 2 * WarnHeightPercentage;
        gameWarnBox.size = colliderSize;
        gameWarnBox.transform.position = new Vector3(0, rUCorner.y * LostHeightPercentage - colliderSize.y / 2, 0);
        gameWarnBox.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
