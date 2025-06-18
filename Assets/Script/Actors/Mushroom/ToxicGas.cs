using UnityEngine;

public class ToxicGas : MonoBehaviour
{
    [SerializeField] private float duration = 2f;
    [SerializeField] private int damage = 5;
    [SerializeField] private float tickInterval = 0.5f;

    private float timer = 0f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            timer += Time.deltaTime;
            if (timer >= tickInterval)
            {
                var stats = other.GetComponent<CharacterStats>();
                if (stats != null)
                    stats.TakeDamage(damage);
                timer = 0f;
            }
        }
    }
}