using UnityEngine;
using System.Collections;

public class StepSounds : MonoBehaviour {

    public FMODUnity.EventRef stepEventRef;
    FMOD.Studio.EventInstance stepEvent;
    FMOD.Studio.ParameterInstance surfaceParam;
    Animator anim;

    bool stone_collide = false;
    bool floor_collide = false;

    void Start() {
        stepEvent = FMODUnity.RuntimeManager.CreateInstance(stepEventRef);
        stepEvent.start();

        stepEvent.getParameter("Surface", out surfaceParam);

        anim = GetComponent<Animator>();
        for (int n = 0; n < anim.parameters.Length; ++n)
        {
            Debug.Log(anim.parameters[n].name);
        }
    }

    void Update() {
        FMOD.ATTRIBUTES_3D attribs;
        stepEvent.get3DAttributes(out attribs);
        attribs.position.x = transform.position.x;
        attribs.position.y = transform.position.y;
        attribs.position.z = transform.position.z;
        stepEvent.set3DAttributes(attribs);

        AnimatorStateInfo stateInfo = anim.GetCurrentAnimatorStateInfo(0);
        FMOD.Studio.PLAYBACK_STATE state;
        stepEvent.getPlaybackState(out state);
        if (stateInfo.IsName("Grounded"))
        {
            if (state == FMOD.Studio.PLAYBACK_STATE.STOPPED) stepEvent.start();
            stepEvent.setPitch(Mathf.Clamp(anim.GetFloat("Forward") * 2.0f, 1.0f, 2.0f));
            stepEvent.setVolume(anim.GetFloat("Forward"));
        }else {
            if (state == FMOD.Studio.PLAYBACK_STATE.PLAYING) stepEvent.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        }

        if (floor_collide) surfaceParam.setValue(2);
        if (stone_collide) surfaceParam.setValue(3);
        floor_collide = false;
        stone_collide = false;
	}

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Stone") stone_collide = true;
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Floor") floor_collide = true;
    }
}
