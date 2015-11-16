using System;
using System.Collections.Generic;
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

/*
* Handles VO for multiple languages.
* Unloads and loads language bank on request to save memory.
*
* All VO events must be the same name when switching languages.
* For example:
* VO_ENG_BANK
*       - myVOname
* VO_SWE_BANK
*       - myVOname
* This way, when switching banks, the event asset names will stay the same.
*/

public class LocalisationVO : MonoBehaviour
{
    //current loaded language bank
    private static Bank currentBank;
    //current language in use
    private static VOLanguage currentLang = VOLanguage.UNKNOWN;

    private void Start() {
        // Loads the english bank by default
        switchBankTo(VOLanguage.ENGLISH);
    }

    public static void switchBankTo(VOLanguage newVOLanguage)
    {
        if (currentLang == newVOLanguage) return;

        FMOD.Studio.System sys = RuntimeManager.StudioSystem;

        // Unload current bank if it exists
        if (currentBank != null) currentBank.unload();

        // Load new bank file
        switch (newVOLanguage)
        {
            case VOLanguage.ENGLISH:
                sys.loadBankFile(Application.dataPath + "/StreamingPaths/VO_ENG.bank",
                                 LOAD_BANK_FLAGS.NORMAL, out currentBank);
                break;
            case VOLanguage.SWEDISH:
                sys.loadBankFile(Application.dataPath + "/StreamingPaths/VO_SWE.bank",
                                 LOAD_BANK_FLAGS.NORMAL, out currentBank);
                break;
        }
        currentLang = newVOLanguage;
    }
    
    public static EventInstance updateVO(EventInstance ev)
    {
        // Get the event path depending on language
        string langPath = "";
        if (currentLang == VOLanguage.ENGLISH) langPath = "VO/ENG/";
        else if (currentLang == VOLanguage.SWEDISH) langPath = "VO/SWE/";

        // Get VO event path
        EventDescription desc;
        ev.getDescription(out desc);
        string path;
        desc.getPath(out path);

        // Event:/ + language event path + asset name
        string eventPath = "event:/" + langPath + path.Substring(path.LastIndexOf("/") + 1);

        // Return the same event if the paths are equal (they are the same language), or
        // return a new instance of the new language VO is they are not the same language
        if (eventPath != path)
        {
            ev.release();
            return RuntimeManager.CreateInstance(eventPath);
        }
        return ev;
    }
}
