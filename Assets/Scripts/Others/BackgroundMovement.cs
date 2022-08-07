using UnityEngine;

/// <summary>
/// Clase encargada del movimiento del fondo en la batalla con el jefe final.
/// </summary>
public class BackgroundMovement : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    [SerializeField] private float end = 0;
    [SerializeField] private float begin = 0;
    
    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
        
        if (transform.position.x <= end)
        {
            Vector2 startPosition = new Vector2(begin, transform.position.y);
            transform.position = startPosition;
        }
    }
}