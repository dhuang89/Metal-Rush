using UnityEngine;
using System.Collections;
[RequireComponent (typeof (BoxCollider2D))]

public class Controller2D : MonoBehaviour {
    BoxCollider2D collider;
    playerOrigins origins;
    public LayerMask collisionMask;
    const float skinWidth = 0.015f;
    public int horizontalRayCount = 4;
    public int verticalRayCount = 4;
    float horizontalRaySpacing;
    float verticalRaySpacing;


	// Use this for initialization
	void Start () {
        collider = GetComponent<BoxCollider2D>();
        calculateRaySpacing();
    }

    public void move(Vector3 velocity)
    {
        updatePlayerOrigins();
        if (velocity.x != 0)
        {
            horizontalCollisions(ref velocity);
        }
        if (velocity.y != 0)
        {
            verticalCollisions(ref velocity);
        }

        transform.Translate(velocity);

    }

    void verticalCollisions(ref Vector3 velocity)
    {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++)
        {
            Vector2 rayOrigin = (directionY == -1) ? origins.bottomLeft : origins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask); 
            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit)
            {
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance; 
            }
        }
    }

    void horizontalCollisions(ref Vector3 velocity)
    {
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            Vector2 rayOrigin = (directionX == -1) ? origins.bottomLeft : origins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);
            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);
            if (hit)
            {
                velocity.x = (hit.distance - skinWidth) * directionX;
                rayLength = hit.distance;
            }
        }
    }

    struct playerOrigins
    {
        public Vector2 topLeft, topRight;
        public Vector2 bottomLeft, bottomRight;
    }

    void updatePlayerOrigins()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);
        origins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        origins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        origins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        origins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    void calculateRaySpacing()
    {
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);
        horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
        verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);

        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }
	
}
