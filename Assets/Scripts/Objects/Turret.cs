using UnityEngine;

/// <summary>
/// Clase asignada a las torretas.
/// </summary>
public class Turret : MonoBehaviour
{
    /// <summary>
    /// Las balas de la torreta.
    /// </summary>
    GameObject bullet;
    /// <summary>
    /// La posición donde se generan las balas.
    /// </summary>
    [SerializeField] Transform shootPoint = null;
    /// <summary>
    /// Momento en el que se realizó el último disparo.
    /// </summary>
    float timeLastShoot;
    /// <summary>
    /// Cadencia de disparo de la torreta.
    /// </summary>
    public float cadency;

    void Update()
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Water"))
        {
            AudioSource audioSource = collision.gameObject.GetComponent<AudioSource>();
            audioSource.Play();
            Destroy(gameObject);
        }
    }
}