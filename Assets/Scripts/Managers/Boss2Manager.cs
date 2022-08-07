using System.Collections;
using UnityEngine;

/// <summary>
/// Clase con las funciones principales de la segunda batalla.
/// </summary>
public class Boss2Manager : MonoBehaviour
{
    #region Variables

    public static Boss2Manager boss2Manager;

    /// <summary>
    /// El prefab de las cajas explosivas.
    /// </summary>
    [Header ("Battle Spawners")]
    [SerializeField] GameObject tnt = null;
    /// <summary>
    /// El prefab de las baterías.
    /// </summary>
    [SerializeField] GameObject battery = null;
    /// <summary>
    /// La posición donde se generan las cajas a la izquierda.
    /// </summary>
    [SerializeField] Transform spawnPointLeft = null;
    /// <summary>
    /// La posición donde se generan las cajas a la derecha.
    /// </summary>
    [SerializeField] Transform spawnPointRight = null;
    /// <summary>
    /// La posición donde se generan las baterías.
    /// </summary>
    [SerializeField] Transform batterySpawnPoint = null;

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
    /// El componente Dog del enemigo.
    /// </summary>
    [SerializeField] Dog enemyClass = null;
    
    #endregion

    void Start()
    {
        boss2Manager = this;
        GameManager.gameManager.InitialFade();
        GameManager.gameManager.StartDialogue(0);
    }

    /// <summary>
    /// Función que inicia la batalla.
    /// </summary>
    public void StartBattle()
    {
        GameManager.gameManager.CloseDialogue();
        GameManager.gameManager.ActivateMusic();
        playerClass.enabled = true;
        enemyClass.enabled = true;
        RespawnTNT();
        StartCoroutine(SpawnBattery());
    }

    /// <summary>
    /// Función que se activa cuando el enemigo muere.
    /// </summary>
    public void BossDie()
    {
        StartCoroutine(EndBattle());
    }

    /// <summary>
    /// Función que se activa cuando explota una caja, haciendo que aparezca otra nueva.
    /// </summary>
    public void RespawnTNT()
    {
        if (Random.value < 0.5f)
        {
            Instantiate(tnt, spawnPointLeft.position, Quaternion.identity);
        }

        else
        {
            Instantiate(tnt, spawnPointRight.position, Quaternion.identity);
        }
    }

    /// <summary>
    /// Corrutina que hace que aparezcan baterías cada cierto tiempo.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBattery()
    {
        while (true)
        {
            yield return new WaitForSeconds(50);

            if (enemy != null)
            {
                Instantiate(battery, batterySpawnPoint.position, Quaternion.identity);
            }

            else
            {
                yield break;
            }
        }
    }

    /// <summary>
    /// Corrutina que se inicia al morir el enemigo, iniciando la transición entre escenas.
    /// </summary>
    /// <returns></returns>
    public IEnumerator EndBattle()
    {
        GameManager.gameManager.DeactivateMusic();
        yield return new WaitForSeconds(3);
        GameManager.gameManager.ChangeCursor(false);
        GameManager.gameManager.FinalFade();
        yield return new WaitForSeconds(2);
        player.SetActive(false);
        yield return new WaitForSeconds(4);
        GameManager.gameManager.LoadScene(5);
    }
}