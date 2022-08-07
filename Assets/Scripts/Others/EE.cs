using UnityEngine;

public class EE : MonoBehaviour
{
    [SerializeField] GameObject eeWall = null;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(eeWall);
            Destroy(gameObject);
        }
    }
}