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
    public bool RandomFruit() => randomFruit;
    public void FruitScore()
    {
        fruitScore++;
    }

}
