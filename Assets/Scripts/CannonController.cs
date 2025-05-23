using UnityEngine;

public class CannonController : MonoBehaviour
{
    private Rigidbody rb;
    public float rotationSpeed = 60f;
    public float elevationSpeed = 30f;
    public GameObject projectile;
    public Transform spawnPoint;
    public float firePower = 2000f;
    
    private float currentElevation = 0f;
    private float maxElevation = 30f;
    private float minElevation = -5f;
    private float can_fire = 0;

    private Vector3 initialCameraOffset;
    private Quaternion initialCameraRotation;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (spawnPoint == null)
        {
            spawnPoint = transform.Find("SpawnPoint");
            if (spawnPoint == null)
            {
                Debug.LogError("Spawn point not found! Create a child object named 'SpawnPoint'");
            }
        }

        // Запоминаем начальное положение и поворот камеры относительно пушки
        if (Camera.main != null)
        {
            initialCameraOffset = Camera.main.transform.position - transform.position;
            initialCameraRotation = Camera.main.transform.rotation;
        }
    }

    void Update()
    {
        float fire = Input.GetAxis("Fire1");
        if (fire == 1 && fire != can_fire && spawnPoint != null)
        {
            Fire();
        }
        can_fire = fire;
    }

    void FixedUpdate()
    {
        float turn = Input.GetAxis("Horizontal") * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        float elevate = Input.GetAxis("Vertical");
        currentElevation += elevate * elevationSpeed * Time.fixedDeltaTime;
        currentElevation = Mathf.Clamp(currentElevation, minElevation, maxElevation);
        
        if (spawnPoint != null)
        {
            spawnPoint.localRotation = Quaternion.Euler(currentElevation, 0f, 0f);
        }
    }

    void Fire()
    {
        if (projectile == null)
        {
            Debug.LogError("Projectile prefab not assigned!");
            return;
        }

        GameObject projectile_clone = Instantiate(projectile, spawnPoint.position, spawnPoint.rotation);
        Rigidbody projectileRb = projectile_clone.GetComponent<Rigidbody>();
        
        if (projectileRb == null)
        {
            projectileRb = projectile_clone.AddComponent<Rigidbody>();
        }
        
        projectileRb.AddForce(spawnPoint.forward * firePower);

        if (Camera.main != null)
        {
            // Добавляем скрипт следования за снарядом с сохранением начального смещения
            CameraFollow followScript = Camera.main.gameObject.AddComponent<CameraFollow>();
            followScript.target = projectile_clone.transform;
            followScript.initialOffset = initialCameraOffset;
            followScript.initialRotation = initialCameraRotation;
        }
    }
}