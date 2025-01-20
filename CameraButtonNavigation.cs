using UnityEngine;

//Простое перемещение камеры с помощью кнопок влево и вправо по позициям заданным через массив. Устаревшая функция, вместо нее используется CameraSwipeNavigation
public class CameraButtonNavigation : MonoBehaviour
{
    public int[] positions;
    public int currentPosition = 0;
    bool isMoving = false;

    void Update()
    {
        if (isMoving){
            Vector3 newPosition = new Vector3(positions[currentPosition], Camera.main.transform.position.y, Camera.main.transform.position.z);
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, newPosition, 0.1f);
            if (Vector3.Distance(Camera.main.transform.position, newPosition) < 0.01f){
                isMoving = false;
            }
        }
    }

    public void MoveLeft()
    {
        if (currentPosition > 0){
            currentPosition--;
            isMoving = true;
        }
    }

    public void MoveRight()
    {
        if (currentPosition < positions.Length - 1){
            currentPosition++;
            isMoving = true;
        }
    }
}
