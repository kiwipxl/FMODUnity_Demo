using UnityEngine;
using System.Collections;

public class StepSounds : MonoBehaviour {

    FMODUnity.StudioEventEmitter emitter;
    FMOD.Studio.ParameterInstance surface_param;
    FMOD.Studio.EventInstance step_ev;
    FMOD.Studio.System studio_sys;

    void Start () {
        emitter = GetComponent<FMODUnity.StudioEventEmitter>();

        studio_sys = FMODUnity.RuntimeManager.StudioSystem;

        FMOD.Studio.EventDescription ev_desc;
        studio_sys.getEvent(emitter.Event.Path, out ev_desc);
        ev_desc.createInstance(out step_ev);

        step_ev.getParameter("Surface", out surface_param);
        surface_param.setValue(2);
        emitter.Play();
    }

    void Update () {
	}
}
