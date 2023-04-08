using Gameplay;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerAnimationsHandler playerAnimator;
    [SerializeField] private BrackeyCharacterController characterController;

    private bool _heldJumpLastFrame, _jump, _crouch;
    private float _moveX;

    private void Awake()
    {
        RespawnAtCheckpoint();
    }

    private void RespawnAtCheckpoint()
    {
        var savedata = SaveManager.GetSave();
        var x = savedata.SavedFloats["CheckpointX"].Value;
        var y = savedata.SavedFloats["CheckpointY"].Value;

        transform.position = new Vector3(x, y);
    }

    private void Update()
    {
        if (Input.GetButton("Jump") || Input.GetAxis("Vertical") > 0.1)
        {
            if (!_heldJumpLastFrame)
            {
                _heldJumpLastFrame = true;
                _jump = true;
            }
        }
        else
        {
            _heldJumpLastFrame = false;
        }

        _crouch = Input.GetAxis("Vertical") < 0.1;
        _moveX = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnAtCheckpoint();
        }
    }

    private void FixedUpdate()
    {
        characterController.Move(_moveX, _crouch, _jump);
        _jump = false;
    }

}
