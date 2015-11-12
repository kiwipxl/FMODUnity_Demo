using UnityEngine;
using FMOD.Studio;

/*
* Handles tank related audio (engine sounds / explosions).
*/

public class TankAudio : MonoBehaviour
{
    //engine and explosion event assets (set in editor)
    public FMODAsset idleEngineAsset;
    public FMODAsset engineAsset;
    public FMODAsset treadRollingAsset;
    public FMODAsset tankExplosionAsset;

    //engine event instances
    private EventInstance idleEngine;
    private EventInstance engine;
    private EventInstance treadRolling;

    //colliding layer mask (to check which layers the tank is colliding with)
    private GameLogic.CollidingLayers collidingLayers = new GameLogic.CollidingLayers();

    private void Start()
    {
        //create engine event instances from event assets, which is set in the editor
        idleEngine = FMOD_StudioSystem.instance.GetEvent(idleEngineAsset);
        idleEngine.start();

        engine = FMOD_StudioSystem.instance.GetEvent(engineAsset);
        engine.start();

        treadRolling = FMOD_StudioSystem.instance.GetEvent(treadRollingAsset);
        treadRolling.start();
    }

    private void Update()
    {
        //set the surface parameter value for the tank tread sound
        if (collidingLayers.contains("Sand"))  treadRolling.setParameterValue("T_Surface", 0);
        if (collidingLayers.contains("Water")) treadRolling.setParameterValue("T_Surface", 1);
        collidingLayers.reset();


        //set idle engine position to the position of the tank
        idleEngine.set3DAttributes(UnityUtil.to3DAttributes(transform.position));
    }

    private void OnDisable()
    {
        //if the tank has been destroyed, stop all engine sounds

        idleEngine.stop(STOP_MODE.IMMEDIATE);

        engine.stop(STOP_MODE.IMMEDIATE);

        treadRolling.stop(STOP_MODE.IMMEDIATE);
    }

    /* Called from TankMovement component to update driving audio */
    public void updateDriving(float normalisedSpeed, float forwardInput, float turnInput)
    {
        //set tread rolling speed to forward input (1 if moving forward, 0 otherwise)
        treadRolling.setParameterValue("Speed", Mathf.Abs(forwardInput));


        //set the tank engine RPM to the normalised speed
        engine.setParameterValue("RPM", normalisedSpeed);


        //set engine load to forward input (1 if moving forward, 0 otherwise)
        engine.setParameterValue("Load", forwardInput);


        //set idle engine RPM to the inverse of the normalised speed of the tank
        idleEngine.setParameterValue("RPM", (1 - normalisedSpeed) * .5f);
    }

    public void playTankExplosion()
    {
        //play an explosion sound once on the tank and then forget about it
        FMOD_StudioSystem.instance.PlayOneShot(tankExplosionAsset, transform.position);
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