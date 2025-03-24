using UnityEngine;

public class Arrow_trap : Trampoline_trap
{
    [SerializeField] private float Rotatespeed = 100;// tốc độ quay
    [SerializeField] private float recreateTime; // thời gian tạo ra object mới
    [SerializeField] private bool rotateDirection; // true: ngược chiều kim đồng hồ, false: theo chiều kim đồng hồ

    [Space]
    [SerializeField] private Vector3 scaleMax;// kích thước lớn nhất
    [SerializeField] private float scaleUpSpeed;// tốc độ thay đổi kích thước
    private void Start()
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    }
    private void Update()
    {
        ScaleUpObject();
        RotateDirection();
    }

    private void ScaleUpObject()
    {
        if (transform.localScale.x < scaleMax.x)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleMax, scaleUpSpeed * Time.deltaTime);
        }
    }

    private void RotateDirection()
    {
        if (rotateDirection) transform.Rotate(0, 0, -Rotatespeed * Time.deltaTime);
        else
            transform.Rotate(0, 0, Rotatespeed * Time.deltaTime);
    }

    private void DestroyObject()
    {
        GameManager.instance.CreateObject(GameManager.instance.prefab, transform, recreateTime);
        Destroy(gameObject);
    } 
}
