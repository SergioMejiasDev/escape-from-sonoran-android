using UnityEngine;

/// <summary>
/// Clase asignada a las plataformas móviles.
/// </summary>
public class MovingPlatform : MonoBehaviour
{
    /// <summary>
    /// Verdadero si las plataformas se mueven en vertical. Falso si se mueven en horizontal.
    /// </summary>
    public bool isVertical;
    /// <summary>
    /// La dirección de movimiento de la plataforma en el eje asignado.
    /// </summary>
    public int direction = 1;
    /// <summary>
    /// La posición inicial de la plataforma.
    /// </summary>
    Vector3 startingPosition;
    /// <summary>
    /// La velocidad de movimiento de la plataforma.
    /// </summary>
    [SerializeField] float speed = 0;
    /// <summary>
    /// La distancia máxima a la que se puede mover la plataforma desde la posición inicial.
    /// </summary>
    [SerializeField] float movementDistance = 0;
    
    void Start()
    {
        startingPosition = transform.position;
    }

    void Update()
    {
        if (!isVertical)
        {
            transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
        }

        else
        {
            transform.Translate(Vector2.up * speed * direction * Time.deltaTime);
        }

        ChangeDirection();
    }

    /// <summary>
    /// Función que hace que la plataforma cambie de dirección.
    /// </summary>
    void ChangeDirection()
    {
        if (!isVertical)
        {
            if (transform.position.x > startingPosition.x + movementDistance)
            {
                direction = -1;
            }

            else if (transform.position.x < startingPosition.x - movementDistance)
            {
                direction = 1;
            }
        }
        
        else
        {
            if (transform.position.y > startingPosition.y + movementDistance)
            {
                direction = -1;
            }

            else if (transform.position.y < startingPosition.y - movementDistance)
            {
                direction = 1;
            }
        }
    }
}