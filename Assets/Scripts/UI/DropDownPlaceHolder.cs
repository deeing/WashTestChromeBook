using UnityEngine;
using TMPro;

public class DropDownPlaceHolder : MonoBehaviour
{
    [SerializeField] 
    string defaultText = "Default Text";
    [SerializeField]
    private TMP_Dropdown dropdownMenu;

    private bool init = true;

    void OnGUI()
    {    
        if (init) {
            init = false;
            dropdownMenu.captionText.text = defaultText;
        }
    }

    void OnDisable()
    {
        init = true;
    }
}
