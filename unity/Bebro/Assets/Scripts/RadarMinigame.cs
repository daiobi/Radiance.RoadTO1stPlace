using Rover;
using UnityEngine;

public class RadarMinigame : MonoBehaviour
{
    [SerializeField] private GameObject _minigamePanel;
    [SerializeField] private Transform _radarRotation;
    [SerializeField] private Move _minigame;
    private bool _isWon;
    private bool _isOpened;

    private void Start()
    {
        _minigame.OnWin.AddListener(HandleWin);
    }

    private void Update()
    {
        if (_isWon)
            _radarRotation.rotation = Quaternion.Slerp(_radarRotation.rotation, Quaternion.Euler(0, -90, 0), Time.deltaTime * 0.5f);
    }

    private void HandleWin()
    {
        _isWon = true;
        Tasks.SetRadarFixed();
        CloseMinigame();
    }

    public void CloseMinigame()
    {
        _isOpened = true;
        _minigamePanel.SetActive(false);
    }

    public void OpenMinigame()
    {
        if (!_isWon)
        {
            _isOpened = true;
            _minigamePanel.SetActive(true);
        }
    }

    public void HandleButtonClick()
    {
        if (_isOpened)
        {
            _minigame.btn();
        }
    }
}
