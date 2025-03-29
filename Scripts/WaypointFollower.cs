using UnityEngine;

public class WayPointFollower : MonoBehaviour
{
    [SerializeField] private GameObject[] waypoints;
    private int currentWaypointIndex = 0;
    [SerializeField] private float speed = 2f;
    [SerializeField] private bool leftDirection = false;
    private SpriteRenderer _sr;
    
    private void Start()
    {
        _sr = GetComponent<SpriteRenderer>();
    }
    private void Update()
    {
        if (Vector2.Distance(transform.position, waypoints[currentWaypointIndex].transform.position) < 0.1f)
        {
            if (currentWaypointIndex == waypoints.Length - 1)
            {
                currentWaypointIndex = 0;
            }
            else
            {
                currentWaypointIndex++;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, waypoints[currentWaypointIndex].transform.position, speed * Time.deltaTime);
        if (transform.position.x < waypoints[currentWaypointIndex].transform.position.x)
        {
            if (leftDirection)
            {
                _sr.flipX = true;
            }
            else
            {
                _sr.flipX = false;
            }
        }
        else
        {
            if (leftDirection)
            {
                _sr.flipX = false;
            }
            else
            {
                _sr.flipX = true;
            }
        }
    }
}
