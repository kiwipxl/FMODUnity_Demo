using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

/*
* Handles the current VO language and can create instances based
* on the current language.
*/

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

    /*
    * Takes in a VO path such as "event:/VO/ENG/ENG_myVOname" and returns
    * an event instance VO based on the current language.
    */
    public static EventInstance createInstance(string VOPath)
    {
        return RuntimeManager.CreateInstance(getCurrentVOPath(VOPath));
    }

    /*
    * Takes in a VO path such as "event:/VO/ENG/ENG_myVOname" and returns
    * a new modified path based on the current
    * language (such as "event:/VO/SWE/SWE_myVOname" if the current language is swedish).
    */
    public static string getCurrentVOPath(string VOPath)
    {
        // Get the event path depending on language
        string langPath = "";
        if (currentLanguage == VOLanguage.ENGLISH) langPath = "VO/ENG/ENG_";
        else if (currentLanguage == VOLanguage.SWEDISH) langPath = "VO/SWE/SWE_";

        string currentPath = "event:/" + langPath;
        /*
        * Gets the name of the VOPath event by getting the substring from 
        * the last '/' + 5 to the end of the string.
        * 5 is used as a position offset because of the "XXX_" characters (plus 1).
        */
        currentPath += VOPath.Substring(VOPath.LastIndexOf("/") + 5);

        return currentPath;
    }
}
