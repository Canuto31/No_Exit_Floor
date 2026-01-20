using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
}
