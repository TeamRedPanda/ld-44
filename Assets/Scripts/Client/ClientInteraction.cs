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

        public void StartTransaction()
        {
            m_ClientBehaviour.SetState(ClientState.ReceivingOffer);

            var product = m_ClientData.LookingProduct;

            m_OfferView.SellProduct(product, m_ClientData.YearsUntilDeath, OnOfferReceived);
        }

        private void OnOfferReceived(int offer)
        {
            bool accept = ShouldAcceptOffer(offer, m_ClientData.LookingProduct.Cost);

            if (accept) {
                m_ScoreSystem.Collect(offer);
                m_ProductDisplayController.SellProduct(m_ClientData.LookingProductIndex);
                m_ClientData.IncreaseAge(offer);
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
            // Discount? YES!
            if (offer < cost)
                return true;

            // @TODO: Add proper calculation. Should be affected by happiness?
            return false;
        }
    }
}
