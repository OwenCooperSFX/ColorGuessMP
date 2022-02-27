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
        if (!TargetObject)
            TargetObject = gameObject;
    }

    private void Start()
    {
        PopuplateUI();
    }

    void PopuplateUI()
    {
        //Revisit this after refactoring dependencies.

        //foreach (GameObject uiElement in uiElements)
        //{
        //    TargetObject = uiElement;
            
        //    Tweener_Simple targetTweener = TargetObject.AddComponent<Tweener_Simple>();
        //    TweenDataSO tweenData = targetTweener.TweenDataSoRef;

        //    TargetObject.SetActive(false);

            
        //    tweenData.StartDelay = delayBetween * ( 1 + uiElements.IndexOf(uiElement));
        //    tweenData.Destination = TargetObject.transform.localPosition;
        //    tweenData.Duration = Duration;
        //    targetTweener.LoopSetting = LoopSetting;
        //    targetTweener.Loops = Loops;
        //    targetTweener.EaseSetting = EaseSetting;

        //    TargetObject.transform.localPosition += StartOffset;
        //    TargetObject.SetActive(true);
            
        //    targetTweener.SetDOTweenType(TweenAnimType);
        //    print(uiElements.IndexOf(uiElement));
        //    print(targetTweener.StartDelay);
        //}
    }

    void SetDirection()
    {
        //Revisit this after refactoring dependencies.

        //switch (direction)
        //    {
        //        case Direction.Up:
        //            break;
        //        case Direction.Down:
        //            break;
        //        case Direction.Left:
        //            break;
        //        case Direction.Right:
        //            break;
        //    }
    }
}
