using System.Collections;
using UnityEngine;

public class Saw_trap : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();

    [SerializeField] private Transform[] points;
    [SerializeField] private float speed;
    [SerializeField] private int pointIndex;
    private bool isStop = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = points[0].position;
    }
    void Update()
    {
        anim.SetBool("Activated", !isStop);
        if (!isStop)
        {
            Move();
        }
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, points[pointIndex].position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, points[pointIndex].position) < 0.1f)
        {

            pointIndex = (pointIndex + 1) % points.Length;
            transform.Rotate(0, 180, 0);
            StartCoroutine(Stop(1));
        }
    }

    private IEnumerator Stop(int delay)
    {
        isStop = true;
        yield return new WaitForSeconds(delay);
        isStop = false;
    }
}
