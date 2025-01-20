using UnityEngine;

public class DragAndMove : MonoBehaviour
{
    private Camera mainCamera;
    private Rigidbody2D rb;
    private Transform tf;
    public Touch touch;
    private Vector2 touchPosition;
    public bool isDragging = false;
    public AudioClip pickUpSound;
    public AudioClip putDownSound;

    private void Start()
    {
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        tf = GetComponent<Transform>();
    }

    //Drag and move. Объект следует за позицией касания
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            touchPosition = mainCamera.ScreenToWorldPoint(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchingObject(touchPosition))
                    {
                        isDragging = true;
                        rb.bodyType = RigidbodyType2D.Kinematic;
                        tf.rotation = Quaternion.identity;
                        GetComponent<AudioSource>().PlayOneShot(pickUpSound);
                        Debug.Log("Dragging started at " + touchPosition);
                    }
                    break;
                case TouchPhase.Stationary or TouchPhase.Moved when isDragging:
                    rb.MovePosition(touchPosition);
                    break;
                case TouchPhase.Canceled or TouchPhase.Ended:
                    if (isDragging)
                    {
                        isDragging = false;
                        if(GetComponent<PlaceOnShelf>().isPlaced)
                        {
                            GetComponent<AudioSource>().PlayOneShot(putDownSound);
                        }
                        else
                        {
                            rb.bodyType = RigidbodyType2D.Dynamic;
                        }
                    }
                    break;
            }
        }
    }

    private bool IsTouchingObject(Vector2 touchPosition)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);
        return hitCollider != null && hitCollider.gameObject == gameObject;
    }
}
