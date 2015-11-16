using UnityEngine;
using UnityEngine.UI;
using FMODUnity;

public class FMODBanner : MonoBehaviour
{
    [EventRef] public string VOPath;

    private void Start() {
        Subtitles.start(RuntimeManager.CreateInstance(VOPath));
	}

    private void Update() {
        if (!Subtitles.isPlaying())
        {
            Color colour = GetComponent<Image>().material.color;
            colour.a -= .025f;
            GetComponent<Image>().material.color = colour;

            if (colour.a <= 0) Destroy(this);
        }
	}
}
