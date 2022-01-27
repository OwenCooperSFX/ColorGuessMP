using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TweenSequencer : MonoBehaviour
{
    public List<TweenData> tweenDataList;

    Sequence sequence;

    private bool sequenceTriggered;

    private void Awake()
    {
        sequence = DOTween.Sequence();
        BuildSequence();
    }

    void BuildSequence()
    {
        sequence.Pause();
        sequence.SetAutoKill(false);

        foreach (TweenData tweenData in tweenDataList)
        {
            if (tweenData.join)
                sequence.Join(tweenData.GetTween());
            else
                sequence.Append(tweenData.GetTween());
        }
    }

    public void PlaySequence()
    {
        if (!sequenceTriggered)
            sequence.PlayForward();
        else
            sequence.PlayBackwards();

        sequenceTriggered = !sequenceTriggered;
    }
}
