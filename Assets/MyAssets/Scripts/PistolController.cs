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
    public GameObject wallImpactPrefab;

    private float _nextFireTime;
    
    [Header("Muzzle Flash Light")]
    public GameObject muzzleFlashLight;
    
    private void Awake()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
        
        if (muzzleFlashLight != null)
        {
            muzzleFlashLight.SetActive(false);
            //muzzleFlashLight.enabled = false;
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
            
            if (muzzleFlashLight != null)
                muzzleFlashLight.SetActive(true);
                //muzzleFlashLight.enabled = true;
            
            Invoke(nameof(StopMuzzleFlash), 0.05f);
        }
        
        //FX
        //if (muzzleFlash) muzzleFlash.Play();
        if (shootSound) shootSound.Play();
        
        if (CameraShake.instance != null) CameraShake.instance.Shake(0.04f, 0.08f);

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);
            
            Vector3 hitPoint = hit.point;
            Vector3 hitNormal = hit.normal;

            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                var damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.TakeDamage(damage, hitPoint, hitNormal);
                }
            }
            else
            {
                //Environment
                Instantiate(wallImpactPrefab, hitPoint, Quaternion.LookRotation(hit.normal));
            }
        }
    }

    private void StopMuzzleFlash()
    {
        if (muzzleFlash)
            muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmitting);
        
        if (muzzleFlashLight != null)
            muzzleFlashLight.SetActive(false);
            //muzzleFlashLight.enabled = false;
    }
}
