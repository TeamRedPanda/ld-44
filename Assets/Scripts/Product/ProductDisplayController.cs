using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductDisplayController : MonoBehaviour
{
    public List<Product> ProductAssets;

    public List<GameObject> ProductDisplayAnchors;

    private Product[] m_ProductsInDisplay;
    private List<int> m_FreeDisplayIndexes;

    void Start()
    {
        // Create data storage for each product display.
        m_ProductsInDisplay = new Product[ProductDisplayAnchors.Count];
        m_FreeDisplayIndexes = new List<int>();

        for (int i = 0; i < m_ProductsInDisplay.Length; i++)
            m_FreeDisplayIndexes.Add(i);

        FillEmptyProductDisplays();
    }

    public void FillEmptyProductDisplays()
    {
        Debug.Log($"Filling {m_ProductsInDisplay.Length} product displays.");
        for (int i = 0; i < m_ProductsInDisplay.Length; i++) {
            // Only fill empty displays.
            if (m_ProductsInDisplay[i] != null)
                continue;

            var productAsset = ProductAssets.RandomElement();
            Instantiate(productAsset.Prefab, ProductDisplayAnchors[i].transform);
            m_ProductsInDisplay[i] = productAsset;
        }
    }

    internal void SellProduct(int index)
    {
        PlayProductSoldSound(m_ProductsInDisplay[index]);
        m_ProductsInDisplay[index] = null;
        var productGameObject = ProductDisplayAnchors[index].gameObject.transform.GetChild(0).gameObject;
        GameObject.Destroy(productGameObject); // @TODO: Properly remove the item.

    }

    public void ResetStoreProducts()
    {
        // Remove products in display.
        for (int i = 0; i < m_ProductsInDisplay.Length; i++) {
            var product = m_ProductsInDisplay[i];

            // Don't touch empty displays.
            if (product == null)
                continue;

            m_ProductsInDisplay[i] = null;
            var productGameObject = ProductDisplayAnchors[i].gameObject.transform.GetChild(0).gameObject;
            GameObject.Destroy(productGameObject); // @TODO: Properly remove the item.
        }

        // Fill products to display.
        FillEmptyProductDisplays();

        // Add products to free indexes.
        m_FreeDisplayIndexes.Clear();
        for (int i = 0; i < m_ProductsInDisplay.Length; i++)
            m_FreeDisplayIndexes.Add(i);
    }

    private void PlayProductSoldSound(Product product)
    {
        // @TODO: Implement unique sound fx
        SoundEffectController.PlayEffect(EffectType.ItemSold);
    }

    /// <summary>
    /// Returns a product not currently being seen by a client.
    /// </summary>
    /// <returns>True if there's one available</returns>
    public bool GrabProductToLook(out int index)
    {
        index = 0;
        if (m_FreeDisplayIndexes.Count == 0)
            return false;

        index = m_FreeDisplayIndexes.PopRandom();
        return true;
    }

    public int GetFreeProductCount()
    {
        return m_FreeDisplayIndexes.Count;
    }

    public void StopLookingAtProduct(int index)
    {
        if (m_FreeDisplayIndexes.Contains(index)) {
            Debug.LogError("Client trying to free product not being looked at.");
        }

        m_FreeDisplayIndexes.Add(index);
    }
    public Product GetProduct(int index)
    {
        return m_ProductsInDisplay[index];
    }

    internal Vector3 GetProductPosition(int productIndex)
    {
        return ProductDisplayAnchors[productIndex].transform.position;
    }
}
