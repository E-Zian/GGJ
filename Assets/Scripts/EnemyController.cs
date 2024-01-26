using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyController : MonoBehaviour
{

    public GameObject _player;
    public float moveSpeed;

    public float health;
    public float healthModifier;
    [Tooltip("Please use 0.1 as step reference")]
    public float modifierStep;

    private string playerTag = "Player";
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

        health = 100f * healthModifier;         //Set health
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            //Drop RNG
            //Current drop rate is 1/10
            int dropChance = Random.Range(1, 11);
            if (dropChance == 1)
            {
                //Drop Weapon Box
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(playerTag))
        {
            _aIDestinationSetter.target = null;
            //Do player damage here
        }
    }

    public void ApplyDamage(float damage)
    {
        health -= damage;
    }
}
