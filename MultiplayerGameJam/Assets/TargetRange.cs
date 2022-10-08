using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRange : MonoBehaviour
{
    private LineRenderer line;
    private CircleCollider2D circle;
    private Vector2[] points = new Vector2[2];
    [SerializeField] private float range = 1.5f;
    
    void Start()
    {
        circle = GetComponent<CircleCollider2D>();
        line = GetComponent<LineRenderer>();
        line.startColor = Color.black;
        line.endColor = Color.black;
        line.positionCount = 2;
    }
    
    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(range * 2, range * 2, range * 2);
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 startPoint = new Vector2(transform.position.x, transform.position.y);
        Vector2 endPoint = mousePosition;
        points[0] = startPoint;
        points[1] = Vector2.MoveTowards(startPoint, endPoint, range);
        line.SetPosition(0, points[0]);
        line.SetPosition(1, points[1]);
    }
}
