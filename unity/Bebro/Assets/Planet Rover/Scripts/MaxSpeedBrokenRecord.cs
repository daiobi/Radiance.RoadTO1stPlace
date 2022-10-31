namespace Rover
{
    public class MaxSpeedBrokenRecord : RoverBrokenRecord
    {
        public readonly int Time;

        public MaxSpeedBrokenRecord()
        {
            Time = (int)UnityEngine.Time.time;
        }
    }
}