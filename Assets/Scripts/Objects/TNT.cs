using UnityEngine;

/// <summary>
/// Clase asignada a las cajas explosivas.
/// </summary>
public class TNT : MonoBehaviour
{
    /// <summary>
    /// La explosión que aparece al destruirse las cajas.
    /// </summary>
    GameObject explosion;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
            
            if (explosion != null)
            {
                explosion.SetActive(true);
                explosion.transform.position = transform.position;
                explosion.transform.rotation = transform.rotation;
            }
            
            Boss2Manager.boss2Manager.RespawnTNT();
            Destroy(gameObject);
        }

        else if (collision.gameObject.CompareTag("Enemy"))
        {
            explosion = ObjectPooler.SharedInstance.GetPooledObject("Explosion");
            
            if (explosion != null)
            {
                explosion.SetActive(true);
                explosion.transform.position = transform.position;
                explosion.transform.rotation = transform.rotation;
            }
            
            Boss2Manager.boss2Manager.RespawnTNT();
            collision.gameObject.GetComponent<Dog>().Hurt(1);
            Destroy(gameObject);
        }
    }
}