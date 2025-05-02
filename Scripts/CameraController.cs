using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private float yShift = 0;

    private void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y+yShift, transform.position.z);
    }
}