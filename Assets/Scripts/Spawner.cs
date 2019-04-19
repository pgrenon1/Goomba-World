using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    public GameObject GoombaPrefab;
    public float Speed;
    public float SpeedGrowth;
    public float MinTimer;
    public float MaxTimer;

    private float _timer;

    // Use this for initialization
    void Start()
    {
        ResetTimer();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.Paused) return;

        _timer -= Time.deltaTime;
        if (_timer <= 0)
        {
            Spawn();
            ResetTimer();
        }
    }

    void ResetTimer()
    {
        if (GameManager.Instance.Level >= 10)
        {
            MinTimer = MinTimer / 2;
        }
        else if (GameManager.Instance.Level >= 6)
        {
            MinTimer = MinTimer / 1.5f;
        }

        _timer = Random.Range(MinTimer / GameManager.Instance.Level, MaxTimer / GameManager.Instance.Level);
    }

    void Spawn()
    {
        GameObject newGoomba = Instantiate(GoombaPrefab, Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 1, 10)), Quaternion.identity, transform);
        float randomFloat = Random.Range(0.5f, 1.5f);
        newGoomba.transform.localScale = new Vector3(randomFloat, randomFloat, 0);
        newGoomba.transform.DOMoveY(-7, Speed * (GameManager.Instance.Level * SpeedGrowth) ).SetSpeedBased(true).SetEase(Ease.Linear);
    }
}