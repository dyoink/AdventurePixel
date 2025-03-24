using UnityEngine;

public class Trampoline_trap : MonoBehaviour
{
    private Animator anim;
    [SerializeField] private float pushForce;
    [SerializeField] private float pushDuration;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            player.Push(transform.up * pushForce, pushDuration);
            // tranform.up : trục y hướng lên của object (đẩy player lên trên theo hướng của trục y)
            anim.SetTrigger("Activated");
            
        }
    }
    
}
