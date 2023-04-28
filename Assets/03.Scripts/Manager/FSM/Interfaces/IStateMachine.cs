using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStateMachine<T>
{
    /// <summary>
    /// ×´Ì¬Ö÷Ìå
    /// </summary>
    T Owner { get; }

    /// <summary>
    /// Ä¬ÈÏ×´Ì¬
    /// </summary>
    Enum DefaultStateID { get; set; }

    /// <summary>
    /// Ìí¼Ó×´Ì¬
    /// </summary>
    /// <param name="state"></param>
    IStateMachine<T> AddState(Enum stateID);

    /// <summary>
    /// ÇÐ»»×´Ì¬
    /// </summary>
    /// <param name="targetState"></param>
    void SwitchState(Enum targetStateID);

    /// <summary>
    /// ÅäÖÃ×ª»»×´Ì¬
    /// </summary>
    /// <param name="fromSateID"></param>
    /// <param name="toStateID"></param>
    /// <param name="transitionCallback"></param>
    /// <returns></returns>
    IStateMachine<T> AddTransition(Enum fromSateID, Enum toStateID, Condition condition);

    /// <summary>
    /// ´ò¿ª¸´ºÏ×´Ì¬µÄ×Ó×´Ì¬»ú
    /// </summary>
    /// <param name="stateID">¸´ºÏ×´Ì¬ID</param>
    /// <returns>×Ó×´Ì¬»ú</returns>
    IStateMachine<T> Open(Enum stateID);
}
