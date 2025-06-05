using System.Collections;
using UnityEngine;
using DG.Tweening;

public class Gun : WeaponBase
{
    [Header("References")]
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform aimPoint;
    [SerializeField] protected Transform recoilTransform;

    [Header("Gun Settings")]
    public float bulletRange = 100f;
    public float spreadAmount = 0.03f;
    public float fireRate = 0.5f;
    public int damage = 20;

    [Header("VFX / Trail")]
    public GameObject muzzleFlash;
    public GameObject trail;
    public float trailSpeed = 200f;

    [Header("Physics")]
    public LayerMask hitLayer;
    public GameObject bulletHole;

    [Header("DOTween Recoil Settings")]
    public bool applyRecoil = true;
    public float recoilDuration = 0.1f;
    public float recoilStrengthX = 1.5f;
    public float recoilStrengthY = 1f;
    public float recoilBackZ = -0.02f;
    public float recoilReturnTime = 0.1f;

    [Header("Camera Shake")]
    [Min(0)] public float camShakeAmount;
    [Range(0f, 1f)] public float camShakeAimMultiplier = 1f;

    private bool canShoot = true;

    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;

    private void Awake()
    {
        if (recoilTransform != null)
        {
            initialLocalPos = recoilTransform.localPosition;
            initialLocalRot = recoilTransform.localRotation;
        }
    }

    public override void Attack()
    {
        if (!canShoot) return;

        canShoot = false;

        if (muzzleFlash != null)
        {
            Instantiate(muzzleFlash, firePoint.position, firePoint.rotation);
        }

        HandleHitscanProjectileShot();

        if (applyRecoil && recoilTransform != null)
        {
            PlayRecoil();
        }

        Invoke(nameof(CanShoot), fireRate);
    }

    private void CanShoot() => canShoot = true;

    private void HandleHitscanProjectileShot()
    {
        StartCoroutine(HandleShooting());
    }

    private IEnumerator HandleShooting()
    {
        Vector3 direction = aimPoint.forward;
        direction += new Vector3(
            Random.Range(-spreadAmount, spreadAmount),
            Random.Range(-spreadAmount, spreadAmount),
            0f
        );
        direction.Normalize();

        CamShake.instance?.ShootShake(camShakeAmount * camShakeAimMultiplier);

        RaycastHit hit;
        bool isHit = Physics.Raycast(firePoint.position, direction, out hit, bulletRange, hitLayer);

        Vector3 endPoint;
        if (isHit)
        {
            int layer = hit.collider.gameObject.layer;
            string layerName = LayerMask.LayerToName(layer);
            Debug.Log($"[Gun] Hit object: {hit.collider.name}, Layer: {layer} ({layerName})");

            endPoint = hit.point;

            if (bulletHole != null)
            {
                Quaternion rotation = Quaternion.LookRotation(hit.normal);
                GameObject hole = Instantiate(bulletHole, hit.point + hit.normal * 0.01f, rotation);
                hole.transform.SetParent(hit.collider.transform);
                Destroy(hole, 10f);
            }
        }
        else
        {
            endPoint = firePoint.position + direction * bulletRange;
        }

        yield return StartCoroutine(SpawnTrail(endPoint));
    }

    private IEnumerator SpawnTrail(Vector3 endPoint)
    {
        if (trail == null) yield break;

        GameObject trailObj = Instantiate(trail, firePoint.position, Quaternion.identity);

        LineRenderer lr = null;
        if (trailObj.TryGetComponent<LineRenderer>(out var lineRenderer))
        {
            lr = lineRenderer;
            lr.positionCount = 2;
            lr.SetPosition(0, firePoint.position);
            lr.SetPosition(1, firePoint.position);
        }

        if (trailObj.TryGetComponent<TrailRenderer>(out var tr))
        {
            trailObj.transform.rotation = Quaternion.LookRotation((endPoint - firePoint.position).normalized);
        }

        Vector3 startPos = firePoint.position;
        float distance = Vector3.Distance(startPos, endPoint);
        float travelTime = distance / trailSpeed;
        float elapsed = 0f;

        while (elapsed < travelTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / travelTime);
            Vector3 currentPos = Vector3.Lerp(startPos, endPoint, t);
            trailObj.transform.position = currentPos;

            if (lr != null)
            {
                lr.SetPosition(1, currentPos);
            }

            yield return null;
        }

        trailObj.transform.position = endPoint;
        if (lr != null)
        {
            lr.SetPosition(1, endPoint);
        }

        yield return new WaitForSeconds(0.6f);
        Destroy(trailObj);
    }

    private void PlayRecoil()
    {
        if (recoilTransform == null) return;

        recoilTransform.DOKill();

        recoilTransform.localPosition = initialLocalPos;
        recoilTransform.localRotation = initialLocalRot;

        float randomYaw = Random.Range(-recoilStrengthY, recoilStrengthY);
        float randomPitch = recoilStrengthX;

        Vector3 recoilPos = initialLocalPos + new Vector3(0f, 0f, recoilBackZ);
        Quaternion recoilRot = initialLocalRot * Quaternion.Euler(-randomPitch, randomYaw, 0f);

        recoilTransform.DOLocalMove(recoilPos, recoilDuration)
            .SetEase(Ease.OutQuad);

        recoilTransform.DOLocalRotateQuaternion(recoilRot, recoilDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                recoilTransform.DOLocalMove(initialLocalPos, recoilReturnTime).SetEase(Ease.InOutSine);
                recoilTransform.DOLocalRotateQuaternion(initialLocalRot, recoilReturnTime).SetEase(Ease.InOutSine);
            });
    }
}
