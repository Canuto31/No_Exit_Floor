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
        
        //FX
        if (muzzleFlash) muzzleFlash.Play();
        if (shootSound) shootSound.Play();

        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.DrawRay(ray.origin, ray.direction * hit.distance, Color.red, 1f);
            
            //Damage
            var damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage);
            }
            
            //Impact FX
        }
    }
}
