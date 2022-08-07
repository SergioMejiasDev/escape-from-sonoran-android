using System.Collections;
using UnityEngine;

/// <summary>
/// Clase asignada a los misiles que lanza el jefe final.
/// </summary>
public class Missile : MonoBehaviour
{
    /// <summary>
    /// Velocidad de movimiento de los misiles.
    /// </summary>
    readonly float speed = 25;

    void OnEnable()
    {
        StartCoroutine(DestroyMissile());
    }

    void Update()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
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
    /// Corrutina que destruye el misil pasados unos segundos.
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyMissile()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
    }
}