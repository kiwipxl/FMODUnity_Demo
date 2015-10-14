using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class StepSounds : MonoBehaviour {

    public EventRef stepEventRef;
    EventInstance stepEvent;
    ParameterInstance surfaceParam;
    Animator anim;

    bool sand_collide = false;
    bool stone_collide = false;
    bool water_collide = false;

    bool onGround = false;

    void Start() {
        anim = GetComponent<Animator>();

        stepEvent = RuntimeManager.CreateInstance(stepEventRef);
        stepEvent.start();

        stepEvent.getParameter("Surface", out surfaceParam);
    }

    public void footDown() {
        if (onGround && anim.GetFloat("Forward") >= .1f) stepEvent.start();
    }

    void Update() {
        ATTRIBUTES_3D attribs;
        stepEvent.get3DAttributes(out attribs);
        attribs.position.x = transform.position.x;
        attribs.position.y = transform.position.y;
        attribs.position.z = transform.position.z;
        stepEvent.set3DAttributes(attribs);

        if (sand_collide)  surfaceParam.setValue(1);
        if (stone_collide) surfaceParam.setValue(3);
        if (water_collide) surfaceParam.setValue(0);
        sand_collide = false;
        stone_collide = false;
        water_collide = false;

        if (!onGround && anim.GetBool("OnGround")) stepEvent.start();
        onGround = anim.GetBool("OnGround");
    }

    void OnTriggerStay(Collider col)
    {
        if (col.tag == "Stone") stone_collide = true;
        if (col.tag == "Water") water_collide = true;
    }

    void OnCollisionStay(Collision col)
    {
        if (col.gameObject.tag == "Floor") sand_collide = true;
    }
}
