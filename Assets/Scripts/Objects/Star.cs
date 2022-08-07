using System.Collections;
using UnityEngine;

/// <summary>
/// Clase asignada a la estrella que lanza el jefe final.
/// </summary>
public class Star : MonoBehaviour
{
    /// <summary>
    /// Velocidad de rotación de la estrella.
    /// </summary>
    float rotationSpeed;
    /// <summary>
    /// Velocidad de disparo de la estrella.
    /// </summary>
    float attackSpeed;
    /// <summary>
    /// Máximo de disparos de la estrella antes de cambiar de dirección.
    /// </summary>
    int maxShoots;
    /// <summary>
    /// Los diferentes cañones de la estrella.
    /// </summary>
    [SerializeField] GameObject[] cannons = null;

    void Start()
    {
        StartCoroutine(Move());
    }

    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Corrutina que hace que la estrella se mueva hasta el centro de la pantalla.
    /// </summary>
    /// <returns></returns>
    IEnumerator Move()
    {
        while (transform.position.x > -12)
        {
            transform.Translate(Vector2.right * 5 * Time.deltaTime);
            yield return null;
        }

        StartCoroutine(Shoot());
    }

    /// <summary>
    /// Corrutina que hace que la gente empiece a disparar.
    /// </summary>
    /// <returns></returns>
    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(3);

        int currentShoots = 0;

        attackSpeed = 1.0f;

        maxShoots = 5;

        rotationSpeed = 25;

        while (currentShoots < maxShoots)
        {
            foreach (GameObject shootPoint in cannons)
            {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet4");

                if (bullet != null)
                {
                    bullet.transform.position = shootPoint.transform.position;
                    bullet.transform.rotation = shootPoint.transform.rotation;
                    bullet.SetActive(true);
                }
            }

            currentShoots += 1;

            yield return new WaitForSeconds(attackSpeed);
        }

        currentShoots = 0; 

        attackSpeed = 0.5f;

        maxShoots = 15;

        rotationSpeed = -25;

        while (currentShoots < maxShoots)
        {
            foreach (GameObject shootPoint in cannons)
            {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet4");

                if (bullet != null)
                {
                    bullet.transform.position = shootPoint.transform.position;
                    bullet.transform.rotation = shootPoint.transform.rotation;
                    bullet.SetActive(true);
                }
            }

            currentShoots += 1;

            yield return new WaitForSeconds(attackSpeed);
        }

        currentShoots = 0;

        attackSpeed = 0.25f;

        maxShoots = 60;

        rotationSpeed = 25;

        while (currentShoots < maxShoots)
        {
            foreach (GameObject shootPoint in cannons)
            {
                GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet4");

                if (bullet != null)
                {
                    bullet.transform.position = shootPoint.transform.position;
                    bullet.transform.rotation = shootPoint.transform.rotation;
                    bullet.SetActive(true);
                }
            }

            currentShoots += 1;

            yield return new WaitForSeconds(attackSpeed);
        }

        yield return new WaitForSeconds(2);
        GameObject boss = GameObject.FindGameObjectWithTag("Boss");
        boss.GetComponent<FinalBoss>().SelectAttack();
        
        GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
        
        if (explosion != null)
        {
            explosion.SetActive(true);
            explosion.transform.position = transform.position;
        }

        Destroy(gameObject);
    }
}