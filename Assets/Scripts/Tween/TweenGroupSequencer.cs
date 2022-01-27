using UnityEngine;
using DG.Tweening;

public class TweenGroupSequencer : MonoBehaviour
{
    public TweenData tweenData;
    public GameObject[] targetObjects;

    Sequence sequence;
    private bool sequenceTriggered;

    [Tooltip("How long to wait (in seconds) between each tween.")]
    public float delay = 0f;

    private void Awake()
    {
        sequence = DOTween.Sequence();
        BuildSequence();
    }

    void BuildSequence()
    {
        sequence.Pause();
        sequence.SetAutoKill(false);

        for (int i = 0; i < targetObjects.Length; i++)
        {
            tweenData.targetObject = targetObjects[i];

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
