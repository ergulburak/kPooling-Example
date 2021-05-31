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

    public static WeaponSystem Instance { get; private set; }

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
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Shoot()
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
                    switch (weaponType)
                    {
                        case WeaponConfig.WeaponType.Empty:
                            break;
                        case WeaponConfig.WeaponType.SemiAuto:
                            SemiAutoWeaponAction();
                            break;
                        case WeaponConfig.WeaponType.FullAuto:
                            FullAutoWeaponAction();
                            break;
                        case WeaponConfig.WeaponType.Shotgun:
                            ShotgunAutoWeaponAction();
                            break;
                        default:
                            Debug.Log("Hatalı silah tipi algılandı.");
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
        if (PoolingSystem.TryGetInstance(projectilePoolKey, out instanceProjectile))
        {
            Physics.IgnoreCollision(instanceProjectile.GetComponent<Collider>(),
                bulletSpawnPoint.parent.GetComponent<Collider>());

            instanceProjectile.transform.position = bulletSpawnPoint.position;
            instanceProjectile.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            instanceProjectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
            instanceProjectile.GetComponent<Rigidbody>()
                .AddForce(bulletSpawnPoint.forward * config.bulletSpeed, ForceMode.Impulse);
            instanceProjectile.GetComponentInChildren<TrailRenderer>().Clear();
        }
    }

    private void FullAutoWeaponAction()
    {
        if (PoolingSystem.TryGetInstance(projectilePoolKey, out instanceProjectile))
        {
            Physics.IgnoreCollision(instanceProjectile.GetComponent<Collider>(),
                bulletSpawnPoint.parent.GetComponent<Collider>());

            instanceProjectile.transform.position = bulletSpawnPoint.position;
            instanceProjectile.transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
            instanceProjectile.GetComponent<Rigidbody>().velocity = Vector3.zero;
            instanceProjectile.GetComponent<Rigidbody>()
                .AddForce(bulletSpawnPoint.forward * config.bulletSpeed, ForceMode.Impulse);
            instanceProjectile.GetComponentInChildren<TrailRenderer>().Clear();
        }
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