using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public GameObject ShockwavePrefab;
    public float DashDistance;
    public float DashDuration;
    public Sprite DashingSprite;
    public Sprite IdleSprite;

    private SpriteRenderer _spriteRend;
    private bool sendingShockwave = false;

    // Use this for initialization
    void Start() {
        _spriteRend = GetComponentInChildren<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseDown()
    {
        Shockwave();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Enemy>())
        {
            StartCoroutine(GameManager.Instance.Die());
        }
    }

    public void Dash(int side)
    {
        if (GameManager.Instance.Paused) return;

        if (!sendingShockwave)
        {

            transform.DOScale(Vector3.one / 3 * 2, DashDuration / 2).OnComplete(() => transform.DOScale(Vector3.one, DashDuration / 2));
            if (side == 1)
            {
                _spriteRend.sprite = DashingSprite;
                _spriteRend.flipX = false;
                transform.DOBlendableMoveBy(new Vector3(DashDistance, 0,0), DashDuration).OnComplete(EndDash);
            }
            else
            {
                _spriteRend.sprite = DashingSprite;
                _spriteRend.flipX = true;
                transform.DOBlendableMoveBy(new Vector3(-DashDistance, 0, 0), DashDuration).OnComplete(EndDash);
            }
        }

        sendingShockwave = false;
    }

    public void Shockwave()
    {
        sendingShockwave = true;
        var newShockwave = Instantiate(ShockwavePrefab, transform);
        transform.DOScale(0.2f, 0.3f).OnComplete(() => ScaleBackUpAndShakeCam());
        newShockwave.transform.DOScale(0.2f, 0.3f).OnComplete(() => newShockwave.transform.DOScale(Vector3.one * 30, 2f).OnComplete(() => Destroy(newShockwave)));
    }

    private void ScaleBackUpAndShakeCam()
    {
        transform.DOScale(1, 0.3f);
        Camera.main.transform.DOPunchRotation(Vector3.one / 3, 0.5f);
    }

    void EndDash()
    {
        GameManager.Instance.UpdateIdleScore();

        if (transform.position.x >= 8)
            transform.position = new Vector3(8, transform.position.y, 0);
        else if (transform.position.x <= -8)
            transform.position = new Vector3(-8, transform.position.y, 0);
        _spriteRend.sprite = IdleSprite;
    }
}