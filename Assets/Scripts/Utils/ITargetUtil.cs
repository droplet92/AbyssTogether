using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;

public static class ITargetUtil
{
    public static ITarget GetTargetUnderMouse(Canvas canvas)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        List<RaycastResult> results = new List<RaycastResult>();
        canvas.GetComponent<GraphicRaycaster>().Raycast(pointerData, results);

        foreach (var result in results)
        {
            var target = result.gameObject.GetComponentInParent<ITarget>();
            if (target != null)
                return target;
        }
        return null;
    }
}
