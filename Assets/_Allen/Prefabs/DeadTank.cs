using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadTank : MonoBehaviour
{
    [SerializeField] private Rigidbody headRigidBody;

    private void Awake()
    {
        float randX = Random.Range(transform.position.x - 1, transform.position.x + 1);
        float randY = Random.Range(transform.position.y - 1, transform.position.y + 1);
        float randZ = Random.Range(transform.position.z - 1, transform.position.z + 1);

        Vector3 exploPos = new Vector3(randX, randY, randZ);

        headRigidBody.AddExplosionForce(1000f, exploPos, 100f);
        Destroy(gameObject, 60f);
    }

}
