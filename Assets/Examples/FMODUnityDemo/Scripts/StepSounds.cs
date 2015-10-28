using UnityEngine;
using FMOD.Studio;
using FMODUnity;

/*
* Handles step sounds for the player character on different surfaces
*/

public class StepSounds : MonoBehaviour {

    public EventRef stepEventRef;
    private EventInstance stepEvent;

    private Animator anim;
    private bool onGround = false;
    private CollidingLayers collidingLayers = new CollidingLayers();

    private void Start() {
        anim = GetComponent<Animator>();

        //create instance of step sound event
        stepEvent = RuntimeManager.CreateInstance(stepEventRef);
        stepEvent.start();
    }

    //called from the animation tab in the editor once the player's foot is on the ground.
    public void footDown() {
        //make sure the player is on the ground and is moving forward
        //and then play the step sound if they are.
        if (onGround && anim.GetFloat("Forward") >= .1f) stepEvent.start();
    }

    private void Update() {
        //if the player is colliding with any of these layers, change their surface param.
        if (collidingLayers.contains("Sand"))  stepEvent.setParameterValue("Surface", 1);
        if (collidingLayers.contains("Stone")) stepEvent.setParameterValue("Surface", 3);
        if (collidingLayers.contains("Water")) stepEvent.setParameterValue("Surface", 0);
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
