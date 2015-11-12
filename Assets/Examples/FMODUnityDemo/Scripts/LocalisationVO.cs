using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public enum VOLanguage
{
    UNKNOWN,
    ENGLISH,
    SWEDISH
};

public class LocalisationVO : MonoBehaviour
{
    // All VO assets set in editor.
    // These can be of any language.
    public FMODAsset[] VOEventAssets;

    // List of VO's in current language bank
    public List<EventInstance> VOEvents = new List<EventInstance>();

    private Bank currentBank;
    private VOLanguage currentLang = VOLanguage.UNKNOWN;

    private void Start() {
        switchBankTo(VOLanguage.ENGLISH);
    }

    public void switchBankTo(VOLanguage newVOLanguage)
    {
        if (currentLang == newVOLanguage) return;

        clearAllVOEvents();

        FMOD.Studio.System sys = FMOD_StudioSystem.instance.System;

        // Unload current bank if it exists
        if (currentBank != null) currentBank.unload();

        // Load new bank file
        switch (newVOLanguage)
        {
            case VOLanguage.ENGLISH:
                sys.loadBankFile(Application.dataPath + "/StreamingAssets/VO_ENG.bank",
                                 LOAD_BANK_FLAGS.NORMAL, out currentBank);
                break;
            case VOLanguage.SWEDISH:
                sys.loadBankFile(Application.dataPath + "/StreamingAssets/VO_SWE.bank",
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

        // Get all events with the same name as the assets in VOEventAssets but
        // change their path depending on language
        for (int n = 0; n < VOEventAssets.Length; ++n)
        {
            string path = VOEventAssets[n].path;
            // Event:/ + language event path + asset name
            string eventPath = "event:/" + langPath + path.Substring(path.LastIndexOf("/") + 1);

            VOEvents.Add(FMOD_StudioSystem.instance.GetEvent(eventPath));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            switchBankTo(currentLang == VOLanguage.ENGLISH ? VOLanguage.SWEDISH : VOLanguage.ENGLISH);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            Subtitles.start(VOEvents[UnityEngine.Random.Range(0, VOEvents.Count)]);
        }
    }
}
