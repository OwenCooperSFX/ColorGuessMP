using UnityEngine;

public class ColorObjectButton : ColorObjectBase, IReadInput
{
    private void OnEnable()
    {
        EventManager.OnButtonInput += HandleInputPressed;
    }

    private void OnDisable()
    {
        EventManager.OnButtonInput -= HandleInputPressed;
    }

    public void HandleInputPressed(PlayerController_new callingPlayer, KeyCode keyCode, ButtonInput buttonInput)
    {
        if ((callingPlayer != OwningPlayer) || (buttonInput != ButtonAssignment))
            return;

        EventManager.RaiseColorObjectButtonSelected(this);

        float flashDuration = 0.2f;

        if (_tweenerSimple)
        {
            _tweenerSimple.PlayTween();
            flashDuration = 2 * _tweenerSimple.TweenData.Duration;
        }

        if (_colorLight)
        {
            _colorLight.LightFlashTime = flashDuration;
            _colorLight.FlashLight();
        }
    }
}
