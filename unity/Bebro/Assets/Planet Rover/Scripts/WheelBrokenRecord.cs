namespace Rover
{
    public class WheelBrokenRecord : RoverBrokenRecord
    {
        public readonly int Time;
        public readonly int WheelNumber;

        public WheelBrokenRecord(int n)
        {
            Time = (int)UnityEngine.Time.time;
            WheelNumber = n;
        }
    }
}