using System.Collections;
using UnityEngine;

/// <summary>
/// Clase con las funciones principales del nivel 2.
/// </summary>
public class Level2Manager : MonoBehaviour
{
    #region Variables

    public static Level2Manager level2Manager;

    /// <summary>
    /// Prefab del enemigo tipo 1.
    /// </summary>
    [Header("Spawn Zone")]
    [SerializeField] GameObject enemy1 = null;
    /// <summary>
    /// Prefab del enemigo tipo 2.
    /// </summary>
    [SerializeField] GameObject enemy2 = null;
    /// <summary>
    /// Prefab del enemigo tipo 3.
    /// </summary>
    [SerializeField] GameObject enemy3 = null;
    /// <summary>
    /// Enemigos restantes en el nivel.
    /// </summary>
    int remainingEnemies;
    /// <summary>
    /// Prefab de las baterías.
    /// </summary>
    [SerializeField] GameObject battery = null;
    /// <summary>
    /// Posición donde se generarán las baterías.
    /// </summary>
    [SerializeField] Transform batterySpawnZone = null;
    /// <summary>
    /// Posición donde se generarán los enemigos.
    /// </summary>
    [SerializeField] Transform enemiesSpawnZone = null;
    /// <summary>
    /// El cartel que aparecerá indicando el peligro.
    /// </summary>
    [SerializeField] GameObject warning = null;

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
    [SerializeField] Animator elevatorAnim = null;
    /// <summary>
    /// Componente AudioSource del ascensor.
    /// </summary>
    [SerializeField] AudioSource elevatorAudio = null;
    /// <summary>
    /// Sonido que se reproduce al llegar el ascensor.
    /// </summary>
    [SerializeField] AudioClip ring = null;
    /// <summary>
    /// Sonido que se reproduce al abrirse las puertas.
    /// </summary>
    [SerializeField] AudioClip doors = null;
    
    #endregion

    void Start()
    {
        level2Manager = this;

        GameManager.gameManager.ChangeCursor(true);
        GameManager.gameManager.InitialFade();
        GameManager.gameManager.TextFading();
    }

    /// <summary>
    /// Función que inicia la corrutina para generar enemigos al final del nivel.
    /// </summary>
    public void SpawnZone()
    {
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Corrutina que genera múltiples enemigos al final del nivel.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnEnemies()
    {
        Camera.main.GetComponent<CameraMovement>().enabled = false;
        GameObject[] aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        for (int i = 0; i < aliveEnemies.Length; i++)
        {
            Destroy(aliveEnemies[i]);
        }

        enemy1.GetComponent<EnemyClass1>().direction = 1;
        enemy2.GetComponent<EnemyClass2>().direction = 1;
        enemy3.GetComponent<EnemyClass3>().direction = 1;
        remainingEnemies = 6;

        yield return new WaitForSeconds(1);
        Instantiate(battery, batterySpawnZone.position, batterySpawnZone.rotation);
        warning.SetActive(true);

        yield return new WaitForSeconds(1);
        Instantiate(enemy2, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        Instantiate(enemy3, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 2;

        yield return new WaitForSeconds(3);
        
        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy2, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        Instantiate(enemy3, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 2;
        warning.SetActive(false);

        yield return new WaitForSeconds(3);

        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy1, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        Instantiate(enemy3, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 2;

        StartCoroutine(VerifyEnemies());
    }

    /// <summary>
    /// Corrutina que comprueba si hay enemigos restantes en el nivel.
    /// </summary>
    /// <returns></returns>
    IEnumerator VerifyEnemies()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        while (true)
        {
            if (!player.activeSelf)
            {
                GameObject[] batteries = GameObject.FindGameObjectsWithTag("Battery");

                if (batteries != null)
                {
                    for (int i = 0; i < batteries.Length; i++)
                    {
                        Destroy(batteries[i]);
                    }
                }

                yield break;
            }

            GameObject[] aliveEnemies = GameObject.FindGameObjectsWithTag("Enemy");

            if (aliveEnemies.Length == 0 && remainingEnemies == 0)
            {
                StartCoroutine(OpenElevator(player));

                yield break;
            }

            yield return new WaitForSeconds(2);
        }
    }

    /// <summary>
    /// Corrutina que abre el ascensor e inicia la transición entre escenas.
    /// </summary>
    /// <param name="player">El jugador.</param>
    /// <returns></returns>
    IEnumerator OpenElevator(GameObject player)
    {
        GameManager.gameManager.DeactivateMusic();
        yield return new WaitForSeconds(1);
        GameManager.gameManager.ChangeCursor(false);
        elevatorAudio.clip = ring;
        elevatorAudio.Play();
        redButton.SetActive(false);
        greenButton.SetActive(true);
        yield return new WaitForSeconds(1);
        elevatorAudio.clip = doors;
        elevatorAudio.Play();
        yield return new WaitForSeconds(1);

        if (!player.activeSelf)
        {
            yield break;
        }

        elevatorAnim.SetTrigger("BossDie");
        GameManager.gameManager.FinalFade();
        yield return new WaitForSeconds(2);
        player.SetActive(false);
        yield return new WaitForSeconds(4);
        GameManager.gameManager.LoadScene(4);
    }
}