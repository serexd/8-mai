using UnityEngine;

public class DragPlatform : MonoBehaviour
{
    private Transform dragging = null;
    private Vector3 offset;
    [SerializeField] private GameObject MovableZone;
    [SerializeField] private GameObject Square;

    void Update()
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("MovablePlatform"))
            {
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.PositiveInfinity, LayerMask.GetMask("Movable"));
                if (hit)
                {
                    dragging = hit.transform;
                    offset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                }



            }
        }




       if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.PositiveInfinity, LayerMask.GetMask("Movable"));
            if (hit)
            {
                dragging = hit.transform;
                offset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
        }
           
        
       else if (Input.GetMouseButtonUp(0))
        {
            dragging = null;
        }

       if(dragging != null)
        {
            dragging.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
        }

    }
}
