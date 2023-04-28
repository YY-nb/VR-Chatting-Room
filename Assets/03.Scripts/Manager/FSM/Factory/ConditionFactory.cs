using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ConditionFactory 
{
    public static Condition GenerateCondition(bool triggerType, Func<bool> transitionCallback)
    {
        return new Condition { TriggerType = triggerType, TransitionCallback = transitionCallback };
    }
}
