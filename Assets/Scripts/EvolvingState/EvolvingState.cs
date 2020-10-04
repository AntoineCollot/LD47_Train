using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvolvingState : EvolvingStateBase
{
    readonly List<EvolvingStateBase> m_tokens = new List<EvolvingStateBase>();

    /// <summary>
    /// True if any token is true
    /// </summary>
    public override bool IsOn
    {
        get
        {
            foreach (EvolvingStateBase token in m_tokens)
            {
                if (token.IsOn)
                    return true;
            }
            return false;
        }
    }

    public void Add(EvolvingStateBase token)
    {
        m_tokens.Add(token);
    }

    public void Remove(EvolvingStateBase token)
    {
        m_tokens.Remove(token);
    }
}

public abstract class EvolvingStateBase
{
    public abstract bool IsOn { get; }
}
