using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Client
{
    class ClientInteraction : MonoBehaviour
    {
        private ClientData m_ClientData;
        private ClientBehaviour m_ClientBehaviour;
       

        private SellingInterfaceManagement m_OfferView;
        private ProductDisplayController m_ProductDisplayController;
        private ScoreSystem m_ScoreSystem;

        void Start()
        {
            m_OfferView = FindObjectOfType<SellingInterfaceManagement>();
            m_ProductDisplayController = GameObject.FindObjectOfType<ProductDisplayController>();
            m_ScoreSystem = FindObjectOfType<ScoreSystem>();

            m_ClientData = GetComponent<ClientData>();
            m_ClientBehaviour = GetComponent<ClientBehaviour>();
        }

        void OnDisable()
        {
            m_OfferView.CancelTransaction();
        }

        public bool TryStartTransaction()
        {
            if (m_ClientBehaviour.IsActiveState(ClientState.Buying) == false)
                return false;

            m_ClientBehaviour.SetState(ClientState.ReceivingOffer);

            var product = m_ClientData.LookingProduct;

            m_OfferView.SellProduct(product, m_ClientData.YearsUntilDeath, OnOfferReceived);
            return true;
        }

        private void OnOfferReceived(int offer)
        {
            var cost = m_ClientData.LookingProduct.Cost;
            bool accept = ShouldAcceptOffer(offer, cost);

            if (accept) {
                m_ScoreSystem.Collect(offer);
                m_ProductDisplayController.SellProduct(m_ClientData.LookingProductIndex);
                m_ClientData.AcceptTransaction(offer, cost);
                m_ClientBehaviour.SetState(ClientState.Idle);
                Debug.Log($"Client {name} accepted offer of {offer}.");
            } else {
                m_ClientData.IncreaseHappiness(-5); // @TODO : Remove hard-coded value.
                m_ClientBehaviour.SetState(ClientState.ProductGiveUp);
                Debug.Log($"Client {name} refused offer.");
            }
        }

        private bool ShouldAcceptOffer(int offer, int cost)
        {
            //(m_ClientData.Happiness - 50) / 100f * 20f; // 10% increase for max happiness. -10% for 0 happiness.
            float happinessFactor = Map(m_ClientData.Happiness, 0, 100, 0.9f, 1.1f);
            Debug.Log($"{happinessFactor} factor");
            var maxAcceptablePrice = Mathf.RoundToInt(offer * happinessFactor);
            // Discount? YES!
            if (offer < maxAcceptablePrice)
                return true;

            return false;
        }

        private float Map(float s, float a1, float a2, float b1, float b2)
        {
            return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
        }
    }
}
