using System.Collections;
using UnityEngine;

/// <summary>
/// Clase asignada a las cajas que rebotan en el nivel 2.
/// </summary>
public class BoxTrap : MonoBehaviour
{
    /// <summary>
    /// La posición donde reaparecerán las cajas si se destruyen.
    /// </summary>
    [SerializeField] Transform respawn = null;
    /// <summary>
    /// La explosión que aparece al destruirse las cajas.
    /// </summary>
    GameObject explosion;
    /// <summary>
    /// Componente SpriteRenderer de la caja.
    /// </summary>
    [SerializeField] SpriteRenderer sr = null;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            sr.enabled = false;

            explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
            
            if (explosion != null)
            {
                explosion.SetActive(true);
                explosion.transform.position = collision.gameObject.transform.position;
                explosion.transform.rotation = collision.gameObject.transform.rotation;
            }

            StartCoroutine(Respawn(collision.gameObject));
            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Corrutina donde el jugador reaparece tras ser golpeado por la caja.
    /// </summary>
    /// <param name="player">El jugador.</param>
    /// <returns></returns>
    IEnumerator Respawn(GameObject player)
    {
        yield return new WaitForSeconds(2);

        if (player != null)
        {
            player.SetActive(true);
            player.transform.position = respawn.position;
            player.transform.rotation = respawn.rotation;
            player.GetComponent<PlayerHealth>().Hurt(2);
            Destroy(gameObject);
        }
    }
}