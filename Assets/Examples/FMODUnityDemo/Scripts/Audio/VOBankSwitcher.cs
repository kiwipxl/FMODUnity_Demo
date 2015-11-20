using System;
using UnityEngine;
using FMOD.Studio;
using FMODUnity;

/*
* Handles switching studio banks depending on the current language.
* Only one language bank at a time will be loaded.
*/

public class VOBankSwitcher : MonoBehaviour
{
    //current loaded language bank
    private static Bank currentBank;

    private void Start() {
        // Loads the english bank by default
        switchBankTo(VOLanguage.ENGLISH);
    }

    /*
    * Switches the current VO bank to the new one specified by the language by 
    * unloading the previous bank and loading the new one.
    */
    public static void switchBankTo(VOLanguage newVOLanguage)
    {
        if (VOLocalisation.currentLanguage == newVOLanguage) return;

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
        VOLocalisation.currentLanguage = newVOLanguage;
    }
}
