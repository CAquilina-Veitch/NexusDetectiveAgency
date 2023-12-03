using System.Collections;
using UnityEngine;

public class DronePlatform : MonoBehaviour
{
    [SerializeField] Animator anim;
    public BoxCollider b;
    public Vector3 targetPosition;
    float duration = 1f; // Duration in seconds
    float p = 0;
    float vel = 200;
    float bobbingAmount = 0.1f;
    float bobbingSpeed = 1f;
    float startHeight = 30;

    bool isFlying = false;
    Vector3 initialPosition;

    public void Init(Vector3 positionToGoTo)
    {
        targetPosition = positionToGoTo;
        initialPosition = positionToGoTo + Vector3.up * startHeight;
        transform.position = initialPosition;
        isFlying = true;
        
    }


    void FixedUpdate()
    {
        if (isFlying)
        {
            p += duration/vel;
            transform.position = Vector3.Lerp(initialPosition, targetPosition, p);
            
            if (p >= 1f)
            {
                isFlying = false;
                transform.position = targetPosition;
                anim.SetTrigger("Destination");
                b.enabled = true;
                StartCoroutine(Bobbing());
            }
        }
    }

    IEnumerator Bobbing()
    {
        while (!isFlying)
        {
            transform.position = new Vector3(transform.position.x, targetPosition.y + Mathf.Sin(Time.time * bobbingSpeed) * bobbingAmount, transform.position.z);
            yield return null;
        }
    }

    float startTime;

    public void StartFlyingUp()
    {
        p = 0f;
        Vector3 temp = initialPosition;
        initialPosition = targetPosition;
        targetPosition = temp;
        isFlying = true;
        StartCoroutine(DestroyAfterDuration());
        b.enabled = false;
    }

    IEnumerator DestroyAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        isFlying = false;
        Destroy(gameObject);
    }
}
