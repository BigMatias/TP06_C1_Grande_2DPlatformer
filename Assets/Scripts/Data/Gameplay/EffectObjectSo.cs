using UnityEngine;

public class EffectObjectSo : MonoBehaviour
{
    [SerializeField] private float time;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, time);
    }
}