using UnityEngine;

public class PistolController : MonoBehaviour
{
    [Header("Shoot Settings")] 
    public float damage = 25f;
    public float fireRate = 0.3f;
    public float range = 100f;

    [Header("References")]
    public Camera playerCamera;
    public Transform shootPoint;
    public ParticleSystem muzzleFlash;
    public AudioSource shootSound;

    [Header("Impact FX")]
    public GameObject bloodImpactPrefab;
    public GameObject wallImpactPrefab;

    private float _nextFireTime;
    
    private void Awake()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void Shoot()
    {
        if (Time.time < _nextFireTime) return;
        _nextFireTime = Time.time + fireRate;

        if (muzzleFlash)
        {
            muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            muzzleFlash.Play();
            Invoke(nameof(StopMuzzleFlash), 0.05f);
        }
        
        //FX
        //if (muzzleFlash) muzzleFlash.Play();
        if (shootSound) shootSound.Play();

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);
            
            Vector3 hitPoint = hit.point;
            Quaternion hitRotation = Quaternion.LookRotation(hit.normal);

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                //Blood
                hitPoint += hit.normal * 0.01f;
                Instantiate(bloodImpactPrefab, hitPoint, hitRotation);
                
                var damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage);
                }
            }
            else
            {
                //Environment
                Instantiate(wallImpactPrefab, hitPoint, hitRotation);
            }
        }
    }

    private void StopMuzzleFlash()
    {
        if (muzzleFlash)
            muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
