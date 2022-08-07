using System.Collections;
using UnityEngine;

/// <summary>
/// Clase asignada a la bola de energía del jefe final.
/// </summary>
public class EnergyBall : MonoBehaviour
{
    /// <summary>
    /// Velocidad de la bola de energía.
    /// </summary>
    readonly float speed = 10;

    void OnEnable()
    {
        StartCoroutine(DestroyBall());
    }

    void Update()
    {
        transform.Translate(Vector2.right * speed * Time.deltaTime);      
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            collision.gameObject.SetActive(false);
        }

        else if (collision.gameObject.CompareTag("Player"))
        {
            Boss3Manager.boss3Manager.StartExplosion(true);
        }
    }

    /// <summary>
    /// Corrutina que hace que la bola se destruya pasados unos segundos.
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyBall()
    {
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }
}