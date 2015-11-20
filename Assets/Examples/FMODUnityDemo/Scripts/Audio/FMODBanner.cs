using UnityEngine;
using FMODUnity;

/*
* Simple FMOD
*/

public class FMODBanner : MonoBehaviour
{
    [EventRef] public string VOPath;

    public void OnClick()
    {
        Subtitles.start(VOLocalisation.createInstance(VOPath));
    }
}
