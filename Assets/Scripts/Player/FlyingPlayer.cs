using UnityEngine;

/// <summary>
/// Clase que permite al jugador atacar y moverse cuando está volando (capítulo 3).
/// </summary>
public class FlyingPlayer : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// La velocidad de movimiento del jugador.
    /// </summary>
    [Header("Movement")]
    [SerializeField] float speed = 4;
    /// <summary>
    /// Componente JoyStickController asignado al Stick izquierdo.
    /// </summary>
    [SerializeField] JoyStickController leftStick = null;
    /// <summary>
    /// Componente JoyStickController asignado al Stick derecho.
    /// </summary>
    [SerializeField] JoyStickController rightStick = null;
    /// <summary>
    /// La rotación inicial del jugador.
    /// </summary>
    Quaternion initialRotation;

    /// <summary>
    /// Momento en el que se realizó el último disparo.
    /// </summary>
    [Header("Shoot")]
    float timeLastShoot;
    /// <summary>
    /// La cadencia de disparo del jugador.
    /// </summary>
    [SerializeField] float cadency = 0.25f;
    /// <summary>
    /// El brazo del jugador.
    /// </summary>
    public GameObject arm;
    /// <summary>
    /// La posición donde se generan las balas.
    /// </summary>
    [SerializeField] Transform shootPoint = null;

    #endregion

    void Start()
    {
        arm.SetActive(true);
        initialRotation = arm.transform.rotation;
    }

    void Update()
    {
        if (Time.timeScale != 0)
        {
            Movement();

            Turn();

            if (Input.GetButtonDown("Cancel"))
            {
                GameManager.gameManager.PauseGame();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "SpawnTrigger3")
        {
            Level3Manager.level3Manager.StartFade();
            Destroy(collision.gameObject);
        }
    }

    /// <summary>
    /// Función que gestiona el movimiento del brazo y la rotación del jugador.
    /// </summary>
    void Turn()
    {
        float h = rightStick.horizontal;
        float v = rightStick.vertical;

        if (h != 0 && v != 0)
        {
            if (h > 0.1f)
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                arm.transform.localScale = new Vector3(1f, 1f, 1f);
            }

            else if (h < -0.1f)
            {
                transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
                arm.transform.localScale = new Vector3(-1f, -1f, 1f);
            }

            float angle = Mathf.Atan2(v, h) * Mathf.Rad2Deg;
            arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

            if (Time.time - timeLastShoot > cadency)
            {
                Shoot();
            }
        }

        else
        {
            arm.transform.rotation = initialRotation;
            transform.localScale = new Vector3(0.5f, 0.5f, 1f);
            arm.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }


    /// <summary>
    /// Función que permite el movimiento del jugador.
    /// </summary>
    void Movement()
    {
        float h = leftStick.horizontal;
        float v = leftStick.vertical;

        transform.Translate(Vector2.right * speed * Time.deltaTime * h);
        transform.Translate(Vector2.up * speed * Time.deltaTime * v);
    }

    /// <summary>
    /// Función que permite que el jugador dispare.
    /// </summary>
    void Shoot()
    {
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletPlayer");

        timeLastShoot = Time.time;

        if (bullet != null)
        {
            bullet.transform.position = shootPoint.position;
            bullet.transform.rotation = shootPoint.rotation;
            bullet.SetActive(true);
        }
    }
}