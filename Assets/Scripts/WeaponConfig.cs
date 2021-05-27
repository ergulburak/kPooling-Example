using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjects/CreateWeapon", order = 1)]
public class WeaponConfig : ScriptableObject
{
    public enum WeaponType
    {
        Empty,
        SemiAuto,
        FullAuto,
        Shotgun,
        Melee,
    }
    public string weaponName;
    public string bulletPrefabName;
    public WeaponType weaponType = WeaponType.Empty;
    public float bulletSpeed = 100f;
    public float bulletDamage = 20f;
    public float lifeTime = 3f;
    public int magazine = 2;
    public int magazineBulletCount = 30;
    public int bulletCount = 55;
    public int bulletInMagazine = 30;
    public int projectileCount=1;
    public float spreadAngel=0;
    public float reloadTime = 2f;
    public float fireRate = 0.1f;
    
}
