using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator animator;
    public string animationTriggerName;
    public Transform playerTransform;
    public float detectionRadius = 1f;
    private bool triggerSet = false; // Prevents setting the trigger multiple times

    void Update()
    {
        Collider[] hitColliders = Physics.OverlapSphere(playerTransform.position, detectionRadius);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.CompareTag("Trigger") && !triggerSet)
            {
                animator.SetTrigger(this.animationTriggerName);
                triggerSet = true; // Ensures the trigger is only set once
                break; // Exit the loop once the trigger is set
            }
        }

        // Optionally, reset the flag when the player moves out of range
        if (hitColliders.Length == 0)
        {
            triggerSet = false;
        }
    }
}
