using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField]
    private Camera m_Camera = default;

    private NavMeshAgent m_NavMeshAgent;

    void Start()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        if (m_Camera == null) {
            Debug.LogError("Camera not set for player movement controller.");
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            Ray ray = m_Camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo)) {
                Debug.Log($"Walking to {hitInfo.point}");
                m_NavMeshAgent.SetDestination(hitInfo.point);
            }
        }
    }
}
