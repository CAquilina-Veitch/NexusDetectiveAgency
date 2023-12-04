using System.Collections;
using UnityEngine;

public class DronePlatform : MonoBehaviour
{
    [SerializeField] Animator anim;
    public BoxCollider b;
    public Vector3 targetPosition;
    float duration = 1f; // Duration in seconds
    float t = 0;

    bool isGoingDown;

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
        isGoingDown = true;
        isFlying = true;
    }

    void FixedUpdate()
    {
        if (isFlying)
        {
            transform.position = Vector3.Lerp(transform.position, isGoingDown ? targetPosition : initialPosition, 0.2f);
            t += Time.deltaTime;
            if (t >= duration)
            {
                isFlying = false;
                transform.position = isGoingDown ? targetPosition : initialPosition;
                anim.SetTrigger("Destination");
                b.enabled = true;
                StartCoroutine(Bobbing());
                if (!isGoingDown)
                {
                    Destroy(gameObject);

                }
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



    public void StartFlyingUp()
    {
        isGoingDown = false;
        isFlying = true;

        b.enabled = false;
    }
}
