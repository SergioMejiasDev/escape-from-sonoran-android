using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que controla las funciones principales del primer jefe.
/// </summary>
public class EnemyBig : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// Velocidad de movimiento del enemigo.
    /// </summary>
    [Header("Movement")]
    readonly float speed = 1;
    /// <summary>
    /// Dirección de movimiento del enemigo en el eje X.
    /// </summary>
    int direction = 0;

    /// <summary>
    /// El jugador.
    /// </summary>
    [Header("Attack")]
    GameObject player;
    /// <summary>
    /// Las balas que utilizará el enemigo.
    /// </summary>
    GameObject bullet;
    /// <summary>
    /// El cañón del enemigo.
    /// </summary>
    [SerializeField] GameObject cannon = null;
    /// <summary>
    /// La posición donde se generarán las balas.
    /// </summary>
    [SerializeField] Transform shootPoint = null;
    /// <summary>
    /// Momento en el que se realizó el último disparo.
    /// </summary>
    float timeLastShoot;
    /// <summary>
    /// Cadencia de disparo del enemigo.
    /// </summary>
    readonly float cadency = 1.5f;

    /// <summary>
    /// La salud máxima del enemigo.
    /// </summary>
    [Header("Health")]
    readonly float maxHealth = 25;
    /// <summary>
    /// La salud actual del enemigo.
    /// </summary>
    float health;
    /// <summary>
    /// La imagen de la barra de salud completa.
    /// </summary>
    [SerializeField] Image fullBattery = null;
    /// <summary>
    /// La explosión que se genera al morir el enemigo.
    /// </summary>
    [SerializeField] GameObject explosion = null;

    /// <summary>
    /// Componente Animator del enemigo.
    /// </summary>
    [Header("Components")]
    [SerializeField] Animator anim = null;
    /// <summary>
    /// Componente EnemyBig del enemigo.
    /// </summary>
    EnemyBig enemyBigScript;

    #endregion

    void Start()
    {
        health = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        enemyBigScript = this;
    }

    void Update()
    {
        if (player.activeSelf)
        {
            if (player.transform.position.x < transform.position.x)
            {
                direction = -1;
            }

            else
            {
                direction = 1;
            }

            Movement();

            Point();

            Animation();
        }

        else
        {
            direction = 0;
            anim.SetBool("IsWalking", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if ((other.gameObject.CompareTag("BulletPlayer")))
        {
            other.gameObject.SetActive(false);
            health -= 1;
            fullBattery.fillAmount -= (1 / maxHealth);

            if (health <= 0)
            {
                StartCoroutine(Die());
            }
        }
    }

    /// <summary>
    /// Función que hace que el enemigo se mueva constantemente.
    /// </summary>
    void Movement()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);
    }

    /// <summary>
    /// Función que hace que el enemigo apunte constantemente al jugador con el cañon.
    /// </summary>
    void Point()
    {
        if (direction == -1)
        {
            transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            cannon.transform.localScale = new Vector3(1f, 1f, 1f);
        }

        else
        {
            transform.localScale = new Vector3(-0.75f, 0.75f, 1f);
            cannon.transform.localScale = new Vector3(-1f, -1f, 1f);
        }

        Vector3 dir = transform.position - player.transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        cannon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        if (Time.time - timeLastShoot > cadency)
        {
            Shoot();
        }
    }

    /// <summary>
    /// Función que hace que el enemigo dispare constantemente.
    /// </summary>
    void Shoot()
    {
        bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletEnemy");

        if (bullet != null)
        {
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = shootPoint.rotation;
            bullet.SetActive(true);
        }
        
        timeLastShoot = Time.time;
    }

    /// <summary>
    /// Función que activa la animación del enemigo.
    /// </summary>
    void Animation()
    {
        anim.SetBool("IsWalking", direction != 0);
    }

    /// <summary>
    /// Corrutina donde el enemigo muere y se activa la transición entre escenas.
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
        
        cannon.SetActive(false);
        enemyBigScript.enabled = false;
        yield return new WaitForSeconds(2);
        Instantiate(explosion, transform.position, transform.rotation);
        Boss1Manager.boss1Manager.BossDeath();
        Destroy(gameObject);
    }
}