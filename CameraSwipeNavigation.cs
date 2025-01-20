using UnityEngine;

public class CameraSwipeNavigation : MonoBehaviour
{
    public float dragSpeed = 0.1f;
    public float minBounds;
    public float maxBounds;
    public Touch touch;
    private Vector2 touchPosition;

    private Vector3 dragOrigin;
    private bool isScrolling = false;

    private void Update()
    {
        HandleTouch();
    }

    //Скроллинг сцены с помощью дельты между изначальной и текущей позицией касания
    private void HandleTouch()
    {
        if (Input.touchCount > 0)
        {
            if(Input.touchCount > 1)
            {
                touch = Input.GetTouch(1);
            }
            else
            {
                touch = Input.GetTouch(0);
            }

            touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    if (IsTouchingBackground(touchPosition))
                    {
                        Debug.Log("Started scrolling");
                        dragOrigin = Camera.main.ScreenToWorldPoint(touch.position);
                        isScrolling = true;
                    }
                    break;
                case TouchPhase.Moved when isScrolling:
                    Debug.Log("Moving camera...");
                    touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    Vector3 movement = dragOrigin - new Vector3(touchPosition.x, touchPosition.y, dragOrigin.z);
                    transform.position = ClampCamera(transform.position + movement);
                    break;
                case TouchPhase.Ended or TouchPhase.Canceled:
                    if (isScrolling)
                        isScrolling = false;
                    Debug.Log("Stopped scrolling");
                    break;
            }
        }
    }

    private Vector3 ClampCamera(Vector3 targetPosition)
    {
        float clampedX = Mathf.Clamp(targetPosition.x, minBounds, maxBounds);

        return new Vector3(clampedX, transform.position.y, targetPosition.z);
    }
    
    //Проверка, что пользователь нажал не на объект Dragable
    private bool IsTouchingBackground(Vector2 touchPosition)
    {
        Collider2D hitCollider = Physics2D.OverlapPoint(touchPosition);
        if(hitCollider == null) return true;
        else if (!hitCollider.gameObject.CompareTag("Dragable"))
        {
            Debug.Log("Not a dragable object");
            return true;
        }
        return false;
    }
}
