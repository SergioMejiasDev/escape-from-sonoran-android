using System.Collections;
using UnityEngine;

/// <summary>
/// Clase que controla las funciones principales del menú.
/// </summary>
public class MenuManager : MonoBehaviour
{
    public static MenuManager manager;

    public delegate void LanguageDelegate(string language);
    public static event LanguageDelegate OnLanguageChange;

    /// <summary>
    /// El prefab de los enemigos que aparecen en el menú.
    /// </summary>
    [SerializeField] GameObject enemy = null;
    /// <summary>
    /// La posición donde se generan los enemigos.
    /// </summary>
    [SerializeField] Transform generationPoint = null;
    /// <summary>
    /// El panel para cambiar el idioma.
    /// </summary>
    [SerializeField] GameObject panelLanguage = null;
    /// <summary>
    /// El panel del menú principal.
    /// </summary>
    [SerializeField] GameObject panelMenu = null;

    private void Awake()
    {
        manager = this;

        if (SaveManager.saveManager.firstTime)
        {
            panelMenu.SetActive(true);
        }

        else
        {
            panelLanguage.SetActive(true);
        }
    }

    void Start()
    {
        GameManager.gameManager.InitialFade();

        StartCoroutine(Generate());
    }

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GameManager.gameManager.CloseGame();
        }
    }

    /// <summary>
    /// Función encargada de cambiar el idioma de los textos en el juego.
    /// </summary>
    /// <param name="language">El código del idioma que se va a activar.</param>
    public void ChangeLanguage(string language)
    {
        MultilanguageManager.multilanguageManager.ChangeLanguage(language);

        OnLanguageChange(language);
    }

    /// <summary>
    /// Función que se activa cuando cambiamos el idioma a través del menú de opciones.
    /// </summary>
    public void NextLanguage()
    {
        switch (SaveManager.saveManager.activeLanguage)
        {
            case "EN":
                ChangeLanguage("ES");
                break;

            case "ES":
                ChangeLanguage("EN");
                break;
        }
    }

    /// <summary>
    /// Función que se activa para generar enemigos.
    /// </summary>
    void GenerateEnemy()
    {
        Destroy(Instantiate(enemy, generationPoint.position, Quaternion.identity), 12);
    }

    /// <summary>
    /// Corrutina que llama a la función de generar enemigos cada pocos segundos.
    /// </summary>
    /// <returns></returns>
    IEnumerator Generate()
    {
        while (true)
        {
            GenerateEnemy();
            yield return new WaitForSeconds(Random.Range(3, 15));
        }
    }
}