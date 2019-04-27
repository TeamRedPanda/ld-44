using UnityEngine;
using UnityEngine.AI;

public class ClientMovementController : MonoBehaviour
{
    public NavMeshAgent clientAgent;

    public GameObject product;

    // Update is called once per frame
    void Update()
    {
        clientAgent.SetDestination(product.transform.position);
    }
}
