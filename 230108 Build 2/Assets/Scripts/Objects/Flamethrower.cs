using System.Collections;
using UnityEngine;

public class FlameThrower : MonoBehaviour
{
    PlayerController player;
    [SerializeField] GasManager gasManager;
    // The particle system for the flames
    public ParticleSystem flameParticles;

    // The damage per second for the flames
    public float flameDamage = 10f;

    // The range of the flames
    public float flameRange = 5f;

    // The timer of the flames
    public float lifetime;
    private float flameTimer;

    // The layer mask for the enemies
    public LayerMask enemyLayer;

    // The audio source for the flamethrower sound
    //public AudioSource flameSound;

    // The flag to indicate if the flamethrower is on
    private bool isFlaming = false;

    // The cost of using this flamethrower
    public float gasCost;
    private float gasInterval = 1f;

    // The timer to control the flame damage frequency
    private float damageTimer = 0f;

    // The interval between each flame damage
    public float damageInterval = 0.125f;

    private void Awake()
    {
        player = GetComponentInParent<PlayerController>();

    }

    private void Start()
    {
        flameTimer = lifetime;
    }

    public void HandleUpdate()
    {
        // Check if the fire button is pressed
        if (Input.GetKey(KeyCode.C))
        {
            // Turn on the flamethrower
            CancelInvoke();
            TurnOnFlame();
            InvokeRepeating("ReduceGas", .005f, gasInterval);
            //player.Player.ReduceGas(gasCost, gasInterval);
            //gasManager.UpdateData(player.Player);
        }
        else
        {
            // Turn off the flamethrower
            TurnOffFlame();
        }

        // If the flamethrower is on, update the timer and damage the enemies
        if (isFlaming)
        {
            damageTimer += Time.deltaTime;
            if (damageTimer >= damageInterval)
            {
                DamageEnemies();
                damageTimer = 0f;
            }
        }
        /*else
        {
            flameTimer -= Time.deltaTime;
            if (flameTimer <= 0)
            {
                Destroy(this.gameObject);
            }
        }*/
    }

    void ReduceGas()
    {
        player.Player.ReduceGas(gasCost);
    }

    // Turn on the flamethrower
    public void TurnOnFlame()
    {

        //CancelInvoke();
        // Enable the particle system
        flameParticles.Play();

        //InvokeRepeating("ReduceGas", .005f, gasInterval);
        player.Player.ReduceGas(gasCost);
        //gasManager.UpdateData(player.Player);

        // Play the sound
        //flameSound.Play();

        // Set the flag to true
        isFlaming = true;
    }

    // Turn off the flamethrower
    public void TurnOffFlame()
    {
        // Disable the particle system
        flameParticles.Stop();
        
        //CancelInvoke();
        // Stop the sound
        //flameSound.Stop();

        // Set the flag to false
        isFlaming = false;
    }

    public void Setup(Vector3 direction)
    {
        transform.rotation = Quaternion.Euler(direction);
    }

    // Damage the enemies in range of the flames
    public void DamageEnemies()
    {
        // Get the direction and angle of the flames
        Vector3 direction = transform.right;
        transform.rotation = Quaternion.Euler(direction);
        float angle = flameParticles.shape.arc / 2f;

        // Get all the colliders in range of the flames
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, flameRange, enemyLayer);

        // Loop through each collider and check if it is hit by the flames
        foreach (Collider2D collider in colliders)
        {
            // Get the vector from the flamethrower to the collider
            Vector2 vector = collider.transform.position - transform.position;

            // Get the angle between the direction and the vector
            float vectorAngle = Vector2.Angle(direction, vector);

            // If the angle is less than or equal to the half of the arc angle, it means it is hit by the flames
            if (vectorAngle <= angle)
            {
                // Get the enemy component and apply damage
                GenericHealth enemy = collider.GetComponent<GenericHealth>();
                if (enemy != null)
                {
                    enemy.Damage(flameDamage * damageInterval);
                    // Aqui se puede agregar efecto (congelar)
                }
            }
        }
    }
}

