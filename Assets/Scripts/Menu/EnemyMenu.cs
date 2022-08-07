using UnityEngine;

/// <summary>
/// Clase encargada del movimiento de los enemigos que aparecen en el menú principal.
/// </summary>
public class EnemyMenu : MonoBehaviour
{
    /// <summary>
    /// Velocidad de movimiento de los enemigos.
    /// </summary>
    readonly float speed = 2;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);
    }
}