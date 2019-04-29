using Assets.Scripts.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ClientData))]
[RequireComponent(typeof(ActorMovementController))]
public class ClientBehaviour : MonoBehaviour
{
    public EmoteView m_EmoteView;

    private ProductDisplayController m_ProductDisplayController;
    private ActorMovementController m_ActorMovementController;
    private ClientSpawnSystem m_ClientSpawnSystem;
    private ScoreSystem m_ScoreSystem;
    private DeathSystem m_DeathSystem;

    public bool IsActiveState(ClientState state)
    {
        return m_StateMachine.GetState() == state;
    }

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
    private StateMachine<ClientState> m_StateMachine = new StateMachine<ClientState>();

    private ClientData m_ClientData;
    private float m_DecisionTime;

    private const float c_BuyingEmoteFrequency = 2.5f;
    private float m_BuyingEmoteCooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        FindReferences();
        SetupStates();
    }

    private void FindReferences()
    {
        m_ProductDisplayController  = FindObjectOfType<ProductDisplayController>();
        m_ClientSpawnSystem         = FindObjectOfType<ClientSpawnSystem>();
        m_ScoreSystem               = FindObjectOfType<ScoreSystem>();
        m_DeathSystem               = FindObjectOfType<DeathSystem>();

        m_ActorMovementController = GetComponent<ActorMovementController>();
        m_ClientData = GetComponent<ClientData>();
    }

    private void SetupStates()
    {
        m_StateMachine.AddState(ClientState.Idle, null, IdleUpdate, null);
        m_StateMachine.AddState(ClientState.LookingAtProduct, LookingAtProductEnter, LookingAtProductUpdate, null);
        m_StateMachine.AddState(ClientState.WalkingToProduct, null, null, null);
        m_StateMachine.AddState(ClientState.Leaving, LeavingEnter, null, null);
        m_StateMachine.AddState(ClientState.Buying, BuyingEnter, BuyingUpdate, null);
        m_StateMachine.AddState(ClientState.ProductGiveUp, ProductGiveUp, null, null);
        m_StateMachine.AddState(ClientState.ReceivingOffer, null, null, null);
        m_StateMachine.AddState(ClientState.Dying, DyingEnter, null, null);

        m_StateMachine.SetState(ClientState.Idle);
    }

    private void DyingEnter()
    {
        // @TODO: Remove hard-coded value.
        m_DeathSystem.PlayDeathAnimation(this.transform.position);
        m_ScoreSystem.Collect(100);
        m_ClientSpawnSystem.DespawnClient(this.gameObject);

        SoundEffectController.PlayEffect(EffectType.ClientDies);
    }

    private void LeavingEnter()
    {
        var exitPosition = m_ClientSpawnSystem.GetSpawnLocation();
        m_ActorMovementController.MoveTowards(exitPosition.position, 0f, OnExitArrive);
    }

    private void OnExitArrive()
    {
        m_ClientSpawnSystem.DespawnClient(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        m_StateMachine.OnUpdate();
    }

    public void SetState(ClientState state)
    {
        m_StateMachine.SetState(state);
    }

    private void IdleUpdate()
    {
        if (m_ClientData.IsHappy == false) {
            Debug.Log($"{gameObject.name} is leaving the store.");
            m_StateMachine.SetState(ClientState.Leaving);
            return;
        }

        if (m_ClientData.IsDead) {
            m_StateMachine.SetState(ClientState.Dying);
            return;
        }

        if (m_ProductDisplayController.GrabProductToLook(out m_ClientData.LookingProductIndex)) {
            Vector3 position = m_ProductDisplayController.GetProductPosition(m_ClientData.LookingProductIndex);

            m_ActorMovementController.MoveTowards(position, 0f, OnArriveAtProduct);
            Debug.Log($"{gameObject.name} is moving towards a product.");
            m_StateMachine.SetState(ClientState.WalkingToProduct);
        } else {
            Debug.Log($"Store is empty, {gameObject.name} is leaving");
            m_StateMachine.SetState(ClientState.Leaving);
        }
    }

    private void OnArriveAtProduct()
    {
        Debug.Log($"{gameObject.name} has arrived at product.");
        m_StateMachine.SetState(ClientState.LookingAtProduct);
    }

    private void LookingAtProductEnter()
    {
        // @TODO : Replace hard-coded value
        // Wait between 0.5s and 1.5s to decide to buy or not.
        m_DecisionTime = UnityEngine.Random.Range(0.5f, 1.5f);
    }

    private void LookingAtProductUpdate()
    {
        m_DecisionTime -= Time.deltaTime;

        if (m_DecisionTime <= 0) {
            if (m_ClientData.WantToBuy) {
                Debug.Log($"{gameObject.name} decides to buy a product.");
                m_StateMachine.SetState(ClientState.Buying);
            } else {
                Debug.Log($"{gameObject.name} decides to keep looking around.");
                m_StateMachine.SetState(ClientState.ProductGiveUp);
            }
        }
    }

    private void BuyingEnter()
    {
        // @TODO : Replace hard-coded value
        // Wait between 10s and 15s for the player to offer a price.
        m_DecisionTime = UnityEngine.Random.Range(10f, 15f);
        m_EmoteView.ShowEmote(EmoteType.Buying);

        m_BuyingEmoteCooldown = 0f;
    }

    private void BuyingUpdate()
    {
        m_DecisionTime -= Time.deltaTime;

        if (m_DecisionTime <= 0) {
            // Player took too long, we give up.
            m_ClientData.IncreaseHappiness(-10); // @TODO : Remove hard-coded value.
            Debug.Log($"{gameObject.name} gave up waiting for an offer.");
            m_StateMachine.SetState(ClientState.ProductGiveUp);
            m_EmoteView.ShowEmote(EmoteType.Angry);
        }

        m_BuyingEmoteCooldown += Time.deltaTime;

        if (m_BuyingEmoteCooldown >= c_BuyingEmoteFrequency) {
            m_EmoteView.ShowEmote(EmoteType.Buying);
            m_BuyingEmoteCooldown = 0f;
        }
    }

    private void ProductGiveUp()
    {
        m_ProductDisplayController.StopLookingAtProduct(m_ClientData.LookingProductIndex);
        m_StateMachine.SetState(ClientState.Idle);
    }
}

public enum ClientState
{
    Idle,
    WalkingToProduct,
    LookingAtProduct,
    Buying,
    Leaving,
    ProductGiveUp,
    ReceivingOffer,
    Dying
}
