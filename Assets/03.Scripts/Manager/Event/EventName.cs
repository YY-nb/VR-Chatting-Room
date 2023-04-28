using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventName 
{
   public const string OnKnowledgePanelStateChange = nameof(OnKnowledgePanelStateChange);
   public const string OnSunMaoStateChange = nameof(OnSunMaoStateChange);

    public const string OnCloseUI = nameof(OnCloseUI);
    public const string OnEnableWristUI = nameof(OnEnableWristUI);

    public const string OnResetPlayer = nameof(OnResetPlayer);

    //�î
    public const string OnCreateSunMao = nameof(OnCreateSunMao);
    public const string OnDestroySunMao = nameof(OnDestroySunMao);
    public const string OnChangeSunMaoMaterial = nameof(OnChangeSunMaoMaterial);
    public const string OnPartCorrect = nameof(OnPartCorrect);
    public const string OnEnterNextLevel = nameof(OnEnterNextLevel);
    public const string OnRestart = nameof(OnRestart);
    public const string OnFinishGame =nameof(OnFinishGame);

    //ץȡ
    public const string OnGrab = nameof(OnGrab);

    //UI
    public const string OnEnableAchievementUI = nameof(OnEnableAchievementUI);
    public const string OnEnableRayInteraction = nameof(OnEnableRayInteraction);


}
