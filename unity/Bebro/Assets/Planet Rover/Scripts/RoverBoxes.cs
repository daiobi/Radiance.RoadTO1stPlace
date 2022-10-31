using UnityEngine;

namespace Rover
{
    public class RoverBoxes : MonoBehaviour
    {
        [SerializeField] private RoverBox _green;
        [SerializeField] private RoverBox _yellow;
        [SerializeField] private RoverBox _red;

        public BoxState GreenState => _green.BoxState;
        public BoxState YellowState => _yellow.BoxState;
        public BoxState RedState => _red.BoxState;

        public void OpenGreen()
        {
            _green.Open();
            _yellow.Close();
            _red.Close(); 
        }

        public void OpenYellow()
        {
            _green.Close();
            _yellow.Open();
            _red.Close();
        }

        public void OpenRed()
        {
            _green.Close();
            _yellow.Close();
            _red.Open();
        }

        public void CloseAll()
        {
            _green.Close();
            _yellow.Close();
            _red.Close();
        }
    }
}