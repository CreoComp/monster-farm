using Code.Logic.Markers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.Logic
{
    public class BordersSpawnTransform : MonoBehaviour
    {
        [FormerlySerializedAs("maxBorderX")] [SerializeField]
        private Transform _maxBorderX;

        [FormerlySerializedAs("minBorderX")] [SerializeField]
        private Transform _minBorderX;

        [FormerlySerializedAs("maxBorderZ")] [SerializeField]
        private Transform _maxBorderZ;

        [FormerlySerializedAs("minBorderZ")] [SerializeField]
        private Transform _minBorderZ;

        public Vector3 GetRandomPositionInLocation()
        {
            while (true)
            {
                Vector3 position = new Vector3(Random.Range(_minBorderX.position.x, _maxBorderX.position.x), 1, Random.Range(_minBorderZ.position.z, _maxBorderZ.position.z));

                if (!IsEmptyPosition(position))
                {
                    continue;
                }

                return position;
                break;
            }
        }

        private bool IsEmptyPosition(Vector3 position)
        {
            Collider[] hitColliders = Physics.OverlapSphere(position, .5f);
            foreach (var hitCollider in hitColliders)
            {
                if (!hitCollider.GetComponent<GroundMarker>())
                    return false;
            }

            return true;
        }
    }
}