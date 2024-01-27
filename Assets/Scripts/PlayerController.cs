using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public static int weaponMode = 0; // 0 = pistol, 1 = rifle, 2 = shotgun, 3 = flamethrower

    public Rigidbody2D rb;

    //Weapons stuff
    public Transform pistolWeapon;
    public Transform rifleWeapon;
    public Transform flamethrowerWeapon;

    public Sprite pistolMode;
    public Sprite rifleMode;
    public Sprite shotgunMode;
    public Sprite flamethrowerMode;

    public List<Transform> rifle;
    public List<Transform> shotgun;
   
    public GameObject bullet;
    public GameObject APBullet;
    public GameObject fire;

    private SpriteRenderer spriteRenderer;
    //Ammo count stuff
    public float availableAmmo;
    public TextMeshProUGUI ammoCountText;

    //Bullet Properties
    public float bulletForce;
    public float fireForce;
    public float moveSpeed;
    public float shootDelay;
    public float pistolDelay;
    [Tooltip("The bullet decay time for shotgun during normal mode")]
    public float shotgunNormalDecay;
    [Tooltip("The bullet decay time for shotgun during crazy mode")]
    public float shotgunCrazyDecay;
    [Tooltip("The bullet decay time for flamethrower during normal mode")]
    public float flamethrowerNormalDecay;
    [Tooltip("The bullet decay time for flamethrower during crazy mode")]
    public float flamethrowerCrazyDecay;

    Vector2 movement;
    Vector2 mousePos;

    string riflePickupTag = "RiflePickup";
    string shotgunPickupTag = "ShotgunPickup";
    string flamethrowerPickupTag = "FlamethrowerPickup";
    string spawnerTag = "SpawnPoint";
    string enemyTag = "Enemy";

    float fireElapsedTime = 0;
    
    bool isShoot;
    bool isCrazy = false;

    public GameObject ammoCounter;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        availableAmmo = 0;
    }
    private void Update()
    {
        if (!pause.isPaused) {
            Inputs();
           
            if (weaponMode == 0)
            {
                spriteRenderer.sprite = pistolMode;
                PistolShooting();
            }
            else if (weaponMode == 1)
            {
                spriteRenderer.sprite = pistolMode;
                RifleShooting();
            }
            else if (weaponMode == 2)
            {
                spriteRenderer.sprite = shotgunMode;
                ShotgunShooting();
            }
            else if (weaponMode == 3)
            {
                spriteRenderer.sprite = flamethrowerMode;
                FlamethrowerShooting();
            }
        }

        if (ammoCounter.activeInHierarchy)
        {
            ammoCountText.text = availableAmmo.ToString();
        }
        if (availableAmmo <= 0)
        {
            weaponMode = 0;
            ammoCounter.SetActive(false);
        }
        
        
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed);
        Vector2 lookDir = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x)* Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag)){
            //Die
            Debug.Log("Dead");
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(riflePickupTag))
        {
            weaponMode = 1;
            availableAmmo = 50;
            ammoCounter.SetActive(true);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag(shotgunPickupTag))
        {
            weaponMode = 2;
            availableAmmo = 35;
            ammoCounter.SetActive(true);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag(flamethrowerPickupTag))
        {
            weaponMode = 3;
            availableAmmo = 1000;
            Destroy(collision.gameObject);
        }
    }
    void Inputs()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown("space"))
        {
            isCrazy = !isCrazy;
        }
        //if not crazy
        if(!isCrazy)
        {
            switch(weaponMode){
                //pistol
                case 0:
                    isShoot = Input.GetButtonDown("Fire1");
                    break;
                //rifle
                case 1:
                    isShoot = Input.GetButton("Fire1");
                    break;
                //shotgun
                case 2:
                    isShoot = Input.GetButtonDown("Fire1");
                    break;
                //flamethrower
                case 3:
                    isShoot = Input.GetButton("Fire1");
                    break;
                default:
                    break;
            }
        }
        //if crazy
        else
        {
            switch (weaponMode)
            {
                //pistol
                case 0:
                    isShoot = Input.GetButtonDown("Fire1");
                    break;
                //rifle
                case 1:
                    isShoot = Input.GetButton("Fire1");
                    break;
                //shotgun
                case 2:
                    isShoot = Input.GetButton("Fire1");
                    break;
                //flamethrower
                case 3:
                    isShoot = Input.GetButton("Fire1");
                    break;
                default:
                    break;
            }
        }
    }
    void PistolShooting()
    {
        fireElapsedTime += Time.deltaTime;

        if (isShoot && fireElapsedTime >= pistolDelay)
        {
            if (isCrazy)
            {
                fireElapsedTime = 0;
                GameObject bulletObject = Instantiate(APBullet, pistolWeapon.position, pistolWeapon.rotation);
                Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
                rb.AddForce(pistolWeapon.up * bulletForce, ForceMode2D.Impulse);
                bulletObject.GetComponent<Bullet>().startDecay(0.4f);
            }
            else
            {
                fireElapsedTime = 0;
                GameObject bulletObject = Instantiate(bullet, pistolWeapon.position, pistolWeapon.rotation);
                Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
                rb.AddForce(pistolWeapon.up * bulletForce, ForceMode2D.Impulse);
            }
        }
    }
    void RifleShooting()
    {
        fireElapsedTime += Time.deltaTime;

        if (isShoot && fireElapsedTime >= shootDelay)
        {
            if(isCrazy)
            {
                fireElapsedTime = 0;
                foreach (var item in rifle)
                {
                    GameObject bulletObject1 = Instantiate(bullet, item.position, item.rotation);
                    Rigidbody2D rb1 = bulletObject1.GetComponent<Rigidbody2D>();
                    rb1.AddForce(item.up * bulletForce, ForceMode2D.Impulse);
                    availableAmmo--;
                }
            }
            else
            {
                fireElapsedTime = 0;
                GameObject bulletObject = Instantiate(bullet, rifleWeapon.position, rifleWeapon.rotation);
                Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
                rb.AddForce(rifleWeapon.up * bulletForce, ForceMode2D.Impulse);
                availableAmmo--;
            }
           
        }
    }
    void ShotgunShooting()
    {
        fireElapsedTime += Time.deltaTime;
        if (isShoot && fireElapsedTime >= shootDelay)
        {
            if (isCrazy)
            {
                fireElapsedTime = 0;
                foreach (var item in shotgun)
                {
                    GameObject bulletObject = Instantiate(bullet, item.position, Quaternion.identity);
                    Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
                    rb.AddForce(item.up * bulletForce, ForceMode2D.Impulse);
                    bulletObject.GetComponent<Bullet>().startDecay(shotgunCrazyDecay);

                }
                availableAmmo--;
            }
            else
            {
                fireElapsedTime = 0;

                foreach (var item in shotgun)
                {
                    GameObject bulletObject = Instantiate(bullet, item.position, Quaternion.identity);
                    Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
                    rb.AddForce(item.up * bulletForce, ForceMode2D.Impulse);
                    bulletObject.GetComponent<Bullet>().startDecay(shotgunNormalDecay);

                }
                availableAmmo--;
            }

        }
    }
    void FlamethrowerShooting()
    {
        if (isShoot)
        {
            if (isCrazy)
            {
                GameObject fireObject = Instantiate(fire, flamethrowerWeapon.position, flamethrowerWeapon.rotation);
                fireObject.transform.localScale = new Vector3(1, 1, 1);
                Rigidbody2D rb = fireObject.GetComponent<Rigidbody2D>();
                rb.AddForce(flamethrowerWeapon.up * fireForce, ForceMode2D.Impulse);
                fireObject.GetComponent<Bullet>().startDecay(flamethrowerNormalDecay);
            }
            else
            {
                GameObject fireObject = Instantiate(fire, flamethrowerWeapon.position, flamethrowerWeapon.rotation);
                fireObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                Rigidbody2D rb = fireObject.GetComponent<Rigidbody2D>();
                rb.AddForce(flamethrowerWeapon.up * fireForce, ForceMode2D.Impulse);
                fireObject.GetComponent<Bullet>().startDecay(flamethrowerNormalDecay);
            }
            availableAmmo--;
        }
    }

    //void LookAtMouse()
    //{
    //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    transform.up = mousePosition - new Vector2(transform.position.x, transform.position.y);
    //}
}
