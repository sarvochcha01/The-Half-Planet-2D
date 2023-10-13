using System.Collections;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    private enum Type
    {
        auto, 
        single,
    }

    [SerializeField] private Type type;
    [SerializeField] private Transform firePoint;
    [SerializeField] private Transform crosshairTransform;

    [SerializeField] private Pickup pickup;
    [SerializeField] private CrosshairManager crosshairManager;
    [SerializeField] private CameraShake cameraShake;
    [SerializeField] private UINotificationManager notificationManager;
    [SerializeField] private UIAmmoCountManager ammoCountManager;

    [SerializeField] private bool isShooting;
    [SerializeField] private bool isReloading = false;

    [SerializeField] private float reloadTime;

    [SerializeField] private float fireRate;
    [SerializeField] private int damage;
    [SerializeField] private float nextTimeToFire = 0f;

    [SerializeField] private float range;
    [SerializeField] private GameObject trail;

    [SerializeField] private int currentMagazineAmmoCount;
    [SerializeField] private int totalAmmoCount;
    [SerializeField] private int magazineCap;

    [SerializeField] private LayerMask shootingLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        firePoint = transform.GetChild(0);
        pickup = GetComponent<Pickup>();
        crosshairManager = FindObjectOfType<CrosshairManager>();
        crosshairTransform = crosshairManager.GetCrosshairTransform();
        cameraShake = FindObjectOfType<CameraShake>();
        notificationManager = FindObjectOfType<UINotificationManager>();
        ammoCountManager = FindObjectOfType<UIAmmoCountManager>();

        currentMagazineAmmoCount = magazineCap;
        //totalAmmoCount -= magazineCap;

    }

    void Update()
    {
        if (isReloading) return;
        bool readyToShoot = pickup.IsEquipped() && !crosshairManager.GetOnPickableObjectBool() && currentMagazineAmmoCount > 0 && !FindObjectOfType<PauseMenu>().isGamePaused;

        if (currentMagazineAmmoCount <= 0 && Input.GetButton("Fire1") && pickup.IsEquipped())
        {
            notificationManager.SetText("Reload Required");
        }

        if (!pickup.IsEquipped())
        {
            //FindObjectOfType<AudioManager>().ResetSound();
        }

        if (Input.GetKeyDown(KeyCode.R) && pickup.IsEquipped() && currentMagazineAmmoCount < magazineCap && totalAmmoCount > 0)
        {
            isReloading = true;
            if (type == Type.auto) FindObjectOfType<GunAudioManager>().Play("AutoReload");
            else if (type == Type.single) FindObjectOfType<GunAudioManager>().Play("PistolReload");
            StartCoroutine(Reload());
        }

        if (totalAmmoCount <= 0 && pickup.IsEquipped() && Input.GetKeyDown(KeyCode.R))
        {
            totalAmmoCount = 0;
            notificationManager.SetText("Out of ammo");
        }

        if (type == Type.auto)
        {
            if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire && readyToShoot)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
                Recoil();
                FindObjectOfType<GunAudioManager>().Play("AutoShoot");
                Shoot();
                isShooting = true;

            }
            else isShooting = false;
        }

        else if (type == Type.single)
        {
            if (Input.GetButtonDown("Fire1") && readyToShoot && !isShooting)
            {
                isShooting = true;
                Recoil();
                FindObjectOfType<GunAudioManager>().Play("PistolShoot");
                Shoot();
            }
            else isShooting = false;
        }
    }

    IEnumerator Reload()
    {
        notificationManager.SetText("Reloading");
        yield return new WaitForSeconds(reloadTime);
        if (currentMagazineAmmoCount > 0)
        {
            notificationManager.SetText(currentMagazineAmmoCount + " ammo wasted");
            FindObjectOfType<GameManager>().totalAmmoWasted += currentMagazineAmmoCount;
        }

        if (totalAmmoCount <= magazineCap)
        {
            currentMagazineAmmoCount = totalAmmoCount;
            totalAmmoCount = 0;
        }
        else
        {
            currentMagazineAmmoCount = magazineCap;
            totalAmmoCount -= magazineCap;
        }

        isReloading = false;
        notificationManager.SetText("Reloaded");
    }

    private void Shoot()
    {
        //Debug.Log("Shoot");
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, range, shootingLayerMask);
        currentMagazineAmmoCount -= 1;
        FindObjectOfType<GameManager>().totalAmmoUsed += 1;
        var trailInst = Instantiate(trail, firePoint.position, firePoint.rotation);
        
        if (hit)
        {
            Enemy enemy = hit.transform.GetComponent<Enemy>();

            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                FindObjectOfType<GameManager>().totalAmmoHitTarget += 1;
            }
        }
    }

    private void Recoil()
    {
        cameraShake.ShakeCamera(0.25f, .1f);
        Vector2 curPos = crosshairManager.GetCrosshairSprite().transform.position;

        float spriteHeight = crosshairManager.GetCrosshairSprite().sprite.bounds.extents.y/2 * crosshairManager.GetCrosshairTransform().localScale.x;
        float spriteWidth = crosshairManager.GetCrosshairSprite().sprite.bounds.extents.x/2 * crosshairManager.GetCrosshairTransform().localScale.y;

        float xpos = curPos.x + Random.Range(-spriteWidth, spriteWidth);
        float ypos = curPos.y + Random.Range(-spriteHeight, spriteHeight);

        Vector2 randomRecoilPos = new Vector2(xpos, ypos);
        Vector3 dir = (randomRecoilPos - (Vector2)firePoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        firePoint.eulerAngles = Vector3.Slerp(firePoint.eulerAngles, new Vector3(0, 0, angle), 5f);
    }



    public bool IsShooting()
    {
        return isShooting;
    }

    public bool IsReloading()
    { 
        return isReloading; 
    }

    public float GetReloadDuration()
    {
        return reloadTime;
    }

    public int GetMagCap()
    {
        return magazineCap;
    }

    public int CurrentMagazineAmmoCount()
    {
        return currentMagazineAmmoCount;
    }

    public int GetTotalAmmoCount()
    {
        return totalAmmoCount;
    }

    public string GetGunType()
    {
        return type.ToString();
    }

}
