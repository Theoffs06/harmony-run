using Player;
using UnityEngine;

public class ProximitySpeedBoost : MonoBehaviour
{
    [SerializeField] private float maxDistanceTrigger;

    [SerializeField] private GameObject mate;

    private PlayerMovement _playerMovement;

    public void OnCreate()
    {
        _playerMovement = GetComponent<PlayerMovement>();
    }

    public float ProximityMultiplier()
    {
        var playerDistance = Vector3.Distance(transform.position, mate.transform.position);
        if ( playerDistance <= maxDistanceTrigger)
        {
            return 2 - playerDistance/maxDistanceTrigger;
        }
        else
        {
            return 1;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = ProximityMultiplier() == 1 ? Color.gray : Color.yellow ;
        Gizmos.DrawLine(transform.position, mate.transform.position);
    }

}
