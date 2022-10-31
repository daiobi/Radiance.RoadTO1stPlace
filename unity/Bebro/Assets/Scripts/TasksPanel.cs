using Rover;
using UnityEngine;

public class TasksPanel : MonoBehaviour
{
    [SerializeField] private GameObject _roverActivatedCheck;
    [SerializeField] private GameObject _radarFixedCheck;
    [SerializeField] private GameObject _radarPhotoCheck;
    [SerializeField] private GameObject _greenSampleCollectedCheck;
    [SerializeField] private GameObject _yellowSampleCollectedCheck;
    [SerializeField] private GameObject _redSampleCollectedCheck;
    [SerializeField] private GameObject _samplesPhotoCheck;
    [SerializeField] private GameObject _roverDeactivatedCheck;

    private void Update()
    {
        _roverActivatedCheck.SetActive(Tasks.Instance.GamePhase != GamePhase.BeforeStart);
        _radarFixedCheck.SetActive(Tasks.Instance.RadarFixed);
        _radarPhotoCheck.SetActive(GameStatistics.Instance.RadarPhoto);
        _greenSampleCollectedCheck.SetActive(Tasks.Instance.GreenCollected);
        _yellowSampleCollectedCheck.SetActive(Tasks.Instance.YellowCollected);
        _redSampleCollectedCheck.SetActive(Tasks.Instance.RedCollected);
        _samplesPhotoCheck.SetActive(GameStatistics.Instance.SamplesPhoto);
        _roverDeactivatedCheck.SetActive(Tasks.Instance.GamePhase == GamePhase.RoverTurnedOff);
    }
}
