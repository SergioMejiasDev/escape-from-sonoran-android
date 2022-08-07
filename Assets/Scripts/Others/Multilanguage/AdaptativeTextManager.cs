using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que hace que los textos activos en pantalla cambien de idioma justo después de hacer efectivo el cambio.
/// </summary>
public class AdaptativeTextManager : MonoBehaviour
{
    /// <summary>
    /// Texto con las posibles traducciones.
    /// </summary>
    [SerializeField] MultilanguageText multilanguageText = null;

    private void OnEnable()
    {
        MenuManager.OnLanguageChange += ChangeLanguage;

        ChangeLanguage(MultilanguageManager.multilanguageManager.activeLanguage);
    }

    private void OnDisable()
    {
        MenuManager.OnLanguageChange -= ChangeLanguage;
    }

    private void Start()
    {
        ChangeLanguage(MultilanguageManager.multilanguageManager.activeLanguage);
    }

    /// <summary>
    /// Función que modifica el texto de acuerdo con el idioma activo.
    /// </summary>
    /// <param name="newLanguage">El idioma que queremos activar.</param>
    void ChangeLanguage(string newLanguage)
    {
        Text text = GetComponent<Text>();

        switch (newLanguage)
        {
            case "EN":
                text.text = multilanguageText.english;
                break;
            case "ES":
                text.text = multilanguageText.spanish;
                break;
        }
    }
}