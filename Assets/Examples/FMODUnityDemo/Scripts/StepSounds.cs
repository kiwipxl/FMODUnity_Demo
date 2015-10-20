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
    private int collidingLayers = 0;

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

        if (collidingWithLayer("Sand"))  surfaceParam.setValue(1);
        if (collidingWithLayer("Stone")) surfaceParam.setValue(3);
        if (collidingWithLayer("Water")) surfaceParam.setValue(0);
        collidingLayers = 0;

        //if not on ground last frame, but now on ground, play
        //step sound (for landing on your feet).
        if (!onGround && anim.GetBool("OnGround")) stepEvent.start();
        onGround = anim.GetBool("OnGround");
    }

    /*
    * Get layer bitmasks to calculate collisions per frame, 
    * (so they can be ordered manually)
    */
    private bool collidingWithLayer(string layerName)
    {
        return (collidingLayers & (1 << LayerMask.NameToLayer(layerName))) != 0;
    }

    private void OnTriggerStay(Collider col)
    {
        collidingLayers |= 1 << col.gameObject.layer;
    }

    private void OnCollisionStay(Collision col)
    {
        collidingLayers |= 1 << col.gameObject.layer;
    }
}
