using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

//Voice Over languages
public enum VOLanguage
{
    UNKNOWN,
    ENGLISH,
    SWEDISH
};

public class VOLocalisation : MonoBehaviour
{
    //current language in use
    public static VOLanguage currentLanguage = VOLanguage.UNKNOWN;

    public static EventInstance createInstance(string VOPath)
    {
        return RuntimeManager.CreateInstance(getCurrentVOPath(VOPath));
    }

    public static string getCurrentVOPath(string VOPath)
    {
        // Get the event path depending on language
        string langPath = "";
        if (currentLanguage == VOLanguage.ENGLISH) langPath = "VO/ENG/ENG_";
        else if (currentLanguage == VOLanguage.SWEDISH) langPath = "VO/SWE/SWE_";

        // Event:/ + language event path + asset name
        string currentPath = "event:/" + langPath + VOPath.Substring(VOPath.LastIndexOf("/") + 5);

        return currentPath;
    }
}
