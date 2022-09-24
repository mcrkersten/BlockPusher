using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Buttons : MonoBehaviour
{
    public List<Button> buttons = new List<Button>();

    [Header("Hover behaviour")]
    [SerializeField] private RestState restState;
    [SerializeField] private Sprite solid;
    [SerializeField] private Sprite unSolid;
    [SerializeField] private Color textColor;

    public void SelectButton(int index)
    {
        buttons[index].Select();
    }

    private void OnDisable()
    {
        foreach (Button b in buttons)
            OnHoverEnd(b.gameObject);
    }

    public void OnHoverStart(GameObject button)
    {
        Image image = button.GetComponent<Image>();
        switch (restState)
        {
            case RestState.Solid:
                SetToHollow(button);
                break;
            case RestState.UnSolid:
                SetToSolid(button);
                break;
            case RestState.NotSet:
                break;
        }
    }

    public void OnHoverEnd(GameObject button)
    {
        switch (restState)
        {
            case RestState.Solid:
                SetToSolid(button);
                break;
            case RestState.UnSolid:
                SetToHollow(button);
                break;
            case RestState.NotSet:
                break;
        }
    }

    private void SetToSolid(GameObject button)
    {
        Image image = button.GetComponent<Button>().image;
        image.sprite = solid;

        //Text color
        foreach (TextMeshProUGUI t in button.GetComponentsInChildren<TextMeshProUGUI>())
            t.color = textColor;

        //Image color
        foreach (Image t in button.GetComponentsInChildren<Image>().Where(go => go.gameObject != button.gameObject))
            if(image != t)
                t.color = textColor;
    }

    public void SetToHollow(GameObject button)
    {
        Image image = button.GetComponent<Button>().image;
        image.sprite = unSolid;

        //Text color
        foreach (TextMeshProUGUI t in button.GetComponentsInChildren<TextMeshProUGUI>())
            t.color = image.color;

        //Image color
        foreach (Image t in button.GetComponentsInChildren<Image>().Where(go => go.gameObject != button.gameObject))
            t.color = image.color;
    }

    private enum RestState
    {
        NotSet = default,
        Solid,
        UnSolid,
    }
}
