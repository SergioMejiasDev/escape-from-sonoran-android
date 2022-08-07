using System.Collections;
using UnityEngine;

/// <summary>
/// Clase que contiene las funciones principales del nivel 3.
/// </summary>
public class Level3Manager : MonoBehaviour
{
    public static Level3Manager level3Manager;

    /// <summary>
    /// Panel de transición de escenas.
    /// </summary>
    [Header("Scene Transition")]
    [SerializeField] GameObject fadeBackgroundPanel = null;
    /// <summary>
    /// La imagen de transición de escenas.
    /// </summary>
    [SerializeField] SpriteRenderer fadeBackgroundImage;
    /// <summary>
    /// La velocidad de transición de escenas.
    /// </summary>
    float fadeBGSpeed = 0.5f;

    void Start()
    {
        level3Manager = this;
        GameManager.gameManager.ChangeCursor(true);
        GameManager.gameManager.InitialFade();
        GameManager.gameManager.TextFading();
    }

    /// <summary>
    /// Función que se activa al llegar al final del nivel.
    /// </summary>
    public void StartFade()
    {
        StartCoroutine(BackgroundFade());
    }

    /// <summary>
    /// Corrutina que hace que aparezca el fondo y se inicie la transición entre escenas.
    /// </summary>
    /// <returns></returns>
    IEnumerator BackgroundFade()
    {
        yield return new WaitForSeconds(2);
        GameManager.gameManager.DeactivateMusic();
        Color imageBGColor = fadeBackgroundImage.color;
        float alphaBGValue;

        while (fadeBackgroundImage.color.a > 0)
        {
            alphaBGValue = imageBGColor.a - (fadeBGSpeed * Time.deltaTime);
            imageBGColor = new Color(imageBGColor.r, imageBGColor.g, imageBGColor.b, alphaBGValue);
            fadeBackgroundImage.color = new Color(imageBGColor.r, imageBGColor.g, imageBGColor.b, alphaBGValue);
            yield return null;
        }

        fadeBackgroundPanel.SetActive(false);

        GameManager.gameManager.FinalFade();
        GameManager.gameManager.ChangeCursor(false);
        yield return new WaitForSeconds(2);
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.SetActive(false);
        yield return new WaitForSeconds(2);
        GameManager.gameManager.LoadScene(6);
    }
}