using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// Clase que controla las funciones principales del juego.
/// </summary>
public class GameManager : MonoBehaviour
{
    #region Variables
    
    public static GameManager gameManager;

    /// <summary>
    /// Los paneles del menú.
    /// </summary>
    [Header("Menu Panels")]
    [SerializeField] GameObject[] panels = null;

    /// <summary>
    /// Panel con el menú de pausa.
    /// </summary>
    [Header ("Pause")]
    [SerializeField] GameObject panelPause = null;
    /// <summary>
    /// Panel con los inputs del jugador.
    /// </summary>
    [SerializeField] GameObject panelControllers = null;

    /// <summary>
    /// El jugador.
    /// </summary>
    [Header("Game Over")]
    [SerializeField] GameObject player = null;
    /// <summary>
    /// La posición donde reaparece el jugador tras morir.
    /// </summary>
    Vector3 respawnPosition;
    /// <summary>
    /// El Trigger al final del nivel.
    /// </summary>
    [SerializeField] GameObject finalTrigger = null;
    /// <summary>
    /// El panel con el texto de Game Over.
    /// </summary>
    [SerializeField] GameObject gameOver = null;
    /// <summary>
    /// El texto de Game Over.
    /// </summary>
    Text gameOverText;
    /// <summary>
    /// El panel de Game Over.
    /// </summary>
    [SerializeField] GameObject gameOverPanel = null;

    /// <summary>
    /// Los textos narrativos al inicio del nivel.
    /// </summary>
    [Header("Narrative Texts")]
    [SerializeField] Text[] narrativeTexts = null;
    /// <summary>
    /// Los tiempos de los textos narrativos.
    /// </summary>
    readonly float[] narrativeTimes = new float[] { 2, 15, 15, 15, 3 };
    /// <summary>
    /// El logotipo del juego.
    /// </summary>
    [SerializeField] Image logo = null;
    /// <summary>
    /// El texto que indica que podemos omitir los textos narrativos.
    /// </summary>
    [SerializeField] Text skipText = null;
    /// <summary>
    /// Verdadero si se pueden omitir los textos. Falso si no.
    /// </summary>
    bool canSkip = false;

    /// <summary>
    /// El panel que muestra los diálogos con los jefes.
    /// </summary>
    [Header("Dialogues")]
    [SerializeField] GameObject dialoguePanel = null;
    /// <summary>
    /// Los posibles diálogos con los jefes.
    /// </summary>
    [SerializeField] GameObject[] dialogues = null;

    /// <summary>
    /// El panel de transición entre escenas.
    /// </summary>
    [Header("Scene Fading")]
    [SerializeField] GameObject fadePanel = null;
    /// <summary>
    /// La imagen negra de transición entre escenas.
    /// </summary>
    Image fadeImage;
    /// <summary>
    /// Los objetos que tienen que activarse al finalizar los textos narrativos.
    /// </summary>
    [SerializeField] GameObject[] activableObjects = null;
    /// <summary>
    /// El panel con el nombre del nivel.
    /// </summary>
    [SerializeField] GameObject levelTextPanel = null;
    /// <summary>
    /// El texto con el nombre del nivel.
    /// </summary>
    Text levelText;

    /// <summary>
    /// AudioSource que reproducirá una canción de fondo.
    /// </summary>
    [Header("Music")]
    [SerializeField] AudioSource ambientMusic = null;
    /// <summary>
    /// Música que sonará durante los textos narrativos.
    /// </summary>
    [SerializeField] AudioClip transitionMusic = null;
    /// <summary>
    /// La canción que sonará de fondo durante el nivel.
    /// </summary>
    [SerializeField] AudioClip levelMusic = null;

    #endregion

    void Awake()
    {
        Time.timeScale = 1;
        gameManager = this;

        LetterBoxer.AddLetterBoxingCamera();

        if (gameOver != null)
        {
            gameOverText = gameOver.GetComponent<Text>();
        }

        if (fadePanel != null)
        {
            fadeImage = fadePanel.GetComponent<Image>();
            fadePanel.SetActive(true);
        }

        if (levelTextPanel != null)
        {
            levelText = levelTextPanel.GetComponent<Text>();
        }

        if (player != null)
        {
            respawnPosition = player.transform.position;
        }
    }

    /// <summary>
    /// Función que se activa al cancelar los textos narrativos.
    /// </summary>
    public void CancelButton()
    {
        if (canSkip)
        {
            StopCoroutine(Narrative());

            StopCoroutine(SkipNarrative());

            for (int i = 0; i < narrativeTexts.Length; i++)
            {
                narrativeTexts[i].enabled = false;
            }

            ambientMusic.Stop();
            StartLevel();
        }
    }

    /// <summary>
    /// Función que abre los paneles del menú.
    /// </summary>
    /// <param name="panelToOpen">El panel que queremos abrir. El resto de los paneles se cerrarán.</param>
    public void OpenPanel(GameObject panelToOpen)
    {
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        panelToOpen.SetActive(true);
    }

    /// <summary>
    /// Función que cierra el juego.
    /// </summary>
    public void CloseGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    /// <summary>
    /// Función que carga una nueva escena.
    /// </summary>
    /// <param name="buildIndex">La escena que queremos cargar.</param>
    public void LoadScene(int buildIndex)
    {
        Time.timeScale = 1;

        SceneManager.LoadScene(buildIndex);
    }

    /// <summary>
    /// Función que activa y desactiva el panel de los inputs.
    /// </summary>
    /// <param name="customCursor">Verdadero para activarlo, falso para desactivarlo.</param>
    public void ChangeCursor(bool customCursor)
    {
        if (panelControllers == null)
        {
            return;
        }

        if (customCursor)
        {
            panelControllers.SetActive(true);
        }

        else
        {
            panelControllers.SetActive(false);
        }
    }

    /// <summary>
    /// Función encargada de pausar el juego.
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
        panelPause.SetActive(true);
        ChangeCursor(false);
    }

    /// <summary>
    /// Función encargada de reanudar el juego.
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
        panelPause.SetActive(false);
        ChangeCursor(true);
    }

    /// <summary>
    /// Función que activa las escenas narrativas.
    /// </summary>
    public void StartNarrative()
    {
        Cursor.visible = false;
        StartCoroutine(Narrative());
    }

    /// <summary>
    /// Función que abre el panel de los diálogos.
    /// </summary>
    /// <param name="dialogue">El diálogo que queremos abrir.</param>
    public void StartDialogue(int dialogue)
    {
        dialoguePanel.SetActive(true);

        ChangeCursor(false);
        
        for (int i = 0; i < dialogues.Length; i++)
        {
            dialogues[i].SetActive(false);
        }

        dialogues[dialogue].SetActive(true);
    }

    /// <summary>
    /// Función que cierra el panel de los diálogos.
    /// </summary>
    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        ChangeCursor(true);
    }

    /// <summary>
    /// Función que activa las corrutinas necesarias al empezar el nivel.
    /// </summary>
    public void StartLevel()
    {
        canSkip = false;
        Cursor.visible = true;
        ChangeCursor(true);
        ActivateMusic();
        InitialFade();
        TextFading();
    }

    /// <summary>
    /// Función que inicia la corrutina del Game Over.
    /// </summary>
    public void GameOver()
    {
        panelControllers.SetActive(false);

        StartCoroutine(FadeGameOver());
    }

    /// <summary>
    /// Función que devuelve el jugador al último punto de control.
    /// </summary>
    public void QuitGameOver()
    {
        StartCoroutine(ContinueGame());
    }

    /// <summary>
    /// Función que se activa cuando alcanzamos un punto de control.
    /// </summary>
    /// <param name="checkPointPosition">Posición del punto de control que hemos alcanzado.</param>
    public void CheckPoint(Vector3 checkPointPosition)
    {
        respawnPosition = checkPointPosition;
    }

    /// <summary>
    /// Función que activa la música del nivel.
    /// </summary>
    public void ActivateMusic()
    {
        ambientMusic.clip = levelMusic;
        ambientMusic.volume = 1;
        ambientMusic.Play();
    }

    /// <summary>
    /// Función que desactiva la música al final del nivel.
    /// </summary>
    public void DeactivateMusic()
    {
        StartCoroutine(FadeOutMusic());
    }

    /// <summary>
    /// Función que activa la transición inicial entre escenas.
    /// </summary>
    public void InitialFade()
    {
        StartCoroutine(FadeIn(2, 0));

        if (activableObjects != null)
        {
            for (int i = 0; i < activableObjects.Length; i++)
            {
                activableObjects[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// Función que activa la transición final entre escenas.
    /// </summary>
    public void FinalFade()
    {
        StartCoroutine(FadeOut(2, 1));
    }

    /// <summary>
    /// Función que activa el texto con el nombre del nivel.
    /// </summary>
    public void TextFading()
    {
        StartCoroutine(TextFade());
    }

    /// <summary>
    /// Corrutina encargada de la transición entre escenas narrativas.
    /// </summary>
    /// <returns></returns>
    IEnumerator Narrative()
    {
        StartCoroutine(SkipNarrative());
        Cursor.visible = false;
        ambientMusic.clip = transitionMusic;
        ambientMusic.volume = 1;
        ambientMusic.Play();

        for (int i = 0; i < narrativeTexts.Length; i++)
        {
            Color imageColor = narrativeTexts[i].color;
            float alphaValue;

            while (narrativeTexts[i].color.a < 1)
            {
                if (!canSkip)
                {
                    yield break;
                }

                alphaValue = imageColor.a + (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                narrativeTexts[i].color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }

            yield return new WaitForSeconds(narrativeTimes[i]);

            while (narrativeTexts[i].color.a > 0)
            {
                if (!canSkip)
                {
                    yield break;
                }

                alphaValue = imageColor.a - (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                narrativeTexts[i].color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }
        }

        if (logo != null)
        {
            yield return new WaitForSeconds(1);
            Color imageColor = logo.color;
            float alphaValue;

            while (logo.color.a < 1)
            {
                if (!canSkip)
                {
                    yield break;
                }

                alphaValue = imageColor.a + (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                logo.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }

            yield return new WaitForSeconds(3);

            while (logo.color.a > 0)
            {
                if (!canSkip)
                {
                    yield break;
                }

                alphaValue = imageColor.a - (0.5f * Time.deltaTime);
                imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                logo.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
                yield return null;
            }
        }

        while (ambientMusic.volume > 0.01f)
        {
            if (!canSkip)
            {
                yield break;
            }

            ambientMusic.volume -= Time.deltaTime / 2;
            yield return null;
        }

        yield return new WaitForSeconds(2);

        if (!canSkip)
        {
            yield break;
        }

        StartLevel();
    }

    /// <summary>
    /// Corrutina que hace que aparezcan el panel y el mensaje de Game Over.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeGameOver()
    {
        gameOver.SetActive(true);
        
        StartCoroutine(FadeOut(1, 2));
        Cursor.visible = false;

        Color gameOverColor = gameOverText.color;
        float alphaValue;

        while (gameOverText.color.a < 1)
        {
            alphaValue = gameOverColor.a + (1 * Time.deltaTime);
            gameOverColor = new Color(gameOverColor.r, gameOverColor.g, gameOverColor.b, alphaValue);
            gameOverText.color = new Color(gameOverColor.r, gameOverColor.g, gameOverColor.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(4);
        gameOverPanel.SetActive(true);
        ChangeCursor(false);
        Cursor.visible = true;

        if (finalTrigger != null)
        {
            if (!finalTrigger.activeSelf)
            {
                GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
                for (int i = 0; i < enemies.Length; i++)
                {
                    Destroy(enemies[i]);
                }
            }
        }

        Time.timeScale = 0;
    }

    /// <summary>
    /// Corrutina que permite continuar la partida después del Game Over.
    /// </summary>
    /// <returns></returns>
    IEnumerator ContinueGame()
    {
        Time.timeScale = 1;
        ChangeCursor(true);
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("BulletEnemy");

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetActive(false);
        }

        gameOverPanel.SetActive(false);
        Color gameOverColor = gameOverText.color;
        float alphaValue;

        while (gameOverText.color.a > 0)
        {
            alphaValue = gameOverColor.a - (3 * Time.deltaTime);
            gameOverColor = new Color(gameOverColor.r, gameOverColor.g, gameOverColor.b, alphaValue);
            gameOverText.color = new Color(gameOverColor.r, gameOverColor.g, gameOverColor.b, alphaValue);
            yield return null;
        }

        gameOver.SetActive(false);
        StartCoroutine(FadeIn(2, 0));
        player.transform.position = respawnPosition;
        Camera.main.GetComponent<CameraMovement>().enabled = true;
        yield return new WaitForSeconds(2);
        player.GetComponent<PlayerHealth>().RestorePlayer();

        if (finalTrigger != null)
        {
            finalTrigger.SetActive(true);
        }
    }

    /// <summary>
    /// Corrutina de la transición inicial.
    /// </summary>
    /// <param name="speed">Velocidad a la que ocurre la transición.</param>
    /// <param name="waitTime">Tiempo de espera antes de iniciar la transición.</param>
    /// <returns></returns>
    public IEnumerator FadeIn(float speed, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        Color imageColor = fadeImage.color;
        float alphaValue;

        while (fadeImage.color.a > 0)
        {
            alphaValue = imageColor.a - (speed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            fadeImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null;
        }

        fadePanel.SetActive(false);
    }

    /// <summary>
    /// Corrutina de la transición final.
    /// </summary>
    /// <param name="speed">Velocidad a la que ocurre la transición.</param>
    /// <param name="waitTime">Tiempo de espera antes de iniciar la transición.</param>
    /// <returns></returns>
    public IEnumerator FadeOut(float speed, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        fadePanel.SetActive(true);
        
        Color imageColor = fadeImage.color;
        float alphaValue;

        while (fadeImage.color.a < 1)
        {
            alphaValue = imageColor.a + (speed * Time.deltaTime);
            imageColor = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            fadeImage.color = new Color(imageColor.r, imageColor.g, imageColor.b, alphaValue);
            yield return null;
        }
    }

    /// <summary>
    /// Corrutina encargada de activar y desactivar los textos con el nombre del nivel.
    /// </summary>
    /// <returns></returns>
    IEnumerator TextFade()
    {
        levelTextPanel.SetActive(true);

        Color levelColor = levelText.color;
        float alphaValue;

        while (levelText.color.a < 1)
        {
            alphaValue = levelColor.a + (0.5f * Time.deltaTime);
            levelColor = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            levelText.color = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(3);

        while (levelText.color.a > 0)
        {
            alphaValue = levelColor.a - (0.5f * Time.deltaTime);
            levelColor = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            levelText.color = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            yield return null;
        }

        levelTextPanel.SetActive(false);
    }

    /// <summary>
    /// Corrutina que desactiva la música al final del nivel.
    /// </summary>
    /// <returns></returns>
    IEnumerator FadeOutMusic()
    {
        while (ambientMusic.volume > 0.01f)
        {
            ambientMusic.volume -= Time.deltaTime / 2;
            yield return null;
        }
    }

    /// <summary>
    /// Corrutina que muestra el texto donde se indica que podemos saltar las narrativas.
    /// </summary>
    /// <returns></returns>
    IEnumerator SkipNarrative()
    {
        canSkip = true;

        Color Color = skipText.color;
        float alphaValue;

        while (skipText.color.a < 1)
        {
            alphaValue = Color.a + (1f * Time.deltaTime);
            Color = new Color(Color.r, Color.g, Color.b, alphaValue);
            skipText.color = new Color(Color.r, Color.g, Color.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(1);

        while (skipText.color.a > 0)
        {
            alphaValue = Color.a - (1f * Time.deltaTime);
            Color = new Color(Color.r, Color.g, Color.b, alphaValue);
            skipText.color = new Color(Color.r, Color.g, Color.b, alphaValue);
            yield return null;
        }
    }
}