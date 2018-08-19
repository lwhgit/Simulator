using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class Script : MonoBehaviour {

    public Text velView;
    public Text accView;
    public Text timeView;
    public Text alertView;

    public GameObject cube;

    private Rigidbody rb;
    private float time = 0;
    private float limitTime = 15;

    private float velocity = 0;
    private float oldVelocity = 0;

    private float k = 0;
    private float dir = 1;

    private bool run = false;

    private List<Data> dataList;

    private string data = "0,\t\t0";

    private void Awake() {
        dataList = new List<Data>();
        k = 1f / 2f * Config.C * Config.rho * Config.A;
        //Time.fixedDeltaTime = 0.001f;
        //Time.timeScale = 0.1f;
    }

    void Start () {
        rb = gameObject.GetComponent<Rigidbody>();
    }
	
    void Update () {
        
        if (!run && Input.GetKeyDown(KeyCode.S)) {
            run = true;
            cube.transform.position += new Vector3(3, 0, 0);
        }
        if (!run && Input.GetKeyDown(KeyCode.D)) {
            run = true;
            rb.AddForce(new Vector3(0, 23.73787f, 0), ForceMode.VelocityChange);
            cube.transform.position += new Vector3(3, 0, 0);
        }
        if (Time.timeScale == 0 && Input.GetKeyDown(KeyCode.W)) {
            WriteData();
            alertView.text = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\rawdata.dat 에 저장됨";
        }
    }

    private void FixedUpdate() {
        velocity = rb.velocity.y;

        if (run) {
            time += Time.deltaTime;

            dataList.Add(new Data(time, velocity));

            if (time >= limitTime) {
                Time.timeScale = 0;
            }
        }
        
        velView.text = "Velocity : " + velocity;
        accView.text = "Acceleration : " + (velocity - oldVelocity) / Time.deltaTime; // 가속도 측정
        timeView.text = "Time(sec) : " + time;

        rb.AddForce(new Vector3(0, -9.81f, 0), ForceMode.Acceleration); // 중력
        dir = velocity > 0 ? -1 : 1; // 떨어지는 방향 측정

        rb.AddForce(0, dir * k * velocity * velocity / Config.m, 0, ForceMode.Force); // 공기저항

        oldVelocity = velocity;
    }

    private void WriteData() {
        string path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory) + "\\rawdata.dat";
        Data d = null;
        for (int i = 0;i < dataList.Count;i ++) {
            d = dataList[i];
            data += "\n" + d.time + ",\t\t" + d.velocity;
        }
        File.WriteAllText(path, data);
    }

    class Data {
        public float time = 0;
        public float velocity = 0;

        public Data(float time, float velocity) {
            this.time = time;
            this.velocity = velocity;
        }
    }
}
