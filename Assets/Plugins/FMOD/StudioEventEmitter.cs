using UnityEngine;
using System;
using System.Collections.Generic;

namespace FMODUnity
{
    [AddComponentMenu("FMOD Studio/FMOD Studio Event Emitter")]
    public class StudioEventEmitter : MonoBehaviour
    {
        public EventRef Event;
        public GameEvent PlayEvent;
        public GameEvent StopEvent;
        public String CollisionTag;
        public bool AllowFadeout = true;
        public bool TriggerOnce = false;

        public ParamRef[] Params;

        private Collider triggerObject;

        private FMOD.Studio.EventDescription eventDescription;
        private FMOD.Studio.EventInstance instance;
        private bool hasTriggered;
        private Rigidbody cachedRigidBody;

        void Start() 
        {                        
            enabled = false;
            cachedRigidBody = GetComponent<Rigidbody>();

            HandleGameEvent(GameEvent.LevelStart);
        }

        void OnDestroy()
        {
            HandleGameEvent(GameEvent.LevelEnd);
        }

        void OnTriggerEnter(Collider other)
        {
            if (String.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag))
            {
                HandleGameEvent(GameEvent.TriggerEnter);
            }
        }

        void OnTriggerExit(Collider other)
        {

            if (String.IsNullOrEmpty(CollisionTag) || other.CompareTag(CollisionTag))
            {
                HandleGameEvent(GameEvent.TriggerExit);
            }
        }

        void OnCollisionEnter()
        {
            HandleGameEvent(GameEvent.CollisionEnter);
        }

        void OnCollisionExit()
        {
            HandleGameEvent(GameEvent.CollisionExit);
        }

        void HandleGameEvent(GameEvent gameEvent)
        {
            if (PlayEvent == gameEvent)
            {
                Play();
            }
            if (StopEvent == gameEvent)
            {
                Stop();
            }
        }

        void Lookup()
        {
            eventDescription = RuntimeManager.GetEventDescription(Event);
        }

        public void Play()
        {
            if (TriggerOnce && hasTriggered)
            {
                return;
            }

            if (eventDescription == null)
            {
                Lookup();
            }

            eventDescription.createInstance(out instance);
            instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));
            foreach(var param in Params)
            {
                instance.setParameterValue(param.Name, param.Value);
            }
            instance.start();

            hasTriggered = true;
        }

        public void Stop()
        {
            if (instance != null)
            {
                instance.stop(AllowFadeout ? FMOD.Studio.STOP_MODE.ALLOWFADEOUT : FMOD.Studio.STOP_MODE.IMMEDIATE);
                instance.release();
                instance = null;
            }
        }

        void Update()
        {
            if (instance != null)
            {
                instance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject, cachedRigidBody));
            }
        }

        void SetParameter(string name, float value)
        {
            if (instance != null)
            {
                instance.setParameterValue(name, value);
            }
        }        
    }
}
