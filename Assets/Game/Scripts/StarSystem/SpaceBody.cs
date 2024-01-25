using SolarSystem;
using UnityEngine;

namespace Game.Scripts.StarSystem
{
    [ExecuteInEditMode]
    public class SpaceBody : MonoBehaviour
    {
        [SerializeField] private SpaceBody parent;

        [SerializeField, Range(-180, 180)] private float tilt;
        [SerializeField, Range(-180, 180)] private float orbitAngle;
        [SerializeField, Min(0.01f)] private float periapsis = 0.01f;
        [SerializeField, Min(0.01f)] private float apoapsis = 0.01f;
        [SerializeField, Min(0.01f)] private float dstMultiplier = 0.01f;

        [SerializeField] private float orbitState;
        [SerializeField] private float orbitPerSecond;
        [SerializeField] private float selfRotationState;
        [SerializeField] private float selfRotationPerSecond;


        [SerializeField] private Transform cachedTransform;

        public Vector3 Position => cachedTransform.position;

        private void OnValidate()
        {
            cachedTransform ??= transform;
        }

        private void Update()
        {
            orbitState += Time.deltaTime * orbitPerSecond;
            if (orbitState is > 1 or < 1)
            {
                orbitState %= 1;
                //add points
            }
            selfRotationState += Time.deltaTime * selfRotationPerSecond;
            if (selfRotationState is > 1 or < 1)
            {
                selfRotationState %= 1;
                //add points
            }

            Vector3 xAxis = new Vector3(Mathf.Cos(orbitAngle * Mathf.Deg2Rad), Mathf.Sin(orbitAngle * Mathf.Deg2Rad), 0);
            Vector3 yAxis = Vector3.forward;

            Vector2 orbitPos = Orbit.CalculatePointOnOrbit(periapsis, apoapsis, orbitState);
            Vector3 selfOffset = (xAxis * orbitPos.x + yAxis * orbitPos.y) * dstMultiplier;
            Quaternion selfRotation = Quaternion.Euler(0, 0, -tilt) * Quaternion.Euler(0, -selfRotationState * 360, 0);

            cachedTransform.position = parent.Position + selfOffset;
            cachedTransform.rotation = selfRotation;
        }
    }
}