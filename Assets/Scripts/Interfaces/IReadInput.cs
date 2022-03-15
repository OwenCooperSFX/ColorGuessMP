using System.Collections;
using UnityEngine;

public interface IReadInput
{
    void HandleInputPressed(PlayerController_new callingPlayer, KeyCode keyCode, ButtonInput buttonInput);
}
