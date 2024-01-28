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

    public GameObject canvas;

    public Image staminaBar;
    public float stamina, maxStamina;
    public float runCost = 20f;
    public Coroutine recharge;
    public float chargeRate;

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
    public GameObject gameOver;

    private SpriteRenderer spriteRenderer;

    public AudioSource gunClip;
    public AudioSource shotgunClip;

    //Ammo count stuff
    public float availableAmmo;
    public TextMeshProUGUI ammoCountText;

    //Bullet Properties
    public float bulletForce;
    public float fireForce;
    public float moveSpeed;
    public float shootDelay;
    public float shotgunDelay;
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
    bool isSprinting;
    //Crazy System
    public static float crazyDuration = 30f;
    public static bool isCrazy;

    public GameObject ammoCounter;
    public AudioSource doomMusic;
    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        availableAmmo = 0;
        isCrazy = false;
        canvas.GetComponent<pause>().enabled = true;
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
                spriteRenderer.sprite = rifleMode;
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
        if(isSprinting && (movement.x != 0 || movement.y != 0))
        {
            moveSpeed = 0.15f;
            stamina -= runCost * Time.deltaTime;
            if (stamina < 0)
            {
                stamina = 0;
                moveSpeed = 0.05f;
            }
            staminaBar.fillAmount = stamina / maxStamina;
            if(recharge != null)StopCoroutine(recharge);
            recharge = StartCoroutine(RechargeStamina());
        }
        else
        {
            moveSpeed = 0.05f;
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
        if (collision.gameObject.CompareTag(enemyTag))
        {
            //Die
            Debug.Log("Dead");
            Time.timeScale = 0f;
            gameOver.SetActive(true);
            canvas.GetComponent<pause>().enabled = false;
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
            availableAmmo = 300;
            ammoCounter.SetActive(true);
            Destroy(collision.gameObject);
        }
    }
   
    void Inputs()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            isSprinting = true;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            isSprinting = false;
        }
        //Enter Crazy Mode
        if (Input.GetKeyDown("space") && crazyMeter.clownMeterValue >= 100.0f)
        {
            isCrazy = true;
            doomMusic.Play();
            //crazyMeter.clownMeterValue = 0;
            StartCoroutine(crazyMode());
            StartCoroutine(crazyBarReset());
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
                    isShoot = Input.GetButton("Fire1");
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
                fireElapsedTime = 0.1f;
                GameObject bulletObject = Instantiate(APBullet, pistolWeapon.position, pistolWeapon.rotation);
                Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
                rb.AddForce(pistolWeapon.up * bulletForce, ForceMode2D.Impulse);
                gunClip.Play();
                bulletObject.GetComponent<Bullet>().startDecay(0.8f);
            }
            else
            {
                fireElapsedTime = 0;
                GameObject bulletObject = Instantiate(bullet, pistolWeapon.position, pistolWeapon.rotation);
                gunClip.Play();
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
            
            if (isCrazy)
            {
                fireElapsedTime = 0;
                foreach (var item in rifle)
                {
                    GameObject bulletObject1 = Instantiate(bullet, item.position, item.rotation);
                    gunClip.Play();
                    Rigidbody2D rb1 = bulletObject1.GetComponent<Rigidbody2D>();
                    rb1.AddForce(item.up * bulletForce, ForceMode2D.Impulse);
                }
            }
            else
            {
                fireElapsedTime = 0;
                GameObject bulletObject = Instantiate(bullet, rifleWeapon.position, rifleWeapon.rotation);
                gunClip.Play();
                Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
                rb.AddForce(rifleWeapon.up * bulletForce, ForceMode2D.Impulse);
                availableAmmo--;
            }
           
        }
    }
    void ShotgunShooting()
    {
        fireElapsedTime += Time.deltaTime;
        if (isShoot && fireElapsedTime >= shotgunDelay)
        {
            
            if (isCrazy)
            {
                fireElapsedTime = 0.2f;
                foreach (var item in shotgun)
                {
                    GameObject bulletObject = Instantiate(bullet, item.position, Quaternion.identity);
                    shotgunClip.Play();
                    Rigidbody2D rb = bulletObject.GetComponent<Rigidbody2D>();
                    rb.AddForce(item.up * bulletForce, ForceMode2D.Impulse);
                    bulletObject.GetComponent<Bullet>().startDecay(shotgunCrazyDecay);

                }
            }
            else
            {
                fireElapsedTime = 0f;

                foreach (var item in shotgun)
                {

                    GameObject bulletObject = Instantiate(bullet, item.position, Quaternion.identity);
                    shotgunClip.Play();
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
                fireObject.GetComponent<FireBullet>().startDecay(flamethrowerNormalDecay * 2);
            }
            else
            {
                GameObject fireObject = Instantiate(fire, flamethrowerWeapon.position, flamethrowerWeapon.rotation);
                fireObject.transform.localScale = new Vector3(0.5f, 0.5f, 1);
                Rigidbody2D rb = fireObject.GetComponent<Rigidbody2D>();
                rb.AddForce(flamethrowerWeapon.up * fireForce, ForceMode2D.Impulse);
                fireObject.GetComponent<FireBullet>().startDecay(flamethrowerNormalDecay);
                availableAmmo--;
            }
            
        }
    }

    IEnumerator crazyMode()
    {
        
        yield return new WaitForSeconds(crazyDuration);
        isCrazy = false;
    }
    IEnumerator crazyBarReset() {
        while(crazyMeter.clownMeterValue > 0)
        {

            crazyMeter.clownMeterValue -= 100/crazyDuration * Time.deltaTime;
            yield return null;
        }
        
    }
    IEnumerator RechargeStamina()
    {
        yield return new WaitForSeconds(1f);
        while(stamina < maxStamina)
        {
            stamina += chargeRate / 10f;
            if (stamina > maxStamina) stamina = maxStamina;
            staminaBar.fillAmount = stamina / maxStamina;
            yield return new WaitForSeconds(.1f);
        }
    }
    //void LookAtMouse()
    //{
    //    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //    transform.up = mousePosition - new Vector2(transform.position.x, transform.position.y);
    //}
}
