using UnityEngine;

public class DragPlatform2D_Parenting : MonoBehaviour
{
    private Vector3 offset;
    private Vector3 lastPosition;

    public Collider2D movementArea;

    void OnMouseDown()
    {
        offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    void OnMouseDrag()
    {
        Vector3 mouseWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorld.z = transform.position.z;

        Vector3 targetPosition = mouseWorld + offset;

        // Limite la plateforme à la zone
        Vector2 clamped = movementArea.ClosestPoint(targetPosition);
        transform.position = new Vector3(clamped.x, clamped.y, transform.position.z);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Si objet ou joueur touche par-dessus
        foreach (ContactPoint2D contact in collision.contacts)
        {
            if (contact.normal.y < -0.5f) // dessus
            {
                collision.transform.SetParent(transform);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Quand il quitte la plateforme
        if (collision.transform.parent == transform)
            collision.transform.SetParent(null);
    }
}