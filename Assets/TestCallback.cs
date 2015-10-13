using System;
using UnityEngine;
using System.Text;
using System.Runtime.InteropServices;
using FMOD;
using FMOD.Studio;

public class TestCallback : MonoBehaviour {

	void Start() {
        FMOD.Studio.EventInstance ev = FMODUnity.RuntimeManager.CreateInstance("event:/Character/Footsteps");
        ev.start();

        Callbacks.setMarkerCallback(ev, callback);

        //ev.setCallback(callback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);
	}

    void Update() {

	}

    public void callback(string markerName)
    {
        UnityEngine.Debug.Log(markerName);
    }
}
