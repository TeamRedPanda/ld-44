using UnityEngine;

[System.Serializable]
public class Product
{
    public string Name = default;

    [Range(1, 100)]
    public int Cost = 50;

    public GameObject Prefab = default;
}
