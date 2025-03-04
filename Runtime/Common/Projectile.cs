using UnityEngine;

namespace GameCore
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;

        private void Update()
        {
            transform.position += transform.forward * (speed * Time.deltaTime);
        }
    }
}