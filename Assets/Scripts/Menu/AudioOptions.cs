using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

/// <summary>
/// Clase encargada de los ajustes de sonido.
/// </summary>
public class AudioOptions : MonoBehaviour
{
    public static AudioOptions audioOptions;

    /// <summary>
    /// Slider que controla el volumen de la música.
    /// </summary>
    [SerializeField] Slider musicSlider = null;
    /// <summary>
    /// Slider que controla el volumen de los SFX.
    /// </summary>
    [SerializeField] Slider sfxSlider = null;
    /// <summary>
    /// AudioMixer asignado al volumen de la música.
    /// </summary>
    [SerializeField] AudioMixerGroup musicMixer = null;
    /// <summary>
    /// AudioMixer asignado al volumen de los SFX.
    /// </summary>
    [SerializeField] AudioMixerGroup sfxMixer = null;
    /// <summary>
    /// El volumen de la música.
    /// </summary>
    float musicVolume;
    /// <summary>
    /// El volumen de los SFX.
    /// </summary>
    float sfxVolume;

    private void Start()
    {
        audioOptions = this;

        LoadOptions();
    }

    private void Update()
    {
        musicVolume = musicSlider.value;
        sfxVolume = sfxSlider.value;

        musicMixer.audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);
        sfxMixer.audioMixer.SetFloat("SFXVolume", Mathf.Log10(sfxVolume) * 20);
    }

    /// <summary>
    /// Función que guarda los ajustes de sonido.
    /// </summary>
    public void SaveOptions()
    {
        SaveManager.saveManager.musicVolume = musicVolume;
        SaveManager.saveManager.sfxVolume = sfxVolume;
        SaveManager.saveManager.SaveOptions();
    }

    /// <summary>
    /// Función que carga los ajustes de sonido.
    /// </summary>
    void LoadOptions()
    {
        float musicVolumeLoaded = SaveManager.saveManager.musicVolume;
        musicSlider.value = musicVolumeLoaded;

        float sfxVolumeLoaded = SaveManager.saveManager.sfxVolume;
        sfxSlider.value = sfxVolumeLoaded;
    }
}