using System;

/// <summary>
/// Clase con las posibles variables que pueden ser guardadas.
/// </summary>
[Serializable]
public class SettingsData
{
    public string activeLanguage;
    public bool firstTime;
    public float musicVolume;
    public float sfxVolume;
}