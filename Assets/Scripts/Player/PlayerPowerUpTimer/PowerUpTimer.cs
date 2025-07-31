using System.Collections;
using UnityEngine;

public class PowerUpTimer : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Coroutine gravityCoroutine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void StartGravityPowerUp(float newGravityScale, float duration) 
    {
        if (gravityCoroutine != null) 
        {
            StopCoroutine(gravityCoroutine);
        }

        gravityCoroutine = StartCoroutine(GravityEffectRoutine(newGravityScale, duration));
    }

    private IEnumerator GravityEffectRoutine(float newGravity, float duration) 
    {
        float originalGravity = rb.gravityScale;
        rb.gravityScale = newGravity;
        float timer = duration;

        Debug.Log("Gravity Lightened!");

        while (timer > 0f) 
        {
            timer -= Time.deltaTime;
            Debug.Log("Gravity timer: " + timer.ToString("F2") + "s");
            yield return null;

        }

        rb.gravityScale = originalGravity;
        Debug.Log("Gravity back to normal!");
    }
}
