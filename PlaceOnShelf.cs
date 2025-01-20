using UnityEngine;

public class PlaceOnShelf : MonoBehaviour
{
    private Camera mainCamera;
    private DragAndMove DragAndMoveComponent;
    private GameObject currentShelf;
    private Transform tf;
    private Vector3 originalScale;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public Color highlightColor = new Color(0.6f, 0.6f, 0.6f, 1);
    private Color originalColor;
    public bool isPlaced = false;
    private bool isPlacing = false;
    private float[] scaleModifier = { 0.9f, 0.8f, 0.7f };

    private void Start()
    {
        mainCamera = Camera.main;
        tf = GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        DragAndMoveComponent = GetComponent<DragAndMove>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        originalScale = tf.localScale;
    }

    //Помещение предмета на полку(триггер)
    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector2 touchPosition = mainCamera.ScreenToWorldPoint(touch.position);
            if(touch.phase == TouchPhase.Began && IsTouchingObject(touchPosition))
            {
                isPlaced = false;
                tf.localScale = originalScale;
            }
            if((touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled) && currentShelf != null)
            {
                isPlaced = true;
                spriteRenderer.color = originalColor;
                rb.bodyType = RigidbodyType2D.Kinematic;
                isPlacing = true;
            }
        }
        //Плавный лерп
        if(isPlacing)
        {
            Vector3 newPosition = new Vector3(currentShelf.transform.position.x, currentShelf.transform.position.y, 0);
            float newScale = originalScale.x * scaleModifier[currentShelf.layer - 6];
            tf.localScale = Vector3.Lerp(tf.localScale, new Vector3(newScale, newScale, newScale), 0.1f);
            tf.position = Vector3.Lerp(tf.position, newPosition, 0.1f);
            if (Vector3.Distance(tf.position, newPosition) < 0.01f && Vector3.Distance(tf.localScale, new Vector3(newScale, newScale, newScale)) < 0.01f)
            {
                isPlacing = false;
                rb.bodyType = RigidbodyType2D.Static;
            }
        }
    }

    //Проверка на попадание в нужный триггер
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shelf" && DragAndMoveComponent.isDragging)
        {
            spriteRenderer.color = highlightColor;
            currentShelf = collision.gameObject;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shelf" && DragAndMoveComponent.isDragging)
        {
            if(!isPlaced)
            {
                spriteRenderer.color = highlightColor;
            }
            currentShelf = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Shelf")
        {
            spriteRenderer.color = originalColor;
            currentShelf = null;
        }
    }

    private bool IsTouchingObject(Vector2 touchPosition)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);
        return hitCollider != null && hitCollider.gameObject == gameObject;
    }
}
