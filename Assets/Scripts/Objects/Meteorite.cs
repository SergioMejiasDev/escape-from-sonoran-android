using System.Collections;
using UnityEngine;

/// <summary>
/// Clase asignada a los meteoritos que lanza el jefe final.
/// </summary>
public class Meteorite : MonoBehaviour
{
    /// <summary>
    /// Velocidad de los meteoritos.
    /// </summary>
    readonly float speed = 10;

    void OnEnable()
    {
        StartCoroutine(DestroyMeteorite());
    }

    void Update()
    {
        transform.Translate(Vector2.down * speed * Time.deltaTime);      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerHealth>().Hurt(1);

            GameObject explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");

            if (explosion != null)
            {
                explosion.SetActive(true);
                explosion.transform.position = transform.position;
                explosion.transform.rotation = transform.rotation;
            }

            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Corrutina que destruye los meteoritos pasados unos segundos.
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyMeteorite()
    {
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }
}