using UnityEngine;

public class DamageReceiver : MonoBehaviour
{
    [SerializeField] Health health;

    [SerializeField] float coveredAngle = 90f;

    private bool isBlocking = false;

    public void SetIsBlocking(bool state)
    {
        isBlocking = state;
    }

    public void DealDamage(Transform attacker, int damageAmount)
    {
        if (isBlocking && AttackerInCoverage(attacker)) { return; }

        health.DealDamage(damageAmount);
    }

    private bool AttackerInCoverage(Transform other)
    {
        var requiredValue = Mathf.Cos(coveredAngle * Mathf.Deg2Rad);
        var directionToAttacker = (other.position - transform.position).normalized;
        var dotProduct = Vector3.Dot(transform.forward, directionToAttacker);
        return dotProduct >= requiredValue;
    }

   
}
