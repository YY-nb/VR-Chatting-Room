using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HintPanel : BaseAnimatedPanel
{
    private TextMeshProUGUI hintText;
    protected override void Awake()
    {
        base.Awake();
        hintText = GetUIComponentByName<TextMeshProUGUI>("HintText");
    }
    public void ShowHint(object text)
    {
        hintText.text = (string)text;
    }
}
