using UnityEngine;
using FMOD;
using FMOD.Studio;
using FMODUnity;

public class StepSounds : MonoBehaviour {

    public EventRef stepEventRef;
    private EventInstance stepEvent;
    private ParameterInstance surfaceParam;

    private Animator anim;
    private bool onGround = false;
    private CollidingLayers collidingLayers = new CollidingLayers();

    private void Start() {
        anim = GetComponent<Animator>();

        stepEvent = RuntimeManager.CreateInstance(stepEventRef);
        stepEvent.start();

        stepEvent.getParameter("Surface", out surfaceParam);
    }

    //called from the animation window once the player's foot is on the ground
    public void footDown() {
        //if the player is on the ground and is moving forward, play the step sound
        if (onGround && anim.GetFloat("Forward") >= .1f) stepEvent.start();
    }

    private void Update() {
        //always position the event at the camera position so no 3d panning occurs
        stepEvent.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));

        //if the player is colliding with any of these layers, change their surface param
        if (collidingLayers.contains("Sand"))  surfaceParam.setValue(1);
        if (collidingLayers.contains("Stone")) surfaceParam.setValue(3);
        if (collidingLayers.contains("Water")) surfaceParam.setValue(0);
        collidingLayers.reset();

        //if not on ground last frame, but now on ground, play
        //step sound (for landing on your feet).
        if (!onGround && anim.GetBool("OnGround")) stepEvent.start();
        onGround = anim.GetBool("OnGround");
    }

    private void OnTriggerStay(Collider col)
    {
        //add colliding layer
        collidingLayers.add(col.gameObject.layer);
    }

    private void OnCollisionStay(Collision col)
    {
        //add colliding layer
        collidingLayers.add(col.gameObject.layer);
    }
}
