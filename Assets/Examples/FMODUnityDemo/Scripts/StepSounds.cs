using UnityEngine;
using FMOD.Studio;

/*
* Handles step sounds for the player character on different surfaces
*/

public class StepSounds : MonoBehaviour
{
    //step event asset (set in editor)
    public FMODAsset stepEventAsset;
    public FMODAsset jumpOrLandAsset;

    //step event instance
    private EventInstance stepEvent;
    private EventInstance jumpOrLand;

    public GameLogic.CollidingLayers collidingLayers = new GameLogic.CollidingLayers();

    private void Start()
    {
        //create instance of step sound event
        stepEvent = FMOD_StudioSystem.instance.GetEvent(stepEventAsset);
        stepEvent.start();

        jumpOrLand = FMOD_StudioSystem.instance.GetEvent(jumpOrLandAsset);
        jumpOrLand.start();
    }

    /* Called from the animation tab in the editor once the player's foot is on the ground. */
    public void footDown()
    {
        //make sure the player is on the ground and is moving forward
        //and then play the step sounds
        Animator anim = GetComponent<Animator>();
        if (anim.GetBool("OnGround") && anim.GetFloat("Forward") >= .1f) stepEvent.start();
    }

    public void landedOnGround()
    {
        stepEvent.start();
    }

    public void jumpPressed()
    {
        FMOD_StudioSystem.instance.PlayOneShot(jumpOrLandAsset, transform.position);
    }

    private void Update()
    {
        //if the player is colliding with any of these layers, change their surface param.
        if (collidingLayers.contains("Sand")) stepEvent.setParameterValue("Surface", 1);
        if (collidingLayers.contains("Stone")) stepEvent.setParameterValue("Surface", 3);
        if (collidingLayers.contains("Water")) stepEvent.setParameterValue("Surface", 0);
        collidingLayers.reset();
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
