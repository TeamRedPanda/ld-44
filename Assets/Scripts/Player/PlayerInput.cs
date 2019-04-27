﻿using Assets.Scripts.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(ActorMovementController))]
public class PlayerInput : MonoBehaviour
{
    private Camera m_Camera;

    private ActorMovementController m_MovementController;

    void Start()
    {
        m_MovementController = GetComponent<ActorMovementController>();

        m_Camera = Camera.main;
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
                // Did we collide with a client?
                var clientInteraction = hitInfo.collider.gameObject.GetComponentInParent<ClientInteraction>();
                if (clientInteraction != null) {
                    Debug.Log($"Walking to client {clientInteraction.name}");
                    m_MovementController.MoveTowards(hitInfo.point, 0.8f, () => { OnArriveAtClient(clientInteraction); });
                } else {
                    Debug.Log($"Walking to {hitInfo.point}");
                    m_MovementController.MoveTowards(hitInfo.point, 0, OnArrived);
                }
            }
        }
    }

    private void OnArrived()
    {
        Debug.Log("Player arrived at destination.");
    }

    private void OnArriveAtClient(ClientInteraction client)
    {
        client.StartTransaction();
    }
}
