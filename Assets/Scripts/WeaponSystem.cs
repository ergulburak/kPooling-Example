using kTools.Pooling;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{
    public enum WeaponState
    {
        ReadyToFire,
        Cooldown,
        Reload,
        Empty,
        Waiting
    }

    public static WeaponSystem instance;
    [Header("Monitor")] public WeaponState weaponState = WeaponState.Empty;
    public WeaponConfig.WeaponType weaponType = WeaponConfig.WeaponType.Empty;
    public Transform bulletSpawnPoint;
    public WeaponConfig config;
    
    [Header("Settings")] public bool enable;
    public int projectilePoolSize;
    public GameObject projectilePoolKey;
    GameObject instanceProjectile;
    
    void Start()
    {
        projectilePoolKey = Resources.Load("Bullets/" + config.bulletPrefabName) as GameObject;
        
        if (enable) PoolingSystem.CreatePool(projectilePoolKey, projectilePoolKey, projectilePoolSize);
        else
            Debug.Log("Pooling kapalı.");
        if (config != null)
        {
            weaponState = WeaponState.ReadyToFire;
            weaponType = config.weaponType;
        }
    }
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void Shoot()
    {
        switch (weaponType)
        {
            case WeaponConfig.WeaponType.Empty:
                break;
            case WeaponConfig.WeaponType.SemiAuto:
                ShootingProcess(1);
                break;
            case WeaponConfig.WeaponType.FullAuto:
                ShootingProcess(2);
                break;
            case WeaponConfig.WeaponType.Shotgun:
                ShootingProcess(3);
                break;
            default:
                Debug.Log("Hatalı silah tipi algılandı.");
                break;
        }
    }

    void ShootingProcess(int weaponTypeNum)
    {
        switch (weaponState)
        {
            case WeaponState.ReadyToFire:
                if (config.bulletInMagazine <= 0)
                {
                    if (config.bulletCount > 0)
                    {
                        weaponState = WeaponState.Reload;
                    }
                    else
                        weaponState = WeaponState.Empty;
                }
                else
                {
                    switch (weaponTypeNum)
                    {
                        case 0:
                            break;
                        case 1:
                            SemiAutoWeaponAction();
                            break;
                        case 2:
                            FullAutoWeaponAction();
                            break;
                        case 3:
                            ShotgunAutoWeaponAction();
                            break;
                    }

                    config.bulletInMagazine--;
                    weaponState = WeaponState.Cooldown;
                }

                break;
            case WeaponState.Cooldown:
                weaponState = WeaponState.Waiting;
                InvokeRepeating(nameof(WaitingCooldown), config.fireRate, 0f);
                break;
            case WeaponState.Reload:
                weaponState = WeaponState.Waiting;
                InvokeRepeating(nameof(WaitingReload), config.reloadTime, 0f);
                break;
            case WeaponState.Empty:
                Debug.Log("Mermi Bitti.");
                if (config.bulletCount > 0)
                    weaponState = WeaponState.Reload;
                break;
        }
    }

    private void SemiAutoWeaponAction()
    {
        GameObject bullet = Instantiate(Resources.Load("Bullets/" + config.bulletPrefabName) as GameObject);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(),
            bulletSpawnPoint.parent.GetComponent<Collider>());

        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        bullet.GetComponent<Rigidbody>()
            .AddForce(bulletSpawnPoint.forward * config.bulletSpeed, ForceMode.Impulse);
    }

    private void FullAutoWeaponAction()
    {
        GameObject bullet = Instantiate(Resources.Load("Bullets/" + config.bulletPrefabName) as GameObject);
        Physics.IgnoreCollision(bullet.GetComponent<Collider>(),
            bulletSpawnPoint.parent.GetComponent<Collider>());

        bullet.transform.position = bulletSpawnPoint.position;
        bullet.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        bullet.GetComponent<Rigidbody>()
            .AddForce(bulletSpawnPoint.forward * config.bulletSpeed, ForceMode.Impulse);
    }

    private void ShotgunAutoWeaponAction()
    {
        if (enable)
        {
            
            for (int i = 0; i < config.projectileCount; i++)
            {
                if (PoolingSystem.TryGetInstance(projectilePoolKey, out instanceProjectile))
                {
                    instanceProjectile.transform.position = bulletSpawnPoint.transform.position;
                    Physics.IgnoreCollision(instanceProjectile.GetComponent<Collider>(),
                        bulletSpawnPoint.parent.GetComponent<Collider>());

                    instanceProjectile.transform.rotation = Quaternion.Euler(
                        UnityEngine.Random.Range(-config.spreadAngel, config.spreadAngel),
                        transform.eulerAngles.y + UnityEngine.Random.Range(-config.spreadAngel, config.spreadAngel),
                        0);

                    instanceProjectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
                    instanceProjectile.GetComponent<Rigidbody>()
                        .AddForce(instanceProjectile.transform.forward * config.bulletSpeed, ForceMode.Impulse);

                    instanceProjectile.GetComponentInChildren<TrailRenderer>().Clear();
                }
            }
        }
        else
        {
            for (int i = 0; i < config.projectileCount; i++)
            {
                GameObject projectile = Instantiate(Resources.Load("Bullets/" + config.bulletPrefabName) as GameObject);
                projectile.transform.position = bulletSpawnPoint.transform.position;
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(),
                    bulletSpawnPoint.parent.GetComponent<Collider>());

                projectile.transform.rotation = Quaternion.Euler(
                    UnityEngine.Random.Range(-config.spreadAngel, config.spreadAngel),
                    transform.eulerAngles.y + UnityEngine.Random.Range(-config.spreadAngel, config.spreadAngel),
                    0);

                projectile.GetComponent<Rigidbody>()
                    .AddForce(projectile.transform.forward * config.bulletSpeed, ForceMode.Impulse);
            }
        }
    }

    private void WaitingCooldown()
    {
        weaponState = WeaponState.ReadyToFire;
        CancelInvoke(nameof(WaitingCooldown));
    }


    private void WaitingReload()
    {
        if (config.bulletCount < config.magazineBulletCount)
        {
            config.bulletInMagazine = config.bulletCount;
            config.bulletCount = 0;
        }
        else
        {
            config.bulletInMagazine = config.magazineBulletCount;
            config.bulletCount -= config.magazineBulletCount;
        }

        weaponState = WeaponState.ReadyToFire;
        CancelInvoke(nameof(WaitingReload));
    }
}