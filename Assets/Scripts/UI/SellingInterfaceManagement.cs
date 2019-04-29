using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class SellingInterfaceManagement : MonoBehaviour
{
    public GameObject SellingPanelUI;
    public GameObject SellingPriceTextUI;
    public GameObject SellingProductNameUI;
    public GameObject SellingPriceAdjustUI;

    public GameObject ScreenInputBlock;

    private Product m_sellingProduct;

    private int m_maxLifeAvaiable;
    private int m_sellingProductValue;

    private Action<int> m_OnOffer;

    // Start is called before the first frame update
    void Start()
    {
        Product testProduct = new Product();
        testProduct.Name = "TV";
        testProduct.Cost = 30;

        //SellProduct( testProduct , 50, null );
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SellProduct( Product sellingProduct , int maxLifeAvaiable, Action<int> OnOffer )
    {
        if (sellingProduct == null) {
            Debug.LogError("Selling product is null?");
        }

        m_sellingProduct = sellingProduct;

        m_maxLifeAvaiable = maxLifeAvaiable;

        m_OnOffer = OnOffer;

        SetupSellingUI();
    }

    internal void CancelTransaction()
    {
        m_OnOffer = null;
        SellingPanelUI.SetActive(false);
        ScreenInputBlock.SetActive(false);
    }

    public void SetSellingPrice()
    {
        m_sellingProductValue = Mathf.RoundToInt(SellingPriceAdjustUI.GetComponent<Slider>().value);
        SellingPriceTextUI.GetComponent<TextMeshProUGUI>().text = m_sellingProductValue.ToString();
    }

    public void Sell()
    {
        Debug.Log( "Product sold." );
        m_OnOffer?.Invoke(m_sellingProductValue);
        SellingPanelUI.SetActive( false );
        ScreenInputBlock.SetActive(false);
    }

    void SetupSellingUI()
    {
        m_sellingProductValue = m_sellingProduct.Cost;

        SellingProductNameUI.GetComponent<TextMeshProUGUI>().text = m_sellingProduct.Name;

        SellingPriceTextUI.GetComponent<TextMeshProUGUI>().text = m_sellingProductValue.ToString();

        SellingPriceAdjustUI.GetComponent<Slider>().maxValue = m_maxLifeAvaiable;

        SellingPriceAdjustUI.GetComponent<Slider>().value = m_sellingProductValue;

        SellingPanelUI.SetActive( true );
        ScreenInputBlock.SetActive(true);
    }
}
