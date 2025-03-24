using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("PLAYER")]
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Transform playerSpawnPoint;
    [SerializeField] private float respawnDelay;
    public Player player;

    [Header("Object")]
    public GameObject prefab;

    [Header("FRUIT MANAGEMENT")]
    public bool randomFruit;
    public int fruitScore = 0;
    public int totalFruit = 0;
    public Fruit[] fruits;

    private void Start()
    {
        GetTotalFruits();

    }

    private void GetTotalFruits()
    {
        fruits = Object.FindObjectsByType<Fruit>(FindObjectsSortMode.None);
        totalFruit = fruits.Length;
    }

    private void Awake()
    {
        if (instance == null) instance = this;// tạo ra 1 instance duy nhất của GameManager
        else Destroy(gameObject);
    }
    public void UpdateRespawnPosition(Transform checkpoint) => playerSpawnPoint = checkpoint; // cập nhật vị trí respawn
    public void RespawnPlayer() => StartCoroutine(RespawnPlayerWithDelay(respawnDelay)); // Respawn player sau 1 khoảng thời gian

    private IEnumerator RespawnPlayerWithDelay(float delay) // Respawn player sau 1 khoảng thời gian
    {
        yield return new WaitForSeconds(delay);
        GameObject playerRespawn = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
        player = playerRespawn.GetComponent<Player>();
        
    }
    public void CreateObject(GameObject prefab, Transform position, float delay =0) // tạo ra 1 object sau 1 khoảng thời gian
    {
        StartCoroutine(CoolDownCreateObject(prefab, position, delay));
    }
    private IEnumerator CoolDownCreateObject(GameObject prefab, Transform position, float delay)
    {
        Vector3 vector3 = position.position;
        yield return new WaitForSeconds(delay);
        GameObject arrow = Instantiate(prefab, vector3, Quaternion.identity);
    }
    public bool RandomFruit() => randomFruit;
    public void FruitScore()
    {
        fruitScore++;
    }

}
