using UnityEngine;

/// <summary>
/// Clase asociada al enemigo tipo 3 (amarillo, vuela y dispara).
/// </summary>
public class EnemyClass3 : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Velocidad de movimiento del enemigo.
    /// </summary>
    [Header("Movement")]
    readonly float speed = 4;
    /// <summary>
    /// La dirección de movimiento del enemigo en el eje X.
    /// </summary>
    public int direction = 1;
    /// <summary>
    /// Verdadero si el enemigo no puede moverse de su posición. Falso si se puede mover.
    /// </summary>
    [SerializeField] bool dontMove = false;
    /// <summary>
    /// Posición inicial del enemigo.
    /// </summary>
    Vector3 startingPosition;
    /// <summary>
    /// Distancia máxima a la que puede moverse el enemigo desde la posición inicial antes de darse la vuelta.
    /// </summary>
    [SerializeField] float movementDistance = 100;

    /// <summary>
    /// Rango de ataque del enemigo.
    /// </summary>
    [Header("Attack")]
    [SerializeField] float attackRange = 0.0f;
    /// <summary>
    /// El jugador.
    /// </summary>
    GameObject player;
    /// <summary>
    /// Las balas que utilizará el enemigo.
    /// </summary>
    GameObject bullet;
    /// <summary>
    /// El brazo del enemigo.
    /// </summary>
    [SerializeField] GameObject arm = null;
    /// <summary>
    /// La posición donde se generarán las balas.
    /// </summary>
    [SerializeField] Transform shootPoint = null;
    /// <summary>
    /// Momento en el que se realizó el último disparo.
    /// </summary>
    float timeLastShoot;
    /// <summary>
    /// La cadencia de disparo del enemigo.
    /// </summary>
    [SerializeField] float cadency = 1; 

    /// <summary>
    /// La salud del enemigo.
    /// </summary>
    [Header("Health")]
    [SerializeField] int health = 5;
    /// <summary>
    /// La explosión que se genera al morir el enemigo.
    /// </summary>
    GameObject explosion;
    
    #endregion

    void Start()
    {
        startingPosition = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        if (direction == 1)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            shootPoint.localRotation = Quaternion.Euler(0, 0, 0);
        }

        else
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
            shootPoint.localRotation = Quaternion.Euler(0, 0, 180);
        }

        if (player.activeSelf && (Vector3.Distance(transform.position, player.transform.position) < attackRange))
        {
            LookAtPlayer();
        }

        else
        {
            if (!dontMove)
            {
                Movement();
            }
        }
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
    /// Función que hace que el enemigo se detenga y mire al jugador.
    /// </summary>
    void LookAtPlayer()
    {
        if (player.transform.position.x < transform.position.x)
        {
            direction = -1;
            Vector3 dir = transform.position - player.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        else if (player.transform.position.x > transform.position.x)
        {
            direction = 1;
            Vector3 dir = player.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        Attack();
    }

    /// <summary>
    /// Función que hace que el enemigo ataque constantemente.
    /// </summary>
    void Attack()
    {
        if (Time.time - timeLastShoot > cadency)
        {
            timeLastShoot = Time.time;

            bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletEnemy");

            if (bullet != null)
            {
                bullet.transform.position = shootPoint.position;
                bullet.transform.rotation = shootPoint.rotation;
                bullet.SetActive(true);
            }
        }
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
}