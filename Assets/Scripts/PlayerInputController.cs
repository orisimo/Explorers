using UnityEngine;
public class PlayerInputController : MonoBehaviour
{
    [SerializeField] private int _playerIndex;

    public PlayerInputData InputData => _playerInputData;
    private ControllerStrings _controllerStrings;
    private PlayerInputData _playerInputData;
    
    private void Awake()
    {
        Init();
    }
    
    private void Init()
    {
        _controllerStrings = new ControllerStrings(_playerIndex);
    }
    
    void Update()
    {
        SampleInput();
    }

    private void SampleInput()
    {
        _playerInputData.Button1DownDuration = Input.GetButton(_controllerStrings.Button1) ? _playerInputData.Button1DownDuration + Time.deltaTime : 0f;
        _playerInputData.Button2DownDuration = Input.GetButton(_controllerStrings.Button2) ? _playerInputData.Button2DownDuration + Time.deltaTime : 0f;
        _playerInputData.Button1Down = Input.GetButtonDown(_controllerStrings.Button1);
        _playerInputData.Button2Down = Input.GetButtonDown(_controllerStrings.Button2);
        _playerInputData.MovementVector.x = Input.GetAxis(_controllerStrings.HorizontalAxis);
        _playerInputData.MovementVector.z = Input.GetAxis(_controllerStrings.VerticalAxis);
    }
}
