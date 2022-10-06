using Rover;
using UnityEngine;

public class RadarMinigame : MonoBehaviour
{
    [SerializeField] private GameObject _minigamePanel;
    [SerializeField] private Transform _radarRotation;
    [SerializeField] private Move _minigame;
    private bool _isWon;

    private void Start()
    {
        _minigame.OnWin.AddListener(HandleWin);
    }

    private void Update()
    {
        if (_isWon)
            _radarRotation.rotation = Quaternion.Slerp(_radarRotation.rotation, Quaternion.Euler(0, 180, 0), Time.deltaTime * 0.1f);
    }

    private void HandleWin()
    {
        _isWon = true;
        Tasks.SetRadarFixed();
        CloseMinigame();
    }

    public void CloseMinigame()
    {
        _minigamePanel.SetActive(false);
    }

    public void OpenMinigame()
    {
        if (!_isWon)
        {
            _minigamePanel.SetActive(true);
        }
    }
}
