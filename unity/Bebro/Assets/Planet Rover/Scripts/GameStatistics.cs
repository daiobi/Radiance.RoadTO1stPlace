using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class GameStatistics : MonoBehaviour
    {
        public static GameStatistics Instance { get; private set; }

        public float MissionTime => Tasks.Instance.MissionEndTime - Tasks.Instance.MissionStartTime;
        public float StartToRadarTime => Tasks.Instance.RadarFixStartTime - Tasks.Instance.MissionStartTime;
        public float RadarFixTime => Tasks.Instance.RadarFixEndTime - Tasks.Instance.RadarFixStartTime;
        public float RadarToSamplesTime => Tasks.Instance.RadarFixEndTime - Tasks.Instance.SamplesCollectStartTime;
        public float SamplesCollectingTime => Tasks.Instance.SamplesCollectEndTime - Tasks.Instance.SamplesCollectStartTime;
        public float SamplesToEndTime => Tasks.Instance.SamplesCollectStartTime - Tasks.Instance.SamplesCollectEndTime;
        public float MaxSpeedTime { get; set; }
        public float FullMaxSpeedTime { get; set; }
        
        public int RadarFixAttemts { get; set; }
        public int CollectedSamples { get; set; }
        public int BrokenSamples { get; set; }

        private List<RoverBrokenRecord> _records = new List<RoverBrokenRecord>();

        public Texture2D RadarPhoto { get; set; }
        public Texture2D SamplesPhoto { get; set; }

        public int NoiseCollected { get; set; }
        public bool SignalLost { get; set; }

        private void Awake()
        {
            if (Instance != null)
            {
                throw new System.InvalidOperationException($"{GetType().Name} singleton error");
            }

            Instance = this;
        }

        public void RegisterEvent(RoverBrokenRecord record)
        {
            _records.Add(record);
        }

        private string GetTime(int time)
        {
            if (time == 0) {
                return "-";
            }
            return $"{time / 60}:{time % 60}";
        }

        public string GetTimeStats()
        {
            return $"Затрачено времени: {GetTime((int)MissionTime)}\n" + 
                $"Время от старта до радара: {GetTime((int)StartToRadarTime)}\n" +
                $"Время от радара до места сбора образцов: {GetTime((int)RadarToSamplesTime)}\n" +
                $"Время от сбора образцов до финиша: {GetTime((int)SamplesToEndTime)}";
        }

        public string GetBreakStats()
        {
            string s = "";
            foreach(var r in _records)
            {
                if (r is WheelBrokenRecord)
                    s += $"[{GetTime((r as WheelBrokenRecord).Time)}] Сломано колесо {(r as WheelBrokenRecord).WheelNumber} (-1 ед.тех.сост.)\n";
                else if (r is MaxSpeedBrokenRecord)
                    s += $"[{GetTime((r as WheelBrokenRecord).Time)}] Движение на макс. скорости более 30с (-1 ед.тех.сост.)\n";
            }

            return s;
        }

        private string GetStatus(bool status)
        {
            return status ? "да" : "нет";
        }

        public string GetRadarStats()
        {
            return $"Время выполнения: {GetTime((int)RadarFixTime)}\n" +
                $"Количество попыток: {RadarFixAttemts}\n" +
                $"Задание выполнено: {GetStatus(Tasks.Instance.RadarFixed)}\n";
        }

        public string GetSamplesCollectStats()
        {
            return $"Время выполнения: {GetTime((int)SamplesCollectingTime)}\n" +
                $"Собрано образцов: {CollectedSamples}\n" +
                $"Разрушено образцов: {BrokenSamples}\n";
        }

        public string GetPhotosStats()
        {
            return $"Фотофиксация радара: {GetStatus(RadarPhoto != null)}\n" +
                $"Фотофиксация места сбора образцов: {GetStatus(SamplesPhoto != null)}\n";
        }

        public string GetSignalStats()
        {
            return $"Количество пойманных помех: {NoiseCollected}\n" +
                $"Потеряно соединение с ровером: {GetStatus(SignalLost)}\n";
        }
    }
}