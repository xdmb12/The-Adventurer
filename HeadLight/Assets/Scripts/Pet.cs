using UnityEngine;

public class Pet : MonoBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private float speed;

    private void FixedUpdate()
    {
        transform.position = Vector3.MoveTowards(transform.position, points[0].position, speed);

        FlipSprite();
    }

    private void FlipSprite()
    {
        if (transform.position.x < points[1].position.x)
        {
            transform.eulerAngles = new Vector3(0, 0, 0);
        }
        else if (transform.position.x > points[1].position.x)
        {
            transform.eulerAngles = new Vector3(0, 180, 0);
        }
    }
}
