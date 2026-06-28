using UdonSharp;
using UnityEngine;

public class AreaActivationManager : UdonSharpBehaviour
{
    public GameObject[] explorationAreas;

    private GameObject currentActiveArea;

    private void Start()
    {
        DeactivateAllAreas();
    }

    public void ActivateArea(GameObject areaToActivate)
    {
        if (areaToActivate == null)
        {
            Debug.Log("[AreaActivationManager] 활성화할 지역이 null");
            return;
        }

        if (!IsManagedArea(areaToActivate))
        {
            Debug.Log("[AreaActivationManager] explorationAreas에 등록되지 않은 지역: " + areaToActivate.name);
            return;
        }

        DeactivateAllAreas();

        areaToActivate.SetActive(true);
        currentActiveArea = areaToActivate;

        Debug.Log("[AreaActivationManager] 지역 활성화: " + areaToActivate.name);
    }

    public void DeactivateAllAreas()
    {
        currentActiveArea = null;

        if (explorationAreas == null)
        {
            Debug.Log("[AreaActivationManager] explorationAreas가 null");
            return;
        }

        for (int i = 0; i < explorationAreas.Length; i++)
        {
            GameObject area = explorationAreas[i];

            if (area == null)
            {
                Debug.Log("[AreaActivationManager] explorationAreas[" + i + "]가 null");
                continue;
            }

            area.SetActive(false);
        }

        Debug.Log("[AreaActivationManager] 모든 탐험 지역 비활성화");
    }

    public bool IsManagedArea(GameObject area)
    {
        if (area == null)
        {
            return false;
        }

        if (explorationAreas == null)
        {
            return false;
        }

        for (int i = 0; i < explorationAreas.Length; i++)
        {
            if (explorationAreas[i] == area)
            {
                return true;
            }
        }

        return false;
    }

    public GameObject GetCurrentActiveArea()
    {
        return currentActiveArea;
    }
}