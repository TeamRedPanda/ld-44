using Assets.Scripts.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClientData))]
[RequireComponent(typeof(ActorMovementController))]
public class ClientBehaviour : MonoBehaviour
{
    private ProductDisplayController m_ProductDisplayController;
    private ActorMovementController m_ActorMovementController;

    /// <summary>
    /// State machine for client behaviour :
    /// Idle -> Find an item to look -> WalkingToProduct
    /// WalkingToProduct -> OnArrive -> LookingAtProduct
    /// LookingAtProduct -> Wait a bit in front of item -> Randomly select :
    ///     -> Idle
    ///     -> Buying
    /// Buying -> Wait Player interaction -> Idle
    /// Buying -> After some time has passed -> Idle
    /// </summary>
    private ClientState m_State;
    private int m_LookingProductIndex;

    private ClientData m_ClientData;
    private float m_DecisionTime;

    // Start is called before the first frame update
    void Start()
    {
        m_ProductDisplayController = GameObject.FindObjectOfType<ProductDisplayController>();
        m_ActorMovementController = GetComponent<ActorMovementController>();

        m_ClientData = GetComponent<ClientData>();

        m_State = ClientState.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        switch (m_State) {
            case ClientState.Idle:
                IdleUpdate();
                break;
            case ClientState.LookingAtProduct:
                LookingAtProductUpdate();
                break;
            case ClientState.Buying:
                BuyingUpdate();
                break;
            default:
                break;
        }
    }

    private void IdleUpdate()
    {
        if (m_ClientData.IsHappy == false) {
            m_State = ClientState.Leaving;
            Debug.Log($"{gameObject.name} is leaving the store.");
            return;
        }

        if (m_ProductDisplayController.GrabProductToLook(out m_LookingProductIndex)) {
            m_State = ClientState.WalkingToProduct;
            Vector3 position = m_ProductDisplayController.GetProductPosition(m_LookingProductIndex);

            m_ActorMovementController.MoveTowards(position, 0f, OnArriveAtProduct);
            Debug.Log($"{gameObject.name} is moving towards a product.");
        }
    }

    private void OnArriveAtProduct()
    {
        // @TODO : Replace hard-coded value
        // Wait between 0.5s and 1.5s to decide to buy or not.
        m_DecisionTime = UnityEngine.Random.Range(0.5f, 1.5f);
        m_State = ClientState.LookingAtProduct;
        Debug.Log($"{gameObject.name} has arrived at product.");
    }

    private void LookingAtProductUpdate()
    {
        m_DecisionTime -= Time.deltaTime;

        if (m_DecisionTime <= 0) {
            if (m_ClientData.WantToBuy) {
                m_State = ClientState.Buying;
                // @TODO : Replace hard-coded value
                // Wait between 10s and 15s for the player to offer a price.
                m_DecisionTime = UnityEngine.Random.Range(10f, 15f);
                Debug.Log($"{gameObject.name} decides to buy a product.");
            } else {
                m_State = ClientState.Idle;
                m_ProductDisplayController.StopLookingAtProduct(m_LookingProductIndex);
                Debug.Log($"{gameObject.name} decides to keep looking around.");
            }
        }
    }

    private void BuyingUpdate()
    {
        m_DecisionTime -= Time.deltaTime;

        if (m_DecisionTime <= 0) {
            // Player took too long, we give up.
            m_State = ClientState.Idle;
            m_ClientData.IncreaseHappiness(-10); // @TODO : Remove hard-coded value.
            m_ProductDisplayController.StopLookingAtProduct(m_LookingProductIndex);
            Debug.Log($"{gameObject.name} gave up waiting for an offer.");
        }
    }
}

public enum ClientState
{
    Idle,
    WalkingToProduct,
    LookingAtProduct,
    Buying,
    Leaving
}
