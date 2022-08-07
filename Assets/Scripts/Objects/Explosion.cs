using System.Collections;
using UnityEngine;

/// <summary>
/// Clase asignada a las explosiones que se generan al morir los enemigos.
/// </summary>
public class Explosion : MonoBehaviour
{
    /// <summary>
    /// Componente AudioSource de la explosión.
    /// </summary>
    [SerializeField] AudioSource audioSource = null;

    void OnEnable()
    {
        audioSource.Play();
        StartCoroutine(DestroyExplosion());
    }

    /// <summary>
    /// Corrutina donde la explosión se destruye.
    /// </summary>
    /// <returns></returns>
    IEnumerator DestroyExplosion()
    {
        yield return new WaitForSeconds(0.7f);
        gameObject.SetActive(false);
    }
}