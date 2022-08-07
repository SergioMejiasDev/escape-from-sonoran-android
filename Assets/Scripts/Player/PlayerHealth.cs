using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que contiene las funciones relacionadas con la salud del jugador.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// La salud máxima del jugador.
    /// </summary>
    [Header("Health")]
    [SerializeField] float maxHealth = 10;
    /// <summary>
    /// La salud del jugador.
    /// </summary>
    float health;
    /// <summary>
    /// Imagen de la barra de salud completa.
    /// </summary>
    [SerializeField] Image fullBattery = null;
    /// <summary>
    /// Imagen que aparece en la pantalla cuando el jugador sufre daño.
    /// </summary>
    [SerializeField] Image hurtImage = null;
    /// <summary>
    /// Sonido que se reproduce cuando el jugador sufre algún daño.
    /// </summary>
    [SerializeField] AudioClip hurtSound = null;
    /// <summary>
    /// Sonido que se reproduce al coger una batería.
    /// </summary>
    [SerializeField] AudioClip batterySound = null;
    /// <summary>
    /// Verdadero si el jugador acaba de ser herido.
    /// </summary>
    bool isHurt;
    /// <summary>
    /// Verdadero si el jugadador está en el Capítulo 2.
    /// </summary>
    [SerializeField] bool isType2 = false;
    /// <summary>
    /// Explosión que aparece al morir el jugador.
    /// </summary>
    GameObject explosion;

    /// <summary>
    /// Componente AudioSource del jugador.
    /// </summary>
    [Header("Components")]
    [SerializeField] AudioSource audioSource = null;
    /// <summary>
    /// Componente Animator del jugador.
    /// </summary>
    [SerializeField] Animator anim = null;
    #endregion

    void Start()
    {
        health = maxHealth;
    }

    void Update()
    {
        if (isHurt)
        {
            hurtImage.color = new Color(1.0f, 0.0f, 0.0f, 0.2f);
            audioSource.clip = hurtSound;
            audioSource.Play();
            isHurt = false;
        }

        else
        {
            hurtImage.color = Color.Lerp(hurtImage.color, Color.clear, 10.0f * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Hurt(1);
        }

        else if (collision.gameObject.CompareTag("Boss"))
        {
            Hurt(1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Sword"))
        {
            Hurt(1);
        }

        else if (collision.gameObject.CompareTag("Battery"))
        {
            RestoreHealth(5);
            audioSource.clip = batterySound;
            audioSource.Play();
            Destroy(collision.gameObject);
        }

        else if (collision.gameObject.CompareTag("CheckPoint"))
        {
            Destroy(collision.gameObject);
            GameManager.gameManager.CheckPoint(collision.gameObject.transform.position);
        }
    }

    /// <summary>
    /// Función que se activa cuando el jugador recibe algún daño.
    /// </summary>
    /// <param name="damage">La cantidad de daño recibido.</param>
    public void Hurt(int damage)
    {
        if (health > 0)
        {
            health -= damage;
            fullBattery.fillAmount -= (damage / maxHealth);
            isHurt = true;

            if (health <= 0)
            {
                health = 0;

                if (GetComponent<Player>() != null)
                {
                    StartCoroutine(Die());
                }

                else
                {
                    StartCoroutine(DieFlying());
                }
            }
        }
    }

    /// <summary>
    /// Function we call when the player restores health.
    /// </summary>
    /// <param name="restoredHealth">Amount of health that is restored.</param>
    void RestoreHealth(float restoredHealth)
    {
        health += restoredHealth;

        fullBattery.fillAmount = (health / maxHealth);
        {
            if (health > maxHealth)
            {
                health = maxHealth;
                fullBattery.fillAmount = 1;
            }
        }
    }

    /// <summary>
    /// Función encargada de restaurar al jugador después del Game Over.
    /// </summary>
    public void RestorePlayer()
    {
        gameObject.SetActive(true);

        if (GetComponent<Player>() != null)
        {
            GetComponent<Player>().enabled = true;
            GetComponent<Player>().arm.SetActive(true);
            GetComponent<Player>().inPlatform = false;
        }

        else
        {
            GetComponent<FlyingPlayer>().enabled = true;
            GetComponent<FlyingPlayer>().arm.SetActive(true);
        }

        RestoreHealth(maxHealth);
    }

    /// <summary>
    /// Corrutina que se inicia al morir el jugador yendo a pie.
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        if (!isType2)
        {
            anim.SetTrigger("Dying1");
        }

        else if (isType2)
        {
            anim.SetTrigger("Dying2");
        }

        GetComponent<Player>().arm.SetActive(false);
        GetComponent<Player>().enabled = false;

        yield return new WaitForSeconds(2);

        explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
        
        if (explosion != null)
        {
            explosion.SetActive(true);
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
        }

        GameManager.gameManager.GameOver();
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Corrutina que se inicia al morir el jugador mientras vuela.
    /// </summary>
    /// <returns></returns>
    IEnumerator DieFlying()
    {
        anim.SetTrigger("Dying3");
        GetComponent<FlyingPlayer>().arm.SetActive(false);
        GetComponent<FlyingPlayer>().enabled = false;
        yield return new WaitForSeconds(2);
        explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
        
        if (explosion != null)
        {
            explosion.SetActive(true);
            explosion.transform.position = transform.position;
            explosion.transform.rotation = transform.rotation;
        }

        GameManager.gameManager.GameOver();
        gameObject.SetActive(false);
    }
}