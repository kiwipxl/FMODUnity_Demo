﻿using UnityEngine;
using FMOD.Studio;
using FMODUnity;

/*
* Handles tank related audio (engine sounds / explosions).
*/

public class TankAudio : MonoBehaviour
{
    //engine and explosion event paths (set in Unity editor to their respective paths)
    public EventRef idleEnginePath;
    public EventRef enginePath;
    public EventRef treadRollingPath;
    public EventRef tankExplosionPath;

    //engine event instances
    private EventInstance idleEngine;
    private EventInstance engine;
    private EventInstance treadRolling;

    private GameLogic.TankShooting tankShooting;                                            //tank shooting component
    private GameLogic.CollidingLayers collidingLayers = new GameLogic.CollidingLayers();    //colliding layer mask

    private void Start()
    {
        tankShooting = GetComponent<GameLogic.TankShooting>();

        //create engine event instances from event paths, which is set in the editor
        idleEngine = RuntimeManager.CreateInstance(idleEnginePath);
        idleEngine.start();
        engine = RuntimeManager.CreateInstance(enginePath);
        engine.start();
        treadRolling = RuntimeManager.CreateInstance(treadRollingPath);
        treadRolling.start();
    }

    private void Update()
    {
        //set the surface parameter value for the tank tread sound
        if (collidingLayers.contains("Sand"))  treadRolling.setParameterValue("T_Surface", 0);
        if (collidingLayers.contains("Water")) treadRolling.setParameterValue("T_Surface", 1);
        collidingLayers.reset();

        if (GameLogic.PerspectiveLogic.isPlayerRig)
        {
            //player is being controlled, set the idle engine to the position of the player.
            idleEngine.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        }
        else
        {
            //tank is being controlled, fake a 2D sound by setting the audio position to the camera listener position
            idleEngine.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.parent.position));
        }
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
        //todo: normalise these in studio

        //set tread rolling speed to forward input (1 if moving forward, 0 otherwise)
        treadRolling.setParameterValue("Speed", Mathf.Abs(forwardInput) * 1500.0f);

        //set the tank engine RPM to the normalised speed
        engine.setParameterValue("RPM", normalisedSpeed * 2000.0f);

        //set engine load to forward input
        engine.setParameterValue("Load", forwardInput);

        //set idle engine RPM to the inverse of the normalised speed of the tank
        idleEngine.setParameterValue("RPM", (1 - normalisedSpeed) * 800.0f);
    }

    public void playTankExplosion()
    {
        //play an explosion sound once and forget about it
        RuntimeManager.PlayOneShot(tankExplosionPath, Camera.main.transform.position);
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