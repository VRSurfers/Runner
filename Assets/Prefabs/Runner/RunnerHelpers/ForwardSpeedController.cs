using UnityEngine;

class ForwardSpeedController
{
    private readonly float normalForwardSpped;
    private readonly float normalForwardSpeedRestoringAxeleration;
    private float currentSpped;

    public ForwardSpeedController(float normalForwardSpped, float normalForwardSpeedRestoringAxeleration)
    {
        this.normalForwardSpped = normalForwardSpped;
        this.normalForwardSpeedRestoringAxeleration = normalForwardSpeedRestoringAxeleration;
        currentSpped = normalForwardSpped;
    }

    internal float GetForwardSpeedAndTick()
    {
        float oldSpeed = currentSpped;
        if (currentSpped < normalForwardSpped)
        {
            currentSpped += Time.deltaTime * normalForwardSpeedRestoringAxeleration;
        }
        return oldSpeed;
    }

    internal void SetMaxCurrentSppen(float newMaxCurrentSpeed)
    {
        if (currentSpped > newMaxCurrentSpeed)
            currentSpped = newMaxCurrentSpeed;
    }
}
