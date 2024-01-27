using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{

    public GameObject _player;
    public GameObject enemy;
    public float moveSpeed;

    public float health;
    public float healthModifier;
    public float maxHealth;
    [Tooltip("Please use 0.1 as step reference")]
    public float modifierStep;

    //Pickups Gameobject
    public GameObject riflePickup;
    public GameObject shotgunPickup;
    public GameObject flamethrowerPickup;

    private string playerTag = "Player";
    private string bulletTag = "Bullet";
    private AIDestinationSetter _aIDestinationSetter;

    // Start is called before the first frame update
    void Start()
    {
        //Pathfinding AI Initial Setting
        _player = GameObject.FindGameObjectWithTag(playerTag);
        GetComponent <AIPath>().maxSpeed = moveSpeed;
        _aIDestinationSetter = GetComponent<AIDestinationSetter>();
        _aIDestinationSetter.target = _player.transform;

        //Health System
        //Health Modifier definition
        if (GameManager.remainingEnemyAmt <= GameManager.startingEnemyAmt)
        {
            healthModifier = 1.0f;
        }
        else if (GameManager.remainingEnemyAmt < GameManager.startingEnemyAmt - 1000)
        {
            healthModifier += modifierStep;
        }
        else if (GameManager.remainingEnemyAmt < GameManager.startingEnemyAmt - 2000)
        {
            healthModifier += modifierStep * 2;
        }
        else if (GameManager.remainingEnemyAmt < GameManager.startingEnemyAmt - 3000)
        {
            healthModifier += modifierStep * 3;
        }
        else if (GameManager.remainingEnemyAmt < GameManager.startingEnemyAmt - 4000)
        {
            healthModifier += modifierStep * 4;
        }

        health = maxHealth * healthModifier;         //Set health
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //Drop RNG
            //Current drop rate is 1/10
            int dropChance = Random.Range(1, 11);
            if (dropChance >= 2)
            {
                int whatToDrop = Random.Range(1, 4);
                switch (whatToDrop)
                {
                    case 1:
                        Instantiate(riflePickup, this.transform.position, Quaternion.identity);
                        break;
                    case 2:
                        Instantiate(shotgunPickup, this.transform.position, Quaternion.identity);
                        break;
                    case 3:
                        Instantiate(flamethrowerPickup, this.transform.position, Quaternion.identity);
                        break;
                    default:
                        break;
                }
            }
            Destroy(gameObject);
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
            ApplyDamage(10);
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
            ApplyDamage(10);
        }
    }

    public void ApplyDamage(float damage)
    {
       health -= damage;
    }

    IEnumerator restartTracking()
    {
        yield return new WaitForSeconds(0.5f);
        _aIDestinationSetter.target = _player.transform;
    }

    private void OnDestroy()
    {
        Instantiate(enemy, transform.position + transform.forward * 2, transform.rotation);
        Instantiate(enemy, transform.position + transform.right * 2, transform.rotation);
    }
}
