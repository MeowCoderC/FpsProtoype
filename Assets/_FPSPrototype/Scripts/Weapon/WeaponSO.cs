using System.Collections.Generic;
using UnityEngine;

public enum ShootStyle
{
    Hitscan,     // Instant hit
    Projectile,  // Bullet travels over time
    Melee,       // Close range (e.g., sword, knife)
    Custom       // Special logic (e.g., beam, explosive charge)
}

public enum ShootingMethod
{
    Press,
    PressAndHold,
    HoldAndRelease,
    HoldUntilReady
}

public enum ReloadingStyle
{
    DefaultReload,
    Overheat
}

[CreateAssetMenu(fileName = "NewWeapon", menuName = "WeaponSO/New Weapon", order = 1)]
public class WeaponSO : ScriptableObject
{
    public string     name;
    public GameObject pickUpGraphics;

    [Header("Shooting Settings")]
    public ShootStyle     shootStyle;
    public ShootingMethod shootingMethod;

    [Tooltip("Time between shots (lower = faster)")]
    public float fireRate = 0.2f;

    public float bulletRange     = 100f;
    public int   bulletsPerFire  = 1;
    public int   ammoCostPerFire = 1;

    [Header("Bullet Spread")]
    public bool  applyBulletSpread = true;
    public float spreadAmount    = 0.03f;
    public float aimSpreadAmount = 0f;

    [Header("Recoil Settings")]
    public bool applyRecoil = true;

    public AnimationCurve recoilX = AnimationCurve.Linear(0, 0, 1, 1);
    public AnimationCurve recoilY = AnimationCurve.Linear(0, 0, 1, 1);

    public float xRecoilAmount                = 2f;
    public float yRecoilAmount                = 4f;
    public float recoilRelaxSpeed             = 1f;
    public bool  applyDifferentRecoilOnAiming = false;

    [Header("Penetration Settings")]
    public float penetrationAmount           = 10f;

    [Range(0f, 1f)]
    public float penetrationDamageReduction  = 0.579f;
}