using UnityEngine;

public class Startpoint : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private void OnTriggerExit2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();// kiểm tra xem player đã đi đến start chưa
        if (player != null)
        {
            anim.SetTrigger("Activated");
            Debug.Log("You start!");
        }
    }
}
