using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour {
    GameManager game;
    public delegate void playerDelegate();
    public static event playerDelegate OnPlayerDied;
    public static event playerDelegate OnPlayerScored;

	public float tapForce=10;
	public float tiltSmooth = 5;
	public Vector3 startPos;

	Rigidbody2D rigidbody;
	Quaternion downRotation;
    Quaternion forwardRotation;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        downRotation = Quaternion.Euler(0, 0, -90);
        forwardRotation = Quaternion.Euler(0, 0, 35);
        game = GameManager.Instance;
	}
    void OnEnable()
    {
        GameManager.OnGameOverConfirmed += OnGameOverConfirmed;
        GameManager.OnGameStarted += OnGameStarted;
    }
    void OnDisable()
    {
        GameManager.OnGameStarted -= OnGameStarted;
        GameManager.OnGameOverConfirmed -= OnGameOverConfirmed;
    }
    void OnGameOverConfirmed()
    {
        transform.localPosition = startPos;
        transform.rotation = Quaternion.identity;
    }
    void OnGameStarted()
    {
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;//turns physics back on
    }
	
	// Update is called once per frame
	void Update () {
        if (game.GameOver) return;

        if (Input.GetMouseButtonDown(0))
        {
            transform.rotation=forwardRotation;
            //before adding force we should zero out the velocity
            rigidbody.velocity = Vector3.zero;
            rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force);
        }

        transform.rotation = Quaternion.Lerp(transform.rotation,downRotation,tiltSmooth*Time.deltaTime);
	}
    void OnTriggerEnter2D(Collider2D col)
    {
            if (col.gameObject.tag == "ScoreZone")
        {
            //register score event
            OnPlayerScored(); //event listened to by GameManager
            //play sound
        }
        if (col.gameObject.tag == "DeadZone")
        {
            rigidbody.simulated = false;
            //register dead event
            OnPlayerDied(); //event sent to GameManager
            //play sound
        }
    }
}
