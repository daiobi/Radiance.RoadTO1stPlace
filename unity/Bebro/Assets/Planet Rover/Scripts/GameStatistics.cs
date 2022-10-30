using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rover
{
    public class GameStatistics : MonoBehaviour
    {
        public static GameStatistics Instance { get; private set; }
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
        private string GetTime(int time)
        {
            return $"{time / 60}:{time % 60}";
        }

        public string MissionTime
        {
            get
            {
                if (Tasks.Instance.MissionEndTime == null || Tasks.Instance.MissionStartTime == null)
                {
                    return "-";
                }

                return GetTime((int)(Tasks.Instance.MissionEndTime.Value - Tasks.Instance.MissionStartTime.Value));
            }
        }

        public string StartToRadarTime
        {
            get
            {
                if (Tasks.Instance.MissionStartTime == null || Tasks.Instance.RadarFixStartTime == null)
                {
                    return "-";
                }

                return GetTime((int)(Tasks.Instance.RadarFixStartTime.Value - Tasks.Instance.MissionStartTime.Value));
            }
        }

        public string RadarFixTime
        {
            get
            {
                if (Tasks.Instance.RadarFixStartTime == null || Tasks.Instance.RadarFixEndTime == null)
                {
                    return "-";
                }

                return GetTime((int)(Tasks.Instance.RadarFixEndTime.Value - Tasks.Instance.RadarFixStartTime.Value));
            }
        }

        public string RadarToSamplesTime
        {
            get
            {
                if (Tasks.Instance.RadarFixEndTime == null || Tasks.Instance.SamplesCollectStartTime == null)
                {
                    return "-";
                }

                return GetTime((int)(Tasks.Instance.SamplesCollectStartTime.Value - Tasks.Instance.RadarFixEndTime.Value));
            }
        }

        public string SamplesTime
        {
            get
            {
                if (Tasks.Instance.SamplesCollectStartTime == null || Tasks.Instance.SamplesCollectEndTime == null)
                {
                    return "-";
                }

                return GetTime((int)(Tasks.Instance.SamplesCollectEndTime.Value - Tasks.Instance.SamplesCollectStartTime.Value));
            }
        }

        public string SamplesToFinishTime
        {
            get
            {
                if (Tasks.Instance.MissionEndTime == null || Tasks.Instance.SamplesCollectEndTime == null)
                {
                    return "-";
                }

                return GetTime((int)(Tasks.Instance.MissionEndTime.Value - Tasks.Instance.SamplesCollectEndTime.Value));
            }
        }

        public void RegisterEvent(RoverBrokenRecord record)
        {
            _records.Add(record);
        }


        public string GetTimeStats()
        {
            return $"Затрачено времени: {MissionTime}\n" + 
                $"Время от старта до радара: {StartToRadarTime}\n" +
                $"Время от радара до места сбора образцов: {RadarToSamplesTime}\n" +
                $"Время от сбора образцов до финиша: {SamplesToFinishTime}";
        }

        public string GetBreakStats()
        {
            string s = "";
            foreach(var r in _records)
            {
                if (r is WheelBrokenRecord)
                    s += $"[{GetTime((r as WheelBrokenRecord).Time)}] Сломано колесо {(r as WheelBrokenRecord).WheelNumber} (-1 ед.тех.сост.)\n";
                else if (r is MaxSpeedBrokenRecord)
                    s += $"[{GetTime((r as MaxSpeedBrokenRecord).Time)}] Движение на макс. скорости более 30с (-1 ед.тех.сост.)\n";
            }

            return s;
        }

        private string GetStatus(bool status)
        {
            return status ? "да" : "нет";
        }

        public string GetRadarStats()
        {
            return $"Время выполнения: {RadarFixTime}\n" +
                $"Количество попыток: {RadarFixAttemts}\n" +
                $"Задание выполнено: {GetStatus(Tasks.Instance.RadarFixed)}\n";
        }

        public string GetSamplesCollectStats()
        {
            return $"Время выполнения: {SamplesTime}\n" +
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