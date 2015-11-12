using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FMOD.Studio;

public class LocalisationVO : MonoBehaviour
{
    public FMODAsset[] VOEventAssets;
    public List<EventInstance> VOEvents = new List<EventInstance>();

    enum BankLang
    {
        UNKNOWN, 
        ENGLISH, 
        SWEDISH
    };

    private Bank currentBank;
    private BankLang currentLang = BankLang.UNKNOWN;

    private void Start() {
        switchBankTo(BankLang.ENGLISH);
    }

    private void switchBankTo(BankLang newBankLang)
    {
        FMOD.Studio.System sys = FMOD_StudioSystem.instance.System;

        if (currentBank != null) currentBank.unload();

        switch (newBankLang)
        {
            case BankLang.ENGLISH:
                sys.loadBankFile(Application.dataPath + "/StreamingAssets/VO_ENG.bank",
                                 LOAD_BANK_FLAGS.NORMAL, out currentBank);
                break;
            case BankLang.SWEDISH:
                sys.loadBankFile(Application.dataPath + "/StreamingAssets/VO_SWE.bank",
                                 LOAD_BANK_FLAGS.NORMAL, out currentBank);
                break;
        }
        currentLang = newBankLang;

        updateVOEvents();
    }

    public void updateVOEvents()
    {
        string langPath = "";
        if (currentLang == BankLang.ENGLISH) langPath = "VO/ENG/";
        else if (currentLang == BankLang.SWEDISH) langPath = "VO/SWE/";

        foreach (EventInstance VOEvent in VOEvents)
        {
            VOEvent.release();
        }

        VOEvents.Clear();
        for (int n = 0; n < VOEventAssets.Length; ++n)
        {
            string path = VOEventAssets[n].path;
            string eventPath = "event:/" + langPath + path.Substring(path.LastIndexOf("/") + 1);

            VOEvents.Add(FMOD_StudioSystem.instance.GetEvent(eventPath));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            Subtitles.start(VOEvents[UnityEngine.Random.Range(0, VOEvents.Count)]);
        }
    }
}
