using Unity.Mathematics;
using UnityEngine;

namespace Player {
    public class ProximitySpeedBoost : MonoBehaviour {
        [SerializeField] private float maxDistanceTrigger;
        [SerializeField] private GameObject mate;
        
        public float ProximityMultiplier() {
            var playerDistance = math.distance(transform.position, mate.transform.position);
            if ( playerDistance <= maxDistanceTrigger) {
                return 2 - playerDistance/maxDistanceTrigger;
            }

            return 1;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Mathf.Approximately(ProximityMultiplier(), 1) ? Color.gray : Color.yellow ;
            Gizmos.DrawLine(transform.position, mate.transform.position);
        }
    }
}
