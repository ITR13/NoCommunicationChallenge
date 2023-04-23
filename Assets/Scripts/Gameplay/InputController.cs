using Gameplay;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [SerializeField] private PlayerAnimationsHandler playerAnimator;
    [SerializeField] private BrackeyCharacterController characterController;

    private bool _heldJumpLastFrame, _jump, _jumpEnded, _crouch;
    private float _moveX;
    private SaveFile saveFile;

    private void Awake()
    {
        RespawnAtCheckpoint();
        saveFile = SaveManager.GetSave();
    }

    public void RespawnAtCheckpoint()
    {
        var savedata = SaveManager.GetSave();
        var x = savedata.SavedFloats["CheckpointX"].Value;
        var y = savedata.SavedFloats["CheckpointY"].Value;

        transform.position = new Vector3(x, y);
    }

    private void Update()
    {
        // Read the inputs for jumping
        if (Input.GetButton("Jump"))
        {
            if (!_heldJumpLastFrame)
            {
                _heldJumpLastFrame = true;
                _jump = true;
                _jumpEnded = false;
            }
        }
        else
        {
            _heldJumpLastFrame = false;
        }


        // Read the inputs for crouching
        _crouch = Input.GetAxis("Vertical") < -0.1;
        // Read the inputs for moving
        _moveX = Input.GetAxis("Horizontal");

        if (Input.GetKeyDown(KeyCode.R))
        {
            RespawnAtCheckpoint();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            SaveManager.DeleteSave();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            saveFile.SavedInts["Money"].SetValue(saveFile.SavedInts["Money"].Value + 1);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    private void FixedUpdate()
    {
        characterController.Move(_moveX, _crouch, _jump, _jumpEnded, _crouch);
        _jump = false;
    }

}
