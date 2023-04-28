using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectionPanel : BaseAnimatedPanel
{
    protected override void OnClick(Button button)
    {
        base.OnClick(button);
        MusicManager.Instance.StartSound("PokeAudio");
        switch (button.name)
        {
            case "Meeting Room Button":

                break;

            case "Jet Room Button":

                break;
        }

    }


}
