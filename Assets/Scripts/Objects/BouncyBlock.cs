using UnityEngine;

/// <summary>
/// Clase que permite que el jugador pueda rebotar sobre ciertas superficies.
/// </summary>
public class BouncyBlock : MonoBehaviour
{
    /// <summary>
    /// Componente AudioSource.
    /// </summary>
    [SerializeField] AudioSource audioSource = null;
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
        }
    }
}