using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SellingInterfaceManagement : MonoBehaviour
{
    public GameObject SellingPanelUI;
    public GameObject SellingPriceTextUI;
    public GameObject SellingProductNameUI;
    public GameObject SellingPriceAdjustUI;

    private Product m_sellingProduct;

    private int m_maxLifeAvaiable;
    private float m_sellingProductValue;

    // Start is called before the first frame update
    void Start()
    {
        Product testProduct = new Product();
        testProduct.Name = "TV";
        testProduct.Cost = 30;

        SellProduct( testProduct , 50 );
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SellProduct( Product sellingProduct , int maxLifeAvaiable )
    {
        m_sellingProduct = sellingProduct;

        m_maxLifeAvaiable = maxLifeAvaiable;

        SetupSellingUI();
    }

    public void SetSellingPrice()
    {
        m_sellingProductValue = SellingPriceAdjustUI.GetComponent<Slider>().value;
        SellingPriceTextUI.GetComponent<TextMeshProUGUI>().text = m_sellingProductValue.ToString();
    }

    public void Sell()
    {
        Debug.Log( "Product sold." );
        SellingPanelUI.SetActive( false );
    }

    void SetupSellingUI()
    {
        m_sellingProductValue = m_sellingProduct.Cost;

        SellingProductNameUI.GetComponent<TextMeshProUGUI>().text = m_sellingProduct.Name;

        SellingPriceTextUI.GetComponent<TextMeshProUGUI>().text = m_sellingProductValue.ToString();

        SellingPriceAdjustUI.GetComponent<Slider>().maxValue = m_maxLifeAvaiable;

        SellingPriceAdjustUI.GetComponent<Slider>().value = m_sellingProductValue;

        SellingPanelUI.SetActive( true );
    }
}
