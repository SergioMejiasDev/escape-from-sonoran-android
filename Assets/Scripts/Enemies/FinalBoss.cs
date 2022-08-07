using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Clase que controla las funciones del tercer y último jefe.
/// </summary>
public class FinalBoss : MonoBehaviour
{
    #region Variables

    /// <summary>
    /// La salud inicial del enemigo.
    /// </summary>
    [Header("Health")]
    readonly float maxHealth = 750f;
    /// <summary>
    /// La salud actual del enemigo.
    /// </summary>
    float health;
    /// <summary>
    /// La imagen de la barra de salud completa del enemigo.
    /// </summary>
    [SerializeField] Image fullBattery = null;

    /// <summary>
    /// El jugador.
    /// </summary>
    [Header("Attack")]
    GameObject player;
    /// <summary>
    /// Las balas que utilizará el enemigo.
    /// </summary>
    GameObject bullet;
    /// <summary>
    /// El cañón del enemigo.
    /// </summary>
    [SerializeField] GameObject cannon = null;
    /// <summary>
    /// El brazo del enemigo.
    /// </summary>
    [SerializeField] GameObject arm = null;
    /// <summary>
    /// La posición donde se generan los ataques en el cañón.
    /// </summary>
    [SerializeField] Transform shootPointCannon = null;
    /// <summary>
    /// La posición donde se generan los ataques en el brazo.
    /// </summary>
    [SerializeField] Transform shootPointArm = null;
    /// <summary>
    /// Verdadero si puede moverse el cañón.
    /// </summary>
    bool moveCannon = true;
    /// <summary>
    /// Verdadero si puede moverse el brazo.
    /// </summary>
    bool moveArm;
    /// <summary>
    /// La rotación inicial del cañón.
    /// </summary>
    Quaternion cannonStartingRotation;
    /// <summary>
    /// La rotación inicial del brazo.
    /// </summary>
    Quaternion armStartingRotation;
    /// <summary>
    /// El tiempo que transcurre entre ataques.
    /// </summary>
    float timeBetweenAttacks = 8;
    /// <summary>
    /// Verdadero si el enemigo ha entrado en el modo alternativo (tiene la salud baja y ataca más rápido).
    /// </summary>
    bool mode2 = false;

    /// <summary>
    /// Momento en el que se realizó el último disparo.
    /// </summary>
    [Header("Attack Shoot")]
    float timeLastShoot;
    /// <summary>
    /// La cadencia de disparo del enemigo.
    /// </summary>
    readonly float cadency = 1.0f;
    /// <summary>
    /// Verdadero si el enemigo puede disparar balas por el cañón.
    /// </summary>
    bool shoot0 = true;

    /// <summary>
    /// Cadencia del ataque del laser.
    /// </summary>
    [Header("Attack Laser")]
    float cadency1;
    /// <summary>
    /// Velocidad del ataque del laser.
    /// </summary>
    float attackSpeed1 = 0.2f;
    /// <summary>
    /// Máximo de disparos del laser por repetición.
    /// </summary>
    int maxShoots1 = 10;
    /// <summary>
    /// Máximo de repeticiones del ataque del laser.
    /// </summary>
    int maxRepetitions1;

    /// <summary>
    /// Cadencia del ataque de los misiles.
    /// </summary>
    [Header("Attack Missile")]
    float cadency2 = 0.75f;
    /// <summary>
    /// Máximo de disparos con el ataque de los misiles.
    /// </summary>
    float maxShoots2;
    /// <summary>
    /// La posición donde se generarán los misiles en el eje X.
    /// </summary>
    [SerializeField] Transform spawnZoneRight2 = null;

    /// <summary>
    /// La bola de energía que crea el enemigo.
    /// </summary>
    [Header("Attack Energy Ball")]
    [SerializeField] GameObject energyBall = null;
    /// <summary>
    /// La bola de energía que dispara el enemigo tras crearla.
    /// </summary>
    [SerializeField] GameObject whiteBall = null;

    /// <summary>
    /// La cadencia del ataque de los meteoritos.
    /// </summary>
    [Header("Attack Meteorite")]
    float cadency4 = 0.5f;
    /// <summary>
    /// El máximo de disparos del ataque de los meteoritos.
    /// </summary>
    int maxShoots4;
    /// <summary>
    /// La posición donde se generarán los meteoritos en el eje Y.
    /// </summary>
    [SerializeField] Transform spawnZoneUp4 = null;

    /// <summary>
    /// La posición donde se detendrá el ataque de la estrella antes de girar.
    /// </summary>
    [Header("Attack Star")]
    [SerializeField] Transform centerRefence = null;
    /// <summary>
    /// El prefab de la estrella.
    /// </summary>
    [SerializeField] GameObject star = null;
    /// <summary>
    /// La bola roja que aparece en la mano del enemigo antes de lanzar la estrella.
    /// </summary>
    [SerializeField] GameObject redBall = null;

    /// <summary>
    /// Componente Animator del enemigo.
    /// </summary>
    [Header("Components")]
    [SerializeField] Animator anim = null;
    /// <summary>
    /// Componente FinalBoss del enemigo.
    /// </summary>
    FinalBoss finalBoss;

    #endregion

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        cannonStartingRotation = cannon.transform.rotation;
        armStartingRotation = arm.transform.rotation;
        health = maxHealth;
        finalBoss = this;
        SelectAttack();
    }

    void Update()
    {
        if (moveCannon && player.activeSelf)
        {
            Vector3 dir = cannon.transform.position - player.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            cannon.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            
            if ((Time.time - timeLastShoot > cadency) && shoot0)
            {
                Shoot(shootPointCannon);
            }
        }

        else
        {
            cannon.transform.rotation = cannonStartingRotation;
        }

        if (moveArm && player.activeSelf)
        {
            Vector3 dir = arm.transform.position - player.transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BulletPlayer"))
        {
            Hurt();

            collision.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Función que hace que el enemigo elija un ataque aleatorio antes de lanzarlo.
    /// </summary>
    public void SelectAttack()
    {
        shoot0 = true;
        moveCannon = true;

        if (health <= (0.2f * maxHealth))
        {
            mode2 = true;
            timeBetweenAttacks = 4;
        }

        if (player.activeSelf)
        {
            float randomValue = Random.value;

            if (randomValue <= 0.1)
            {
                StartCoroutine(AttackEnergyBall());
            }

            else if (randomValue <= 0.2)
            {
                StartCoroutine(AttackStar());
            }

            else if (randomValue <= 0.4)
            {
                StartCoroutine(AttackMeteorite());
            }

            else if (randomValue <= 0.7)
            {
                StartCoroutine(AttackMissile());
            }

            else if (randomValue <= 1.0)
            {
                StartCoroutine(AttackLaser());
            }
        }
    }

    /// <summary>
    /// Función que se activa cuando el enemigo sufre algún daño.
    /// </summary>
    void Hurt()
    {
        health -= 1;

        fullBattery.fillAmount -= 1 / maxHealth;
        
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    /// <summary>
    /// Función que se activa para que el enemigo apunte a un objetivo con la mano.
    /// </summary>
    /// <param name="target">La posición a la que el enemigo va a apuntar.</param>
    void PointHand(Transform target)
    {
        Vector3 dir = arm.transform.position - target.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        arm.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    /// <summary>
    /// Función que se activa cuando el enemigo dispara por el cañón.
    /// </summary>
    /// <param name="shootPoint">Sitio donde se va a generar la bala.</param>
    void Shoot(Transform shootPoint)
    {
        bullet = ObjectPooler.SharedInstance.GetPooledObject("BulletEnemy");

        if (bullet != null)
        {
            bullet.transform.position = shootPoint.transform.position;
            bullet.transform.rotation = shootPoint.transform.rotation;
            bullet.SetActive(true);
        }

        timeLastShoot = Time.time;
    }

    /// <summary>
    /// Función que se activa cuando el enemigo muere y que limpia todos los objetos en la pantalla.
    /// </summary>
    void StopAttacks()
    {
        GameObject[] bullets = GameObject.FindGameObjectsWithTag("BulletEnemy");

        for (int i = 0; i < bullets.Length; i++)
        {
            bullets[i].SetActive(false);
        }

        GameObject activeStar = GameObject.FindGameObjectWithTag("Star");

        if (activeStar != null)
        {
            Destroy(activeStar);
        }

        GameObject[] activeMissiles = GameObject.FindGameObjectsWithTag("Missile");

        if (activeMissiles != null)
        {
            for (int i = 0; i < activeMissiles.Length; i++)
            {
                activeMissiles[i].SetActive(false);
            }
        }

        GameObject[] activeMeteorites = GameObject.FindGameObjectsWithTag("Meteorite");

        if (activeMeteorites != null)
        {
            for (int i = 0; i < activeMeteorites.Length; i++)
            {
                activeMeteorites[i].SetActive(false);
            }
        }

        GameObject[] activeLasers = GameObject.FindGameObjectsWithTag("Bullet4");

        if (activeLasers != null)
        {
            for (int i = 0; i < activeLasers.Length; i++)
            {
                activeLasers[i].SetActive(false);
            }
        }
    }

    /// <summary>
    /// Corrutina que inicia el ataque de los láseres.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackLaser()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        PointHand(player.transform);
        moveArm = true;
        int repetitions = 0;

        if (!mode2)
        {
            cadency1 = 1;
            maxRepetitions1 = 3;
        }

        else
        {
            cadency1 = 0.5f;
            maxRepetitions1 = 6;
        }

        while (repetitions < maxRepetitions1)
        {
            int currentShoots = 0;

            while (currentShoots < maxShoots1)
            {
                bullet = ObjectPooler.SharedInstance.GetPooledObject("Bullet4");

                if (health <= 0)
                {
                    yield break;
                }

                if (bullet != null)
                {
                    bullet.transform.position = shootPointArm.position;
                    bullet.transform.rotation = shootPointArm.rotation;
                    bullet.SetActive(true);
                }

                currentShoots += 1;

                yield return new WaitForSeconds(attackSpeed1);

            }

            repetitions += 1;

            yield return new WaitForSeconds(cadency1);
        }

        moveArm = false;

        arm.transform.rotation = armStartingRotation;

        SelectAttack();
    }

    /// <summary>
    /// Corrutina que inicia el ataque de los misiles.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackMissile()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        int shoots = 0;

        if (!mode2)
        {
            maxShoots2 = 10;
        }

        else
        {
            maxShoots2 = 20;
        }

        while (shoots < maxShoots2)
        {
            Vector2 spawnZone = new Vector2(spawnZoneRight2.position.x, spawnZoneRight2.position.y + Random.Range(-7f, 7f));

            GameObject missile = ObjectPooler.SharedInstance.GetPooledObject("Missile");

            if (health <= 0)
            {
                yield break;
            }

            if (missile != null)
            {
                missile.transform.position = spawnZone;
                missile.SetActive(true);
                shoots += 1;
                yield return new WaitForSeconds(cadency2);
            }
        }

        SelectAttack();
    }

    /// <summary>
    /// Corrutina que inicia el ataque de la bola de energía.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackEnergyBall()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        if (!mode2)
        {
            shoot0 = false;
            moveCannon = false;
        }

        moveArm = true;
        whiteBall.SetActive(true);
        yield return new WaitForSeconds(4);
        whiteBall.SetActive(false);

        if (health <= 0)
        {
            yield break;
        }

        Instantiate(energyBall, shootPointArm.position, shootPointArm.rotation);
        yield return new WaitForSeconds(7);
        moveArm = false;
        arm.transform.rotation = armStartingRotation;
        moveCannon = true;
        shoot0 = true;
        SelectAttack();
    }

    /// <summary>
    /// Corrutina que inicia el ataque de los meteoritos.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackMeteorite()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        int shoots = 0;

        if (!mode2)
        {
            maxShoots4 = 30;
        }

        else
        {
            maxShoots4 = 60;
        }

        shoot0 = false;
        moveCannon = false;

        PointHand(spawnZoneUp4);

        whiteBall.SetActive(true);

        yield return new WaitForSeconds(2);

        whiteBall.SetActive(false);

        Instantiate(energyBall, shootPointArm.position, shootPointArm.rotation);

        yield return new WaitForSeconds(4);

        arm.transform.rotation = armStartingRotation;

        while (shoots < maxShoots4)
        {
            Vector2 spawnZone = new Vector2(spawnZoneUp4.position.x + Random.Range(-23.0f, 13.0f), spawnZoneUp4.position.y);

            GameObject meteorite = ObjectPooler.SharedInstance.GetPooledObject("Meteorite");

            if (health <= 0)
            {
                yield break;
            }

            if (meteorite != null)
            {
                meteorite.transform.position = spawnZone;
                meteorite.transform.rotation = spawnZoneUp4.rotation;
                meteorite.SetActive(true);
                shoots += 1;
                yield return new WaitForSeconds(cadency4);
            }
        }

        yield return new WaitForSeconds(3);

        moveCannon = true;
        shoot0 = true;

        SelectAttack();
    }

    /// <summary>
    /// Corrutina que inicia el ataque de la estrella.
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackStar()
    {
        yield return new WaitForSeconds(timeBetweenAttacks);

        shoot0 = false;
        moveCannon = false;

        PointHand(centerRefence);

        redBall.SetActive(true);

        yield return new WaitForSeconds(2);

        redBall.SetActive(false);

        if (health <= 0)
        {
            yield break;
        }

        Instantiate(star, redBall.transform.position, shootPointArm.rotation);

        yield return new WaitForSeconds(6);

        arm.transform.rotation = armStartingRotation;
    }

    /// <summary>
    /// Corrutina que se inicia cuando el enemigo muere.
    /// </summary>
    /// <returns></returns>
    IEnumerator Die()
    {
        anim.SetTrigger("Dying");
        StopAttacks();
        arm.SetActive(false);
        cannon.SetActive(false);
        finalBoss.enabled = false;
        yield return new WaitForSeconds(2);
        Boss3Manager.boss3Manager.StartExplosion(false);
    }
}