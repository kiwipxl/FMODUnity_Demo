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
    // All VO assets set in editor.
    // These can be of any language.
    [EventRef] public string [] VOEventPaths;

    // List of VO's in current language bank
    public List<EventInstance> VOEvents = new List<EventInstance>();

    //current loaded language bank
    private Bank currentBank;
    //current language in use
    private VOLanguage currentLang = VOLanguage.UNKNOWN;

    private void Start() {
        // Loads the english bank by default
        switchBankTo(VOLanguage.ENGLISH);
    }

    public void switchBankTo(VOLanguage newVOLanguage)
    {
        if (currentLang == newVOLanguage) return;

        clearAllVOEvents();

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

        updateVOEvents();
    }

    public void clearAllVOEvents()
    {
        // Stops and releases all current VO Events
        foreach (EventInstance VOEvent in VOEvents)
        {
            VOEvent.stop(STOP_MODE.IMMEDIATE);
            VOEvent.release();
        }
        VOEvents.Clear();
    }

    public void updateVOEvents()
    {
        clearAllVOEvents();

        // Get the event path depending on language
        string langPath = "";
        if (currentLang == VOLanguage.ENGLISH) langPath = "VO/ENG/";
        else if (currentLang == VOLanguage.SWEDISH) langPath = "VO/SWE/";

        // Get all events with the same name as the assets in VOEventPaths but
        // change their path depending on language
        for (int n = 0; n < VOEventPaths.Length; ++n)
        {
            string path = VOEventPaths[n];
            // Event:/ + language event path + asset name
            string eventPath = "event:/" + langPath + path.Substring(path.LastIndexOf("/") + 1);

            VOEvents.Add(RuntimeManager.CreateInstance(eventPath));
        }
    }

    private void Update()
    {
        //temporary code
        //press B for random subtitles
        if (Input.GetKeyDown(KeyCode.B))
        {
            Subtitles.start(VOEvents[UnityEngine.Random.Range(0, VOEvents.Count)]);
        }
    }
}
