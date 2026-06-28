using UdonSharp;
using UnityEngine;

public class AreaActivationManager : UdonSharpBehaviour
{
    public GameObject[] explorationAreas;

    private void Start()
    {
        DeactivateAllAreas();
    }

    public void ActivateArea(GameObject areaObject)
    {
        if (areaObject == null)
        {
            Debug.Log("[AreaActivationManager] areaObject가 null");
            return;
        }

        DeactivateAllAreas();

        areaObject.SetActive(true);

        Debug.Log("[AreaActivationManager] 지역 활성화: " + areaObject.name);
    }

    public void DeactivateAllAreas()
    {
        if (explorationAreas == null)
        {
            Debug.Log("[AreaActivationManager] explorationAreas가 null");
            return;
        }

        for (int i = 0; i < explorationAreas.Length; i++)
        {
            GameObject area = explorationAreas[i];

            if (area == null) continue;

            area.SetActive(false);
        }

        Debug.Log("[AreaActivationManager] 모든 탐사 지역 비활성화");
    }
}