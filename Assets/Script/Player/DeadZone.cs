using UnityEngine;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player != null)
        {
            player.Die();
            GameManager.instance.RespawnPlayer();
        }
    }
}
/**
 * using UnityEngine;
using System.Collections;

public class DeadZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.GetComponent<Player>();
        if(player != null)
        {
            player.Die();
            StartCoroutine(RespawnPlayerWithDelay(0.5f));
        }
    }

    private IEnumerator RespawnPlayerWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.instance.RespawnPlayer();
    }
}
 **/
