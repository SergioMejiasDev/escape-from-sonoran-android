using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase encargada de que aparezcan los créditos en la escena.
/// </summary>
public class Credits : MonoBehaviour
{
    /// <summary>
    /// Velocidad de movimiento de los créditos.
    /// </summary>
    readonly float speed = 50;
    /// <summary>
    /// Componente RectTransform de los créditos.
    /// </summary>
    [SerializeField] RectTransform creditsTransform = null;
    /// <summary>
    /// Texto de agradecimiento que aparece al final de los créditos.
    /// </summary>
    [SerializeField] Text thanksText = null;
    /// <summary>
    /// Panel que nos permite saltar los créditos.
    /// </summary>
    [SerializeField] GameObject cancelPanel = null;
    /// <summary>
    /// La posición final de los créditos antes de aparecer el cartel de agradecimiento.
    /// </summary>
    readonly float finalPosition = 1500;

    void Start()
    {
        GameManager.gameManager.ChangeCursor(false);
        GameManager.gameManager.InitialFade();
        GameManager.gameManager.ActivateMusic();

        StartCoroutine(WaitForCancel());

        StartCoroutine(CreditsMovement());
    }

    /// <summary>
    /// Función que omite los créditos.
    /// </summary>
    public void CancelButton()
    {
        cancelPanel.SetActive(false);
        StartCoroutine(CloseCredits());
    }

    /// <summary>
    /// Corrutina que inicia el movimiento de los créditos.
    /// </summary>
    /// <returns></returns>
    IEnumerator CreditsMovement()
    {
        while (creditsTransform.anchoredPosition.y < finalPosition)
        {
            creditsTransform.anchoredPosition += new Vector2(0, speed * Time.deltaTime);
            yield return null;
        }

        Color levelColor = thanksText.color;
        float alphaValue;

        while (thanksText.color.a < 1)
        {
            alphaValue = levelColor.a + (0.5f * Time.deltaTime);
            levelColor = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            thanksText.color = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(3);

        while (thanksText.color.a > 0)
        {
            alphaValue = levelColor.a - (0.5f * Time.deltaTime);
            levelColor = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            thanksText.color = new Color(levelColor.r, levelColor.g, levelColor.b, alphaValue);
            yield return null;
        }

        yield return new WaitForSeconds(2);

        StartCoroutine(CloseCredits());
    }

    /// <summary>
    /// Corrutina que cierra los créditos y abre el menú.
    /// </summary>
    /// <returns></returns>
    IEnumerator CloseCredits()
    {
        GameManager.gameManager.FinalFade();
        yield return new WaitForSeconds(2);
        GameManager.gameManager.DeactivateMusic();
        yield return new WaitForSeconds(1);
        Cursor.visible = true;
        GameManager.gameManager.LoadScene(0);
    }

    /// <summary>
    /// Corrutina que activa el panel para saltar los créditos.
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForCancel()
    {
        yield return new WaitForSeconds(4);

        cancelPanel.SetActive(true);
    }
}