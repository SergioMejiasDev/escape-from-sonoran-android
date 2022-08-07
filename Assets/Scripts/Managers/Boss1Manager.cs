using System.Collections;
using UnityEngine;

/// <summary>
/// Clase con las funciones principales de la primera batalla.
/// </summary>
public class Boss1Manager : MonoBehaviour
{
    #region Variables

    public static Boss1Manager boss1Manager;
    
    /// <summary>
    /// El botón rojo del ascensor.
    /// </summary>
    [Header("Elevator")]
    [SerializeField] GameObject redButton = null;
    /// <summary>
    /// El botón verde del ascensor.
    /// </summary>
    [SerializeField] GameObject greenButton = null;
    /// <summary>
    /// Componente Animator del ascensor.
    /// </summary>
    [SerializeField] Animator elevatorAnim;
    /// <summary>
    /// Componente AudioSource del ascensor.
    /// </summary>
    [SerializeField] AudioSource elevatorAudio;
    /// <summary>
    /// Sonido que se reproduce cuando llega el ascensor.
    /// </summary>
    [SerializeField] AudioClip ring = null;
    /// <summary>
    /// Sonido que se reproduce al abrirse las puertas.
    /// </summary>
    [SerializeField] AudioClip doors = null;
    
    /// <summary>
    /// Panel que indica que tenemos una nueva arma.
    /// </summary>
    [Header("Scene Transition")]
    public GameObject gunPanel;

    /// <summary>
    /// Prefab de las baterías.
    /// </summary>
    [Header("Batteries Spawner")]
    [SerializeField] GameObject battery = null;
    /// <summary>
    /// Posición donde se generarán las baterías a la izquierda.
    /// </summary>
    [SerializeField] Transform spawnPoint1 = null;
    /// <summary>
    /// Posición donde se generarán las baterías a la derecha.
    /// </summary>
    [SerializeField] Transform spawnPoint2 = null;

    /// <summary>
    /// El jugador.
    /// </summary>
    [Header("Characters")]
    [SerializeField] GameObject player = null;
    /// <summary>
    /// El componente Player del jugador.
    /// </summary>
    [SerializeField] Player playerClass = null;
    /// <summary>
    /// El enemigo.
    /// </summary>
    [SerializeField] GameObject enemy = null;
    /// <summary>
    /// El componente EnemyBig del enemigo.
    /// </summary>
    [SerializeField] EnemyBig enemyClass = null;
    
    #endregion

    void Start()
    {
        boss1Manager = this;
        GameManager.gameManager.InitialFade();
        GameManager.gameManager.StartDialogue(0);
    }

    /// <summary>
    /// Función que inicia la batalla con el jefe.
    /// </summary>
    public void StartBattle()
    {
        GameManager.gameManager.CloseDialogue();
        GameManager.gameManager.ActivateMusic();
        playerClass.enabled = true;
        enemyClass.enabled = true;
        StartCoroutine(SpawnBattery());
    }

    /// <summary>
    /// Función que se activa cuando el enemigo muere.
    /// </summary>
    public void BossDeath()
    {
        StartCoroutine(OpenElevator());
    }

    /// <summary>
    /// Corrutina que activa la animación del ascensor y comienza la transición de escenas.
    /// </summary>
    /// <returns></returns>
    IEnumerator OpenElevator()
    {
        GameManager.gameManager.DeactivateMusic();
        yield return new WaitForSeconds(2);
        GameManager.gameManager.ChangeCursor(false);
        elevatorAudio.clip = ring;
        elevatorAudio.Play();
        redButton.SetActive(false);
        greenButton.SetActive(true);
        yield return new WaitForSeconds(1);
        elevatorAudio.clip = doors;
        elevatorAudio.Play();
        yield return new WaitForSeconds(1);
        elevatorAnim.SetTrigger("BossDie");
        yield return new WaitForSeconds(2);
        GameManager.gameManager.FinalFade();
        yield return new WaitForSeconds(2);
        player.SetActive(false);
        yield return new WaitForSeconds(3);
        GameManager.gameManager.ChangeCursor(false);
        gunPanel.SetActive(true);
    }

    /// <summary>
    /// Corrutina que genera baterías de forma aleatoria.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBattery()
    {
        while (true)
        {
            yield return new WaitForSeconds(25);

            float randomNumber = Random.value;
            Transform batterySpawnPoint;

            if (randomNumber < 0.5)
            {
                batterySpawnPoint = spawnPoint1;
            }

            else
            {
                batterySpawnPoint = spawnPoint2;
            }

            if (enemy != null)
            {
                Instantiate(battery, batterySpawnPoint.position, batterySpawnPoint.rotation);
            }

            else
            {
                yield break;
            }
        }
    }
}