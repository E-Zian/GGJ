using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{

    public GameObject _player;
    public GameObject enemy;
    public float moveSpeed;
    public float crazyMoveSpeed;

    public float health;
    public float maxHealth;
    [Tooltip("Please use 0.1 as step reference")]
    public float modifierStep;

    //Pickups Gameobject
    public GameObject riflePickup;
    public GameObject shotgunPickup;
    public GameObject flamethrowerPickup;
    public float bulletDamage;

    //DoT stuff
    public bool isOnFire;
    public float fireDoTDecay;

    private string playerTag = "Player";
    private string bulletTag = "Bullet";
    private string fireBulletTag = "FireBullet";
    private AIDestinationSetter _aIDestinationSetter;

    //GameObject to be used by object pooling
    private GameObject pickups;
    [Tooltip("This is also used to hold the corpse gameobject")]
    public GameObject corpse;

    //String
    private string fatEnemyName = "Enemy Fat";

    // Start is called before the first frame update
    void Start()
    {
        //Pathfinding AI Initial Setting
        _player = GameObject.FindGameObjectWithTag(playerTag);
        _aIDestinationSetter = GetComponent<AIDestinationSetter>();
        _aIDestinationSetter.target = _player.transform;

        health = maxHealth;    //Set health
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (!PlayerController.isCrazy)
            {
                //Drop RNG
                //Current drop rate is 1/10
                float dropChance = Random.Range(1.0f, 11.0f);
                if (dropChance <= 3f)
                {
                    int whatToDrop = Random.Range(1, 4);
                    switch (whatToDrop)
                    {
                        case 1:
                            pickups = Instantiate(riflePickup, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity);
                            GameManager.droppedWeaponPool.Add(pickups);
                            break;
                        case 2:
                            pickups = Instantiate(shotgunPickup, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity);
                            GameManager.droppedWeaponPool.Add(pickups);
                            break;
                        case 3:
                            pickups = Instantiate(flamethrowerPickup, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity);
                            GameManager.droppedWeaponPool.Add(pickups);
                            break;
                        default:
                            break;
                    }
                }
            }
            GameManager.currentSpawnedEnemy--;
            GameManager.remainingEnemyAmt--;
            if (GameManager.remainingEnemyAmt <= 0)
            {
                GameManager.finalEnemiesCount--;
            }
            if (!PlayerController.isCrazy)
            {
                crazyMeter.clownMeterValue += 2;
            }
            GameObject corpSe = Instantiate(corpse, this.transform.position, Quaternion.identity);
            GameManager.enemyCorpsePool.Add(corpSe);
            Destroy(gameObject);
        }

        if (PlayerController.isCrazy)
        {
            GetComponent<AIPath>().maxSpeed = crazyMoveSpeed;
        }
        else
        {
            GetComponent<AIPath>().maxSpeed = moveSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            _aIDestinationSetter.target = null;
            //Do player damage here
        }
        else if (collision.gameObject.CompareTag(bulletTag))
        {
            ApplyDamage(bulletDamage);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            StartCoroutine(restartTracking());
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(bulletTag))
        {
            ApplyDamage(bulletDamage);
        }
    }

    public void ApplyDamage(float damage)
    {
        if (PlayerController.isCrazy)
        {
            health -= 1000;
        }
        else
        {
            health -= damage;
        }
    }

    IEnumerator restartTracking()
    {
        yield return new WaitForSeconds(0.5f);
        _aIDestinationSetter.target = _player.transform;
    }

    public void startKenaFire()
    {
        if (!isOnFire)
        {
            isOnFire = true;
            StartCoroutine(kenaFire());
            StartCoroutine(fireDecay());
        }
    }

    IEnumerator kenaFire()
    {
        while (isOnFire)
        {
            ApplyDamage(100);
            yield return new WaitForSeconds(1f);
        }
    }

    IEnumerator fireDecay()
    {
        yield return new WaitForSeconds(fireDoTDecay);
        isOnFire = false;
    }
    private void OnDestroy()
    {
        //Definetely not how you do this, but, ain't nobody got time for that
        if (this.gameObject.name == fatEnemyName)
        {
            Instantiate(enemy, transform.position + transform.forward * 2, transform.rotation);
            Instantiate(enemy, transform.position + transform.right * 2, transform.rotation);
        }
    }
}
