using UnityEngine;
using System.Collections;

namespace Steerings
{

    public class KeepDistanceVersatile : SteeringBehaviour
    {
        public GameObject target;
        public float requiredDistance;
        [SerializeField]
        public float alpha = 45;
        private static GameObject surrogateTarget = null;

        public override SteeringOutput GetSteering()
        {

            if (ownKS == null)
                ownKS = GetComponent<KinematicState>();
            return KeepDistanceVersatile.GetSteering(ownKS, target, requiredDistance, alpha);
        }

        public static SteeringOutput GetSteering(KinematicState ownKS, GameObject target,
            float requieredDistance, float alpha)
        {
            // Y axis of the target
            Vector3 targetUp = target.transform.up;
            // We rotate the Y vector alpha angles on the Z axis
            Vector3 rotatedDir = Quaternion.Euler(0, 0, alpha) * targetUp;
            // We add the position of the target to the direction multiplied
            // by the distance to keep from the target
            Vector3 desiredPos = target.transform.position + (rotatedDir * requieredDistance);

            if (KeepDistanceVersatile.surrogateTarget == null)
            {
                KeepDistanceVersatile.surrogateTarget = new GameObject("surrogateee");
            }
            KeepDistanceVersatile.surrogateTarget.transform.position = desiredPos;

            return Seek.GetSteering(ownKS, surrogateTarget);
        }

        public static float GetAngleBetween(Vector2 v1, Vector2 v2)
        {
            float AngleRad = Mathf.Atan2(v1.y - v2.y, v1.x - v2.x);
            float AngleDeg = (180 / Mathf.PI) * AngleRad;
            return AngleDeg;
        }
    }
}