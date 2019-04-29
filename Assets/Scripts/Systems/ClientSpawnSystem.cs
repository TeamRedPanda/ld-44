using Assets.Scripts.Client;
using System.Collections.Generic;
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

    private List<GameObject> m_SpawnedClients = new List<GameObject>();

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

        m_SpawnedClients.Add(clientGameObject);

        SoundEffectController.PlayEffect(EffectType.ClientEnter);
    }

    public void RemoveAllClients()
    {
        for(int i = m_SpawnedClients.Count - 1; i >= 0; i--) {
            var client = m_SpawnedClients[i];
            client.GetComponent<ClientBehaviour>().Leave();
        }

        Debug.Assert(m_SpawnedClients.Count == 0, "Spawned clients still contain clients after removing.");
    }

    private GameObject GetRandomClientPrefab()
    {
        var index = Random.Range(0, ClientPrefabs.Length);
        return ClientPrefabs[index];
    }

    public void DespawnClient(GameObject client)
    {
        ClientCount--;
        m_SpawnedClients.Remove(client);
        Destroy(client);
    }

    public Transform GetSpawnLocation()
    {
        return SpawnLocation;
    }
}
