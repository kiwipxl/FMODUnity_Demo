using UnityEngine;
using FMOD.Studio;
using FMODUnity;

public class TankAudio : MonoBehaviour
{
    //engine and explosion event paths (set in Unity editor)
    public EventRef idleEnginePath;
    public EventRef enginePath;
    public EventRef treadRollingPath;
    public EventRef tankExplosionPath;

    //engine event instances
    private EventInstance idleEngine;
    private EventInstance engine;
    private EventInstance treadRolling;

    private GameLogic.TankShooting tankShooting;                        //tank shooting component
    private CollidingLayers collidingLayers = new CollidingLayers();    //colliding layer mask

    private void Start()
    {
        tankShooting = GetComponent<GameLogic.TankShooting>();

        //create engine event instances from paths set
        idleEngine = RuntimeManager.CreateInstance(idleEnginePath);
        idleEngine.start();
        engine = RuntimeManager.CreateInstance(enginePath);
        engine.start();
        treadRolling = RuntimeManager.CreateInstance(treadRollingPath);
        treadRolling.start();
    }

    private void Update()
    {
        //set surface parameter values for the tank tread sound
        if (collidingLayers.contains("Sand"))  treadRolling.setParameterValue("T_Surface", 0);
        if (collidingLayers.contains("Water")) treadRolling.setParameterValue("T_Surface", 1);
        collidingLayers.reset();

        //todo (rich):
        //3d panner can't be removed in engine!
        engine.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));

        if (PerspectiveSwitch.isPlayerRig) idleEngine.set3DAttributes(RuntimeUtils.To3DAttributes(transform));
        else                               idleEngine.set3DAttributes(RuntimeUtils.To3DAttributes(Camera.main.transform.position));
    }

    private void OnDisable()
    {
        idleEngine.stop(STOP_MODE.IMMEDIATE);
        engine.stop(STOP_MODE.IMMEDIATE);
        treadRolling.stop(STOP_MODE.IMMEDIATE);
    }

    public void updateDriving(float normalisedSpeed, float forwardInput, float turnInput)
    {
        //todo (rich):
        //these calculations should probably be normalised!
        //or at least a constant

        treadRolling.setParameterValue("Speed", Mathf.Abs(forwardInput) * 1500.0f);

        engine.setParameterValue("RPM", normalisedSpeed * 2000.0f);
        engine.setParameterValue("Load", forwardInput);

        idleEngine.setParameterValue("RPM", (1 - normalisedSpeed) * 800.0f);
    }

    public void playTankExplosion()
    {
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