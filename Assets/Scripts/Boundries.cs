using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundries : MonoBehaviour
{
    public PhysicsMaterial2D BoundriesPhysicsMaterial;

    // Start is called before the first frame update
    void Start()
    {
        Vector2 lDCorner = Camera.main.ViewportToWorldPoint(new Vector3(0, 0f, Camera.main.nearClipPlane));
        Vector2 rUCorner = Camera.main.ViewportToWorldPoint(new Vector3(1f, 1f, Camera.main.nearClipPlane));
        Vector2[] colliderpoints;

        GameObject bounderies = new GameObject("Bounderies");

        EdgeCollider2D upperEdge = new GameObject("UpperEdge").AddComponent<EdgeCollider2D>();
        upperEdge.transform.SetParent(bounderies.transform);
        colliderpoints = upperEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, rUCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, rUCorner.y);
        upperEdge.points = colliderpoints;
        upperEdge.sharedMaterial= BoundriesPhysicsMaterial;

        EdgeCollider2D lowerEdge = new GameObject("LowerEdge").AddComponent<EdgeCollider2D>();
        lowerEdge.transform.SetParent(bounderies.transform);
        colliderpoints = lowerEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        lowerEdge.points = colliderpoints;
        lowerEdge.sharedMaterial = BoundriesPhysicsMaterial;

        EdgeCollider2D leftEdge = new GameObject("LeftEdge").AddComponent<EdgeCollider2D>();
        leftEdge.transform.SetParent(bounderies.transform);
        colliderpoints = leftEdge.points;
        colliderpoints[0] = new Vector2(lDCorner.x, lDCorner.y);
        colliderpoints[1] = new Vector2(lDCorner.x, rUCorner.y);
        leftEdge.points = colliderpoints;
        leftEdge.sharedMaterial = BoundriesPhysicsMaterial;

        EdgeCollider2D rightEdge = new GameObject("RightEdge").AddComponent<EdgeCollider2D>();
        rightEdge.transform.SetParent(bounderies.transform);
        colliderpoints = rightEdge.points;
        colliderpoints[0] = new Vector2(rUCorner.x, rUCorner.y);
        colliderpoints[1] = new Vector2(rUCorner.x, lDCorner.y);
        rightEdge.points = colliderpoints;
        rightEdge.sharedMaterial = BoundriesPhysicsMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
