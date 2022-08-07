using UnityEngine;

/// <summary>
/// Clase asignada a las balas, ya sean del jugador o de los enemigos.
/// </summary>
public class Bullet : MonoBehaviour
{
    /// <summary>
    /// La velocidad de la bala.
    /// </summary>
    [SerializeField] float speed = 0;
    /// <summary>
    /// El daño que realiza la bala.
    /// </summary>
    [SerializeField] int damage = 0;
    /// <summary>
    /// Verdadero si la bala se destruye al chocar con algo. Falso si la atraviesa.
    /// </summary>
    [SerializeField] bool destroyOnGround = true;
    /// <summary>
    /// Verdadero si es una bala del enemigo. Falso si es del jugador.
    /// </summary>
    [SerializeField] bool enemyBullet = false;

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);       
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyOnGround)
        {
            if ((collision.gameObject.CompareTag("Ground")) || (collision.gameObject.CompareTag("MovingPlatform")) || (collision.gameObject.CompareTag("BoxTrap")))
            {
                gameObject.SetActive(false);
            }
        }
        
        if (enemyBullet)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                collision.gameObject.GetComponent<PlayerHealth>().Hurt(damage);
                
                gameObject.SetActive(false);
            }
        }

        else if (!enemyBullet)
        {
            if (collision.gameObject.CompareTag("Borders"))
            {
                gameObject.SetActive(false);
            }
        }
    }
}