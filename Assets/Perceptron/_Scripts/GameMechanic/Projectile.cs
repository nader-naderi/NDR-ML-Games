using UnityEngine;

namespace PerceptronExample
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] new Rigidbody rigidbody;
        [SerializeField] Vector3Int inputs;

        public Vector3Int Input => inputs;
        private void Start()
        {
            Shoot();

            Destroy(gameObject, 10);
        }
        public void Shoot()
        {
            rigidbody.AddForce(transform.forward * 500);
        }
    }
}
