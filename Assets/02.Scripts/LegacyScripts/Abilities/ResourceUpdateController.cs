using UnityEngine;

public class ResourceUpdateController: MonoBehaviour
{
    public ResourceController selectedResource;
    public StatHandler statHandler;

    public void GetStatsByObject(GameObject gameObject)
    {
        selectedResource = gameObject.GetComponent<ResourceController>();
        statHandler = gameObject.GetComponent<StatHandler>();
    }

    public void UpdateCurrentHealth(ResourceController resourceController, float increaseAmount)
    {
        resourceController.ChangeHealth(resourceController.CurrentHealth + increaseAmount);
    }

    public void UpdateSpeed(StatHandler statHandler, float increaseAmount)
    {
        statHandler.Speed += increaseAmount;
    }
}