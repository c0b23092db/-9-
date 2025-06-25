using UnityEngine;
using UnityEngine.EventSystems;

public class commonMenuButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int menuIndex;
    public void OnPointerEnter(PointerEventData eventData)
    {
        classbasePanel.num_menu = menuIndex;
        if(ManagerDebug.DebugLog)
            Debug.Log("Hover 'Enter' change : " + classbasePanel.num_menu);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        classbasePanel.num_menu = 0;
        if(ManagerDebug.DebugLog)
            Debug.Log("Hover 'Exit' change : " + classbasePanel.num_menu);
    }
    public void OnClick()
    {
        if (classbasePanel.ActivePanel != null)
            classbasePanel.ActivePanel.HandleCursorSelect(menuIndex);
        else
            Debug.LogWarning("No active panel found.");
        if (ManagerDebug.DebugLog)
            Debug.Log("Click : " + menuIndex);
    }
}