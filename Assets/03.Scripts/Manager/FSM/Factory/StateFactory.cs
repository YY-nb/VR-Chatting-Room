using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// <summary>
/// ×´Ì¬¹¤³§
/// </summary>
public static class StateFactory
{
    public static State<T> GetState<T>(object stateMachine, Enum stateID)
    {
        Type stateType = EnumToTypeFactory.GetType(stateID);

        ConstructorInfo ctor = stateType.GetConstructors(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)[0];

        return ctor.Invoke(new object[] { stateMachine, stateID }) as State<T>;
    }
}
