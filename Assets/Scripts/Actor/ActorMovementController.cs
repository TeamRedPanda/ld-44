using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ActorMovementController : MonoBehaviour
{
    private NavMeshAgent m_NavMeshAgent;
    private event Action m_OnArrived;

    void Awake()
    {
        m_NavMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        CheckArrived();
    }

    private void CheckArrived()
    {
        // If we don't have a callback we don't need to check.
        if (m_OnArrived == null)
            return;

        if (m_NavMeshAgent.pathPending == true)
            return;

        if (m_NavMeshAgent.remainingDistance <= m_NavMeshAgent.stoppingDistance) {
            m_OnArrived?.Invoke();
            m_OnArrived = null;
        }
    }

    public void MoveTowards(Vector3 position, float stoppingDistance, Action OnArrived)
    {
        m_NavMeshAgent.stoppingDistance = stoppingDistance;
        m_NavMeshAgent.SetDestination(position);
        m_OnArrived += OnArrived;
    }
}
