using System.Collections;
using UnityEngine;

public class Saw_trap : MonoBehaviour
{
    private Animator anim => GetComponent<Animator>();

    [SerializeField] private Transform[] points;
    [SerializeField] private float speed;
    [SerializeField] private int pointIndex;
    [SerializeField] private float delay;
    private Vector3[] stopPoint;
    private int moveDirection = 1;
    private bool isStop = false;
    private float distanceThreshold = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SaveStopPointPosition();
        transform.position = stopPoint[0];
    }

    private void SaveStopPointPosition()
    {
        stopPoint = new Vector3[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            stopPoint[i] = points[i].position;
           
        }
    }

    void Update()
    {
        if (!isStop)
        {
            Move();
        }
        anim.SetBool("Activated", !isStop);
    }

    //private void Move()
    //{
    //    transform.position = Vector2.MoveTowards(transform.position, stopPoint[pointIndex], speed * Time.deltaTime);
    //    if (Vector2.Distance(transform.position, stopPoint[pointIndex]) < 0.1f)
    //    {
    //        if(pointIndex == stopPoint.Length - 1 || pointIndex == 0)
    //        {
    //            moveDirection *= -1;
    //            transform.Rotate(0, 180,0);
    //            StartCoroutine(Stop(0.5f));
    //        }
    //        pointIndex += moveDirection;

    //    }
    //}
    private void Move()
    {
        transform.position = Vector3.MoveTowards(transform.position, stopPoint[pointIndex], speed * Time.deltaTime);
        if (Vector3.SqrMagnitude(transform.position - stopPoint[pointIndex]) < distanceThreshold * distanceThreshold)
        {
            HandleDirectionChange();
            pointIndex += moveDirection;
        }
    }

    private void HandleDirectionChange()
    {
        if (pointIndex == stopPoint.Length - 1 || pointIndex == 0)
        {
            moveDirection *= -1;
            StartCoroutine(Stop(delay));
            transform.Rotate(0, 180, 0);
        }
    }
    private IEnumerator Stop(float delay)
    {
        isStop = true;
        yield return new WaitForSeconds(delay);
        isStop = false;
    }
}
