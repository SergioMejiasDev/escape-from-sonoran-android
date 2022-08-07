using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Clase encargada de cargar y guardar los ajustes en el dispositivo.
/// </summary>
public class SaveManager : MonoBehaviour
{
    public static SaveManager saveManager;

    /// <summary>
    /// El idioma activo.
    /// </summary>
    [Header("Options")]
    public string activeLanguage = "EN";
    /// <summary>
    /// Falso si es la primera vez que se abre el juego. Verdadero si ya se ha hecho previamente.
    /// </summary>
    public bool firstTime = false;
    /// <summary>
    /// El volumen de la música.
    /// </summary>
    public float musicVolume = 1;
    /// <summary>
    /// El volumen de los SFX.
    /// </summary>
    public float sfxVolume = 1;

    private void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("SaveManager");

        if (objs.Length > 1)
        {
            Destroy(gameObject);
        }

        else
        {
            saveManager = this;

            DontDestroyOnLoad(gameObject);

            LoadOptions();

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
    }

    /// <summary>
    /// Función que carga las variables de las opciones.
    /// </summary>
    public void LoadOptions()
    {
        SettingsData data = new SettingsData();

        string path = Application.persistentDataPath + "/Settings.sav";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            data = formatter.Deserialize(stream) as SettingsData;
            stream.Close();

            activeLanguage = data.activeLanguage;
            firstTime = data.firstTime;
            musicVolume = data.musicVolume;
            sfxVolume = data.sfxVolume;
        }
    }

    /// <summary>
    /// Función que guarda las variables de las opciones.
    /// </summary>
    public void SaveOptions()
    {
        SettingsData data = new SettingsData
        {
            activeLanguage = activeLanguage,
            firstTime = firstTime,
            musicVolume = musicVolume,
            sfxVolume = sfxVolume,
        };

        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/Settings.sav";

        FileStream fileStream = new FileStream(path, FileMode.Create);

        formatter.Serialize(fileStream, data);

        fileStream.Close();
    }
}