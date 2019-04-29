﻿using Assets.Scripts.Client;
using UnityEngine;

public class ClientSpawnSystem : MonoBehaviour
{
    public Transform ClientParent;
    public Transform SpawnLocation;

    public GameObject[] ClientPrefabs;

    private int ClientCount = 0;

    public float MinimumSpawnInterval = 1f;
    public float MaximumSpawnInterval = 3f;
    private float TimeUntilNextCheck;

    public int MaxClientsInStore = 5;

    public int MinimumSpawnAge = 20;
    public int MaximumSpawnAge = 60;

    // Start is called before the first frame update
    void Start()
    {
        ResetSpawnCooldown();
    }

    // Update is called once per frame
    void Update()
    {
        TimeUntilNextCheck -= Time.deltaTime;

        if (TimeUntilNextCheck > 0)
            return;

        if (ClientCount >= MaxClientsInStore) {
            ResetSpawnCooldown();
            return;
        }

        var age = Random.Range(MinimumSpawnAge, MaximumSpawnAge + 1);
        SpawnClient(age);
        ResetSpawnCooldown();
    }

    private void ResetSpawnCooldown()
    {
        TimeUntilNextCheck = Random.Range(MinimumSpawnInterval, MaximumSpawnInterval);
    }

    public void SpawnClient(int startingAge)
    {
        ClientCount++;

        GameObject ClientPrefab = GetRandomClientPrefab();
        var clientGameObject = Instantiate(ClientPrefab, SpawnLocation.position, SpawnLocation.rotation, ClientParent);
        var clientData = clientGameObject.GetComponent<ClientData>();
        clientData.Age = startingAge;

        SoundEffectController.PlayEffect(EffectType.ClientEnter);
    }

    private GameObject GetRandomClientPrefab()
    {
        var index = Random.Range(0, ClientPrefabs.Length);
        return ClientPrefabs[index];
    }

    public void DespawnClient(GameObject client)
    {
        ClientCount--;
        Destroy(client);
    }

    public Transform GetSpawnLocation()
    {
        return SpawnLocation;
    }
}
