using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que contiene las funciones principales de la última batalla.
/// </summary>
public class Boss3Manager : MonoBehaviour
{
    #region Variables

    public static Boss3Manager boss3Manager;

    /// <summary>
    /// El panel blanco que aparece al explotar la bola de energía.
    /// </summary>
    [Header("Explosion")]
    [SerializeField] GameObject whitePanel = null;
    /// <summary>
    /// La imagen blanca que cubre la pantalla al explotar la bola de energía.
    /// </summary>
    [SerializeField] Image whiteImage = null;
    /// <summary>
    /// Velocidad de transición de la pantalla blanca.
    /// </summary>
    readonly float fadeSpeed = 0.6f;
    /// <summary>
    /// Componente AudioSource de la bola de energía.
    /// </summary>
    [SerializeField] AudioSource audioSource = null;

    /// <summary>
    /// El sol en el fondo de la escena.
    /// </summary>
    [Header("Sky Fading")]
    [SerializeField] GameObject sun = null;
    /// <summary>
    /// La velocidad a la que desciende el sol.
    /// </summary>
    readonly float sunSpeed = 0.25f;
    /// <summary>
    /// El SpriteRenderer de la imagen del fondo.
    /// </summary>
    [SerializeField] SpriteRenderer bgsr = null;
    /// <summary>
    /// Velocidad de transición de la imagen del fondo.
    /// </summary>
    readonly float skyFadeSpeed = 0.01f;

    /// <summary>
    /// El prefab de las baterías.
    /// </summary>
    [Header("Battery Spawner")]
    [SerializeField] GameObject battery = null;

    /// <summary>
    /// El jugador.
    /// </summary>
    [Header("Characters")]
    [SerializeField] GameObject player = null;
    /// <summary>
    /// La clase FlyingPlayer del jugador.
    /// </summary>
    [SerializeField] FlyingPlayer playerClass = null;
    /// <summary>
    /// El enemigo.
    /// </summary>
    [SerializeField] GameObject enemy = null;
    /// <summary>
    /// El componente FinalBoss del enemigo.
    /// </summary>
    [SerializeField] FinalBoss enemyClass = null;

    /// <summary>
    /// Los textos que aparecerán tras morir el jefe.
    /// </summary>
    [Header("Narrative")]
    [SerializeField] Text[] narrativeTexts = null;
    /// <summary>
    /// Los tiempos que durarán los textos.
    /// </summary>
    readonly float[] narrativeTimes = new float[] {7, 7};
    
    #endregion

    void Start()
    {
        boss3Manager = this;
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
        StartCoroutine(Sun());
        StartCoroutine(SkyFade());
        playerClass.enabled = true;
        enemyClass.enabled = true;
        StartCoroutine(SpawnBattery());
    }

    /// <summary>
    /// Función que se activa cuando ocurre una explosión.
    /// </summary>
    /// <param name="killPlayer">Será verdadero si el jugador muere en la explosión. Falso si muere el enemigo.</param>
    public void StartExplosion(bool killPlayer)
    {
        StartCoroutine(Explosion(killPlayer));
    }

    /// <summary>
    /// Corrutina que inicia la explosión blanca.
    /// </summary>
    /// <param name="killPlayer">Será verdadero si el jugador muere en la explosión. Falso si muere el enemigo.</param>
    /// <returns></returns>
    IEnumerator Explosion(bool killPlayer)
    {
        if (killPlayer)
        {
            player.SetActive(false);
        }

        whitePanel.SetActive(true);
        Color imageColor = whiteImage.color;
        float alphaValue;

        while (whiteImage.color.a < 1)
        {
            alphaValue = imageColor.a + (fadeSpeed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            whiteImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null;
        }
        
        if (!killPlayer)
        {
            Destroy(enemy);
            audioSource.Play();
        }

        yield return new WaitForSeconds(4);

        while (whiteImage.color.a > 0)
        {
            alphaValue = imageColor.a - (fadeSpeed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            whiteImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null;
        }

        if (killPlayer)
        {
            GameManager.gameManager.GameOver();
        }

        else
        {
            StartCoroutine(EndGame());
        }
    }

    /// <summary>
    /// Corrutina encargada del movimiento del sol.
    /// </summary>
    /// <returns></returns>
    IEnumerator Sun()
    {
        while (sun.transform.position.y > -12)
        {
            sun.transform.Translate(Vector2.down * sunSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(sun);
    }

    /// <summary>
    /// Corrutina que inicia la transición entre fondos.
    /// </summary>
    /// <returns></returns>
    IEnumerator SkyFade()
    {
        yield return new WaitForSeconds(20);

        Color imageColor = bgsr.color;
        float alphaValue;

        while (bgsr.color.a > 0)
        {
            alphaValue = imageColor.a - (skyFadeSpeed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            bgsr.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null; 
        }
    }

    /// <summary>
    /// Corrutina que inicia los textos narrativos y comienza la transición entre escenas.
    /// </summary>
    /// <returns></returns>
    IEnumerator EndGame()
    {
        GameManager.gameManager.DeactivateMusic();
        yield return new WaitForSeconds(3);
        GameManager.gameManager.ChangeCursor(false);
        GameManager.gameManager.FinalFade();
        yield return new WaitForSeconds(2);
        GameManager.gameManager.ChangeCursor(false);
        Cursor.visible = false;
        player.SetActive(false);
        yield return new WaitForSeconds(4);
        
        for (int i = 0; i < narrativeTexts.Length; i++)
        {
            Color imageColor = narrativeTexts[i].color;
            float alphaValue;

            while (narrativeTexts[i].color.a < 1)
            {
                alphaValue = imageColor.a + (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                narrativeTexts[i].color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }

            yield return new WaitForSeconds(narrativeTimes[i]);

            while (narrativeTexts[i].color.a > 0)
            {
                alphaValue = imageColor.a - (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                narrativeTexts[i].color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }
        }

        GameManager.gameManager.LoadScene(7);
    }

    /// <summary>
    /// Corrutina que genera baterías de forma aleatoria.
    /// </summary>
    /// <returns></returns>
    IEnumerator SpawnBattery()
    {
        while (true)
        {
            float waitForBatteries = Random.Range(40, 50);
            
            yield return new WaitForSeconds(waitForBatteries);

            Vector2 batterySpawnPoint = new Vector2(Random.Range(-17, 2), Random.Range(-9, 9));

            if (enemy != null)
            {
                Instantiate(battery, batterySpawnPoint, Quaternion.identity) ;
            }

            else
            {
                yield break;
            }
        }
    }
}