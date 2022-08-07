using System.Collections;
using UnityEngine;

/// <summary>
/// Clase asociada a los enemigos tipo 2 (verdes, caminan y atacan con la espada).
/// </summary>
public class EnemyClass2 : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Velocidad de movimiento del enemigo.
    /// </summary>
    [Header("Movement")]
    readonly float speed = 3;
    /// <summary>
    /// Dirección de movimiento del enemigo en el eje X.
    /// </summary>
    public int direction = 1;
    /// <summary>
    /// Posición inicial del enemigo.
    /// </summary>
    Vector3 startingPosition;
    /// <summary>
    /// Distancia máxima que puede recorrer el enemigo desde la posición inicial antes de darse la vuelta.
    /// </summary>
    [SerializeField] float movementDistance = 100;

    /// <summary>
    /// Momento en el que se ha realizado el último ataque.
    /// </summary>
    [Header("Attack")]
    float timeLastAttack;
    /// <summary>
    /// Cadencia de ataque del enemigo.
    /// </summary>
    readonly float cadency = 0.85f;
    /// <summary>
    /// El jugador.
    /// </summary>
    GameObject player;
    /// <summary>
    /// El punto de contacto del ataque del enemigo.
    /// </summary>
    [SerializeField] GameObject attackPoint = null;

    /// <summary>
    /// La salud del enemigo.
    /// </summary>
    [Header("Health")]
    [SerializeField] int health = 5;
    /// <summary>
    /// La explosión que aparece al morir el enemigo.
    /// </summary>
    GameObject explosion;

    /// <summary>
    /// Componente Animator del enemigo.
    /// </summary>
    [Header("Components")]
    [SerializeField] Animator anim = null;
    /// <summary>
    /// Componente AudioSource del enemigo.
    /// </summary>
    [SerializeField] AudioSource audioSource = null;

    #endregion

    void Start()
    {
        startingPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        attackPoint.SetActive(false);
    }

    void Update()
    {
        if (direction == 1)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }

        else
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
        }

        if (player.activeSelf && (Vector3.Distance(transform.position, player.transform.position) < 3))
        {
            Attack();
        }

        else
        {
            Movement();
        }

        Animation();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("BulletPlayer"))
        {
            other.gameObject.SetActive(false);
            health -= 1;

            if (health <= 0)
            {
                explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
                
                if (explosion != null)
                {
                    explosion.SetActive(true);
                    explosion.transform.position = transform.position;
                    explosion.transform.rotation = transform.rotation;
                }

                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Función que hace que el enemigo se mueva constantemente.
    /// </summary>
    void Movement()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

        ChangeDirection();
    }

    /// <summary>
    /// Función que hace que el enemigo ataque constantemente.
    /// </summary>
    void Attack()
    {
        if (player.transform.position.x < transform.position.x)
        {
            direction = -1;
        }

        else if (player.transform.position.x > transform.position.x)
        {
            direction = 1;
        }

        if (Time.time - timeLastAttack > cadency)
        {
            timeLastAttack = Time.time;
            attackPoint.SetActive(true);
            audioSource.Play();
            StartCoroutine(DestroyAttack());
        }
    }

    /// <summary>
    /// Función que activa la animación del enemigo.
    /// </summary>
    void Animation()
    {
        anim.SetBool("Attack", player.activeSelf && (Vector3.Distance(transform.position, player.transform.position) < 3));
    }

    /// <summary>
    /// Función que hace que el enemigo cambie de dirección.
    /// </summary>
    void ChangeDirection()
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

    /// <summary>
    /// Corrutina que hace que el punto de contacto del arma del enemigo desaparezca.
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyAttack()
    {
        yield return new WaitForSeconds(0.1f);
        attackPoint.SetActive(false);
    }
}