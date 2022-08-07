using System.Collections;
using UnityEngine;

/// <summary>
/// Clase asignada a los interruptores de las puertas.
/// </summary>
public class Switch : MonoBehaviour
{
    /// <summary>
    /// El interruptor apagado.
    /// </summary>
    [SerializeField] GameObject offSwitch = null;
    /// <summary>
    /// La puerta asignada al interruptor.
    /// </summary>
    [SerializeField] GameObject door = null;
    /// <summary>
    /// Verdadero si el interruptor puede reiniciarse.
    /// </summary>
    [SerializeField] bool restart = false;
    /// <summary>
    /// Componente AudioSource del interruptor.
    /// </summary>
    [SerializeField] AudioSource audioSource = null;
    /// <summary>
    /// Componente AudioSource de la puerta.
    /// </summary>
    AudioSource doorAudioSource;
    /// <summary>
    /// Componente BoxCollider2D del interruptor.
    /// </summary>
    [SerializeField] BoxCollider2D switchCollider = null;

    void Start()
    {
        doorAudioSource = door.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            collision.gameObject.SetActive(false);
            StartCoroutine(SwitchOn());
        }
    }

    /// <summary>
    /// Corrutina que activa el interruptor y abre la puerta.
    /// </summary>
    /// <returns></returns>
    IEnumerator SwitchOn()
    {
        offSwitch.SetActive(false);
        switchCollider.enabled = false;
        audioSource.Play();
        yield return new WaitForSeconds(1);
        doorAudioSource.Play();
        yield return new WaitForSeconds(1);
        door.SetActive(false);
        
        if (restart)
        {
            StartCoroutine(SwitchRestart());
        }
    }

    /// <summary>
    /// Corrutina que resetea el interruptor y la puerta tras unos segundos.
    /// </summary>
    /// <returns></returns>
    IEnumerator SwitchRestart()
    {
        yield return new WaitForSeconds(2);
        door.SetActive(true);
        doorAudioSource.Play();
        yield return new WaitForSeconds(2);
        offSwitch.SetActive(true);
        audioSource.Play();
        switchCollider.enabled = true;
    }
}