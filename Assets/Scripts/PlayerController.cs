using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static int weaponMode = 0; // 0 = pistol, 1 = rifle, 2 = shotgun, 3 = flamethrower

    public Rigidbody2D rb;

    public Transform pistolWeapon;
    public Transform rifleWeapon;
    public Transform rifle1;
    public Transform rifle2;
    public Transform shotgun1;
    public Transform shotgun2;
    public Transform shotgun3;
    public Transform flamethrowerWeapon;

    public GameObject bullet;
    public GameObject shotgunPickup;
    public GameObject riflePickup;
    public GameObject flamethrowerPickup;

    public float bulletForce;
    public float moveSpeed;
    public float fireDelay = 0.2f;

    Vector2 movement;

    string riflePickupString = "RiflePickup";
    string shotgunPickupString = "ShotgunPickup";
    string flamethrowerPickupString = "FlamethrowerPickup";

    float fireElapsedTime = 0;
    
    bool isShoot;
    bool isCrazy;

    private void Start()
    {
        
    }
    private void Update()
    {
        Inputs();
        LookAtMouse();
        if (weaponMode == 0)
        {
            PistolShooting();
        }
        else if (weaponMode == 1)
        {
            RifleShooting();
        }
        else if (weaponMode == 2)
        {
            ShotgunShooting();
        }
        else if(weaponMode == 3)
        {
            FlamethrowerShooting();
        }
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(riflePickupString))
        {
            weaponMode = 1;
        }
        if (collision.gameObject.CompareTag(shotgunPickupString))
        {
            weaponMode = 2;
        }
        if (collision.gameObject.CompareTag(flamethrowerPickupString))
        {
            weaponMode = 3;
        }
    }
    void Inputs()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
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
        if (isShoot)
        {
            GameObject bulletObject = Instantiate(bullet, pistolWeapon.position, pistolWeapon.rotation);
            Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
            rb.AddForce(pistolWeapon.up * bulletForce, ForceMode2D.Impulse);
        }
    }
    void RifleShooting()
    {
        fireElapsedTime += Time.deltaTime;

        if (isShoot && fireElapsedTime >= fireDelay)
        {
            if(isCrazy)
            {
                fireElapsedTime = 0;
                GameObject bulletObject1 = Instantiate(bullet, rifle1.position, rifle1.rotation);
                Rigidbody2D rb1 = bulletObject1.GetComponent<Rigidbody2D>();
                rb1.AddForce(rifle1.up * bulletForce, ForceMode2D.Impulse);

                GameObject bulletObject2 = Instantiate(bullet, rifle2.position, rifle2.rotation);
                Rigidbody2D rb2 = bulletObject2.GetComponent<Rigidbody2D>();
                rb2.AddForce(rifle2.up * bulletForce, ForceMode2D.Impulse);
            }
            else
            {
                fireElapsedTime = 0;
                GameObject bulletObject = Instantiate(bullet, rifleWeapon.position, rifleWeapon.rotation);
                Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
                rb.AddForce(rifleWeapon.up * bulletForce, ForceMode2D.Impulse);
            }
           
        }
    }
    void ShotgunShooting()
    {
        fireElapsedTime += Time.deltaTime;
        if (isShoot)
        {
            Shoot();
        }
        else if(isShoot && isCrazy && fireElapsedTime >= fireDelay)
        {
            fireElapsedTime = 0;
            Shoot();
        }
    }
    void FlamethrowerShooting()
    {
        if (isShoot)
        {
            Shoot();
        }
    }
    
    void LookAtMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = mousePosition - new Vector2(transform.position.x, transform.position.y);
    }
    void Shoot()
    {
        //GameObject bulletObject = Instantiate(bullet, weapon.position, weapon.rotation);
        //Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
        //rb.AddForce(weapon.up * bulletForce, ForceMode2D.Impulse);
    }
}
