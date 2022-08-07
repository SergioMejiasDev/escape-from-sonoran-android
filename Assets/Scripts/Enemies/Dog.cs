using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que controla las funciones principales del segundo jefe.
/// </summary>
public class Dog : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Velocidad de movimiento del enemigo.
    /// </summary>
    [Header("Movement")]
    readonly float speed = 2;
    /// <summary>
    /// Dirección de movimiento del enemigo en el eje X.
    /// </summary>
    int direction = 1;
    /// <summary>
    /// Posición inicial del enemigo.
    /// </summary>
    Vector2 startingPosition;

    /// <summary>
    /// El jugador.
    /// </summary>
    [Header("Attack")]
    GameObject player;
    /// <summary>
    /// El prefab de las balas que utilizará el enemigo.
    /// </summary>
    GameObject bullet;
    /// <summary>
    /// La posición donde se generarán las balas.
    /// </summary>
    [SerializeField] Transform cannon = null;
    /// <summary>
    /// Momento en el que se realizó el último disparo.
    /// </summary>
    float timeLastShoot;
    [SerializeField] float cadency = 1.5f;

    /// <summary>
    /// La salud máxima del enemigo.
    /// </summary>
    [Header("Health")]
    readonly float maxHealth = 4;
    /// <summary>
    /// La salud actual del enemigo.
    /// </summary>
    float health;
    /// <summary>
    /// La imagen de la barra de vida completa.
    /// </summary>
    [SerializeField] Image fullBattery = null;
    /// <summary>
    /// La explosión que aparece al morir el enemigo.
    /// </summary>
    [SerializeField] GameObject explosion = null;

    /// <summary>
    /// Componente Animator del enemigo.
    /// </summary>
    [Header("Components")]
    [SerializeField] Animator anim = null;

    #endregion

    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        startingPosition = transform.position;
    }

    void Update()
    {
        if (player.activeSelf)
        {
            Movement();

            ChangeDirection();

            Point();

            Animation();
        }

        else
        {
            anim.SetBool("IsIddle", true);
            direction = 0;
        }
    }

    /// <summary>
    /// Función que hace que el enemigo se mueva de un lado a otro de la pantalla.
    /// </summary>
    void Movement()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
    }

    /// <summary>
    /// Función que hace que el enemigo cambie de dirección cuando alcanza el extremo de la pantalla.
    /// </summary>
    void ChangeDirection()
    {
        if (transform.position.x > startingPosition.x + 13)
        {
            direction = -1;
        }

        else if (transform.position.x < startingPosition.x - 13)
        {
            direction = 1;
        }

        if (player.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-0.9f, 0.9f, 1f);
        }

        else
        {
            transform.localScale = new Vector3(0.9f, 0.9f, 1f);
        }
    }

    /// <summary>
    /// Función que hace que el enemigo apunte constantemente al jugador.
    /// </summary>
    void Point()
    {
        Vector3 dir = player.transform.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cannon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Time.time - timeLastShoot > cadency)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Función que hace que el enemigo dispare constantemente al jugador.
    /// </summary>
    void Shoot()
    {
        bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletEnemy");
        
        if (bullet != null)
        {
            bullet.transform.position = cannon.position;
            bullet.transform.rotation = cannon.rotation;
            bullet.SetActive(true);
        }

        timeLastShoot = Time.time;
    }

    /// <summary>
    /// Función que activa la animación del enemigo.
    /// </summary>
    void Animation()
    {
        anim.SetBool("IsWalking", true);
    }

    /// <summary>
    /// Función que se activa cada vez que el enemigo sufre daño.
    /// </summary>
    /// <param name="damage">Cantidad de daño recibido.</param>
    public void Hurt(int damage)
    {
        health -= damage;
        fullBattery.fillAmount -= (damage / maxHealth);

        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    /// <summary>
    /// Corrutina donde el enemigo muere y se inicia la transición entre escenas.
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        anim.SetTrigger("Dying");
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("BulletEnemy");

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetActive(false);
        }

        GetComponent<Dog>().enabled = false;
        yield return new WaitForSeconds(2);
        Instantiate(explosion, transform.position, transform.rotation);
        Boss2Manager.boss2Manager.BossDie();
        Destroy(gameObject);
    }
}