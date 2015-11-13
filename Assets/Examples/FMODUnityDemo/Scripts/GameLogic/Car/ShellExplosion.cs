using UnityEngine;

/*
* Handles the logic for when a shell explodes.
*/

namespace GameLogic
{
    public class ShellExplosion : MonoBehaviour
    {
        public LayerMask tankMask;                         // Used to filter what the explosion affects, this should be set to "Players".
        public ParticleSystem ExplosionParticles;         // Reference to the particles that will play on explosion.
        public float MaxDamage = 100f;                    // The amount of damage done if the explosion is centred on a tank.
        public float ExplosionForce = 1000f;              // The amount of force added to a tank at the centre of the explosion.
        public float MaxLifeTime = 2f;                    // The time in seconds before the shell is removed.
        public float ExplosionRadius = 5f;                // The maximum distance away from the explosion tanks can be and are still affected.
        [HideInInspector] public GameObject tankParent;

        private void Start ()
        {
            // If it isn't destroyed by then, destroy the shell after it's lifetime.
            Destroy (gameObject, MaxLifeTime);
        }

        private void OnTriggerEnter (Collider other)
        {
            if (other.gameObject == tankParent) return;

			// Collect all the colliders in a sphere from the shell's current position to a radius of the explosion radius.
            Collider[] colliders = Physics.OverlapSphere (transform.position, ExplosionRadius, tankMask);

            // Go through all the colliders...
            for (int i = 0; i < colliders.Length; i++)
            {
                // ... and find their rigidbody.
                Rigidbody targetRigidbody = colliders[i].GetComponent<Rigidbody>();
                if (tankParent == targetRigidbody.gameObject) continue;

                // If they don't have a rigidbody, go on to the next collider.
                if (!targetRigidbody)
                    continue;

                // Add an explosion force.
                targetRigidbody.AddExplosionForce(ExplosionForce, transform.position, ExplosionRadius);
            }

            // Unparent the particles from the shell.
            ExplosionParticles.transform.parent = null;

            // Play the particle system.
            ExplosionParticles.Play();

            GetComponent<ShellAudio>().playShellExplosion(transform.position, LayerMask.LayerToName(other.gameObject.layer));

            // Once the particles have finished, destroy the gameobject they are on.
            Destroy (ExplosionParticles.gameObject, ExplosionParticles.duration);

            // Destroy the shell.
            Destroy (gameObject);
        }

        private float CalculateDamage (Vector3 targetPosition)
        {
            // Create a vector from the shell to the target.
            Vector3 explosionToTarget = targetPosition - transform.position;

            // Calculate the distance from the shell to the target.
            float explosionDistance = explosionToTarget.magnitude;

            // Calculate the proportion of the maximum distance (the explosionRadius) the target is away.
            float relativeDistance = (ExplosionRadius - explosionDistance) / ExplosionRadius;

            // Calculate damage as this proportion of the maximum possible damage.
            float damage = relativeDistance * MaxDamage;

            // Make sure that the minimum damage is always 0.
            damage = Mathf.Max (0f, damage);

            return damage;
        }
    }
}