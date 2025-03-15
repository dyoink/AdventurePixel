using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();
    private bool activated;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!activated)
        {
            Player player = collision.GetComponent<Player>();
            if (player != null)
            {
                ActivateCheckpoint();
            }
        }
    }
    private void ActivateCheckpoint()
    {
        activated = true;
        anim.SetTrigger("Activated");
        GameManager.instance.UpdateRespawnPosition(transform);
    }
}
