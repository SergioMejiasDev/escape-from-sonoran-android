using System.Collections;
using UnityEngine;

/// <summary>
/// Clase asignada a los objetos vacíos en las zonas de agua para que produzca efectos en el jugador.
/// </summary>
public class Water : MonoBehaviour
{
    /// <summary>
    /// La posición donde reaparecerá el jugador.
    /// </summary>
    [SerializeField] Transform respawn = null;
    /// <summary>
    /// Componente AudioSource del agua.
    /// </summary>
    [SerializeField] AudioSource audioSource = null;
    /// <summary>
    /// El prefab de la salpicadura que aparece al caer el jugador.
    /// </summary>
    [SerializeField] GameObject splatter = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            StartCoroutine(Respawn(collision.gameObject));
            Destroy(Instantiate(splatter, collision.gameObject.transform.position, collision.gameObject.transform.rotation), 0.5f);
        }
    }

    /// <summary>
    /// Corrutina que hace que el jugador reaparezca.
    /// </summary>
    /// <param name="player">El jugador.</param>
    /// <returns></returns>
    IEnumerator Respawn(GameObject player)
    {
        player.SetActive(false);
        yield return new WaitForSeconds(2);
        
        if (player != null)
        {
            player.SetActive(true);
            player.transform.position = respawn.position;
            player.transform.rotation = respawn.rotation;
        }

        player.GetComponent<PlayerHealth>().Hurt(2);
    }
}