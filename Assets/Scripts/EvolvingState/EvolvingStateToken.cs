using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EvolvingStateToken : EvolvingStateBase
{
    bool m_isOn;

    public EvolvingStateToken()
    {
        m_isOn = false;
    }

    public EvolvingStateToken(bool value)
    {
        m_isOn = value;
    }

    public override bool IsOn
    {
        get
        {
            return m_isOn;
        }
    }

    public void SetOn(bool value)
    {
        m_isOn = value;
    }
}