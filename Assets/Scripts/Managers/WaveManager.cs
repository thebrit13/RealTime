using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private Transform _SpawnLoc;

    private List<Data.Wave> _WaveInfo;

    private int _CurrentWave = -1;

    private List<Coroutine> _CoroutineList = new List<Coroutine>();

    private void Awake()
    {
        EventManager.StartGameLogic += StartWaveLogic;
    }

    public void Setup(List<Data.Wave> waveInfo)
    {
        _WaveInfo = waveInfo;
    }       

    public void StartWaveLogic()
    {
        _CurrentWave = 1;
        if(_WaveInfo != null)
        {
            StartWave(_CurrentWave);
        }
        else
        {
            Debug.LogError("Waves List is Null");
        }
    }

    public void StartWave(int waveNumber)
    {
        Debug.Log(string.Format("Starting Wave {0}", waveNumber));
        Data.Wave wave = _WaveInfo.Find(o => o.Number == waveNumber);
        if(wave != null)
        {
            foreach(Data.WaveUnitInfo unitInfo in wave.Units)
            {
                Coroutine tempCo = StartCoroutine(SpawnCoroutine(unitInfo));
                _CoroutineList.Add(tempCo);
            }       
        }
    }

    IEnumerator SpawnCoroutine(Data.WaveUnitInfo waveInfo)
    {
        yield return new WaitForSeconds(waveInfo.SpawnTime);
        //Will want to clean this up
        Data.Unit unitInfo = PlayerManager.Instance.DataManager.DataClass.Units.GetUnitByID(waveInfo.ID);
        for(int i = 0; i < waveInfo.Amt;i++)
        {
            EventManager.OnCreateUnit(unitInfo, _SpawnLoc.position,2);
        }
    }

    private void OnDestroy()
    {
        EventManager.StartGameLogic -= StartWaveLogic;

        foreach(Coroutine co in _CoroutineList)
        {
            if(co != null)
            {
                StopCoroutine(co);
            }
        }
    }
}
