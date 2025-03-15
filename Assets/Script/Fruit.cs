using UnityEngine;

public enum fruitType { Apple, Banana, Cherry, Kiwi, Melon, Orange, Pineapple, Strawberry };
public class Fruit : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private GameObject PickUpFruit;
    private Animator anim;
    [SerializeField] private fruitType fruitType;
    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }
    private void Start()
    {
        gameManager = GameManager.instance;
        SetRandomFruit();
    }
    private void SetRandomFruit()
    {
        if (gameManager.RandomFruit() == false)
        {
            anim.SetFloat("Fruit", (int)fruitType);
            return;
        }
        int random = Random.Range(0, 8);
        anim.SetFloat("Fruit", random);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            gameManager.FruitScore();
            Destroy(gameObject);
            GameObject vfx = Instantiate(PickUpFruit,transform.position,Quaternion.identity);
            
        }
    }
}
