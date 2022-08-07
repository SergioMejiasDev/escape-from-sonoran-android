using UnityEngine;

/// <summary>
/// Clase que hace que la cámara siga constantemente al jugador.
/// </summary>
public class CameraMovement : MonoBehaviour
{
    [SerializeField] Transform target = null;
    [Range(0.0f, 20.0f)]
    [SerializeField] float smoothing;
    [SerializeField] bool interpolation;
    Vector3 offset;

    void Start()
    {
        offset = transform.position - target.position;
    }

    void Update()
    {
        if (target != null)
        {
            Vector3 cameraPosition = target.position + offset;

            if (interpolation)
            {
                transform.position = Vector3.Lerp(transform.position, cameraPosition, smoothing * Time.deltaTime);
            }

            else
            {
                transform.position = cameraPosition;
            }
        }
    }
}