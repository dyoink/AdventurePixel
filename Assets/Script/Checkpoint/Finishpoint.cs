using UnityEngine;

public class Finishpoint : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();// kiểm tra xem player đã đi đến finish chưa
        if (player != null)
        {
            anim.SetTrigger("Activated");
            Debug.Log("You win!");
        }
    }
}
