using System.Collections;
using UnityEngine;

/// <summary>
/// Clase que contiene las funciones principales del nivel 1.
/// </summary>
public class Level1Manager : MonoBehaviour
{
    #region Variables

    public static Level1Manager level1Manager;

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
    /// La puerta cerrada.
    /// </summary>
    [Header("Scene Transition")]
    [SerializeField] GameObject closedDoor = null;
    /// <summary>
    /// La puerta abierta.
    /// </summary>
    [SerializeField] GameObject openedDoor = null;
    /// <summary>
    /// Componente AudioSource de la puerta.
    /// </summary>
    [SerializeField] AudioSource doorSource = null;
    
    #endregion

    void Start()
    {
        level1Manager = this;

        GameManager.gameManager.StartNarrative();
    }

    /// <summary>
    /// Función que se activa cuando el jugador entra en la zona final del nivel.
    /// </summary>
    public void SpawnZone()
    {
        StartCoroutine(SpawnEnemies());
    }

    /// <summary>
    /// Corrutina que genera múltiples enemigos antes de abrir la puerta del final.
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
        remainingEnemies = 5;

        yield return new WaitForSeconds(1);
        Instantiate(battery, batterySpawnZone.position, batterySpawnZone.rotation);
        warning.SetActive(true);

        yield return new WaitForSeconds(1);
        Instantiate(enemy2, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 1;

        yield return new WaitForSeconds(2);

        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy2, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        warning.SetActive(false);
        remainingEnemies -= 1;

        yield return new WaitForSeconds(2);
        Instantiate(battery, batterySpawnZone.position, batterySpawnZone.rotation);
        yield return new WaitForSeconds(2);

        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy1, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 1;
        yield return new WaitForSeconds(4);

        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy1, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 1;

        yield return new WaitForSeconds(1);
        Instantiate(battery, batterySpawnZone.position, batterySpawnZone.rotation);

        yield return new WaitForSeconds(6);

        if (!player.activeSelf)
        {
            yield break;
        }

        Instantiate(enemy3, enemiesSpawnZone.position, enemiesSpawnZone.rotation);
        remainingEnemies -= 1;

        StartCoroutine(VerifyEnemies());
    }

    /// <summary>
    /// Corrutina que comprueba si hay algún enemigo en la escena.
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
                StartCoroutine(OpenDoor(player));

                yield break;
            }

            yield return new WaitForSeconds(2);
        }
    }

    /// <summary>
    /// Corrutina que abre la puerta del final e inicia la transición de escenas.
    /// </summary>
    /// <param name="player">El jugador.</param>
    /// <returns></returns>
    IEnumerator OpenDoor(GameObject player)
    {
        GameManager.gameManager.DeactivateMusic();

        yield return new WaitForSeconds(1);

        GameManager.gameManager.ChangeCursor(false);
        
        if (!player.activeSelf)
        {
            yield break;
        }

        closedDoor.SetActive(false);
        openedDoor.SetActive(true);
        doorSource.Play();
        
        GameManager.gameManager.FinalFade();

        yield return new WaitForSeconds(4);

        GameManager.gameManager.LoadScene(2);
    }
}