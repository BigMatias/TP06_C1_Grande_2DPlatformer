using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Vector3 cameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        cameraPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        cameraPosition.x = target.position.x;
        transform.position = cameraPosition;
    }
}
