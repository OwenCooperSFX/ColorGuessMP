using UnityEngine;
using System.Collections.Generic;

public enum Direction { Up, Down, Left, Right }
public class UIPopulator : Tweener_Simple
{
    public List<GameObject> uiElements;

    public float delayBetween;
    public float spacing;
    public Direction direction;

    private void Awake()
    {
        if (!targetObject)
            targetObject = gameObject;
    }

    private void Start()
    {
        PopuplateUI();
    }

    void PopuplateUI()
    {
        foreach (GameObject uiElement in uiElements)
        {
            targetObject = uiElement;
            
            Tweener_Simple targetTweener = targetObject.AddComponent<Tweener_Simple>();
            
            targetObject.SetActive(false);
            
            targetTweener.startDelay = delayBetween * ( 1 + uiElements.IndexOf(uiElement));
            targetTweener.destination = targetObject.transform.localPosition;
            targetTweener.duration = duration;
            targetTweener.loopType = loopType;
            targetTweener.loops = loops;
            targetTweener.ease = ease;

            targetObject.transform.localPosition += startOffset;
            targetObject.SetActive(true);
            
            targetTweener.SetDOTweenType(tweenAnimType);
            print(uiElements.IndexOf(uiElement));
            print(targetTweener.startDelay);
        }
    }

    void SetDirection()
    {
        switch (direction)
        {
            case Direction.Up:
                break;
            case Direction.Down:
                break;
            case Direction.Left:
                break;
            case Direction.Right:
                break;
        }
    }
}
