using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Clase que permite el movimiento de los sticks en la pantalla.
/// </summary>
public class JoyStickController : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [SerializeField] RectTransform background = null;
    [SerializeField] RectTransform stick = null;
    public float horizontal = 0;
    public float vertical = 0;

    Vector2 pointPosition;

    void OnDisable()
    {
        ResetPosition();
    }

    void Update()
    {
        horizontal = pointPosition.x;
        vertical = pointPosition.y;
    }

    public void OnDrag(PointerEventData eventData)
    {
        pointPosition = new Vector2((eventData.position.x - background.position.x) / ((background.rect.size.x - stick.rect.size.x) / 2), (eventData.position.y - background.position.y) / ((background.rect.size.y - stick.rect.size.y) / 2));

        pointPosition = pointPosition.magnitude > 1.0f ? pointPosition.normalized : pointPosition;

        stick.transform.position = new Vector2(pointPosition.x * (background.rect.size.x - stick.rect.size.x) + background.position.x, pointPosition.y * (background.rect.size.y - stick.rect.size.y) + background.position.y);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetPosition();
    }

    /// <summary>
    /// Función que devuelve el joystick a la posición inicial después de levantar el dedo de la pantalla.
    /// </summary>
    public void ResetPosition()
    {
        pointPosition = new Vector2(0.0f, 0.0f);

        stick.transform.position = background.position;
    }
}