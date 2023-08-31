using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _SpawnLoc;

    private List<Data.Wave> _WaveInfo;

    private int _CurrentWave;

    private List<Coroutine> _CoroutineList = new List<Coroutine>();

    private void Awake()
    {
        EventManager.StartGameLogic += StartWaveLogic;
        EventManager.WaveComplete += StartNextWave;
    }

    public void Setup(List<Data.Wave> waveInfo)
    {
        _WaveInfo = waveInfo;
    }       

    public void StartWaveLogic()
    {
        if(_WaveInfo != null)
        {
            StartNextWave();
        }
        else
        {
            Debug.LogError("Waves List is Null");
        }
    }

    public void StartNextWave()
    {
        ++_CurrentWave;
        Debug.Log(string.Format("Starting Wave {0}", _CurrentWave));
        Data.Wave wave = _WaveInfo.Find(o => o.Number == _CurrentWave);
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
            EventManager.OnCreateUnit(unitInfo, _SpawnLoc[Random.Range(0,_SpawnLoc.Count)].position,2);
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

        EventManager.WaveComplete -= StartNextWave;
    }
}
