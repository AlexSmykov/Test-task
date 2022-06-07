using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine.UI;

public class BezierMove : MonoBehaviour
{
    [SerializeField] private GameObject dotObject;
    [SerializeField] private Material linkMaterial;
    [SerializeField] private Material bezieMaterial;

    [SerializeField] private List<Vector3> dots = new List<Vector3>();

    private string source;
    private int loop;
    private bool random;
    private float time;

    private bool started;
    private float currentTime;

    [SerializeField] private float distance;

    [Obsolete]
    void Start()
    {
        if (source == null)
        {
            source = PlayerPrefs.GetString("source", "https://drive.google.com/drive/my-drive/Test.txt");
        }
        distance = 0;
        currentTime = 0;
        ParseFile();
        CreateDots();
        CalculateDistance();
        DrawDotPath();
        DrawBeziePath();
    }

    void Update()
    {
        if (started)
        {
            if (currentTime < time)
            {
                currentTime += Time.deltaTime;
                transform.position = Bezie.GetCords(dots, currentTime / time);
            }
            else if (loop == 1)
            {
                currentTime = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.position = dots[0];
            currentTime = 0;
            started = true;
        }
    }

    [Obsolete]
    private void ParseFile()
    {
        WebProxy wp = WebProxy.GetDefaultProxy();
        wp.BypassProxyOnLocal = true;
        WebRequest wr = WebRequest.Create(source);
        wr.Proxy = wp;
        Stream stream;
        stream = wr.GetResponse().GetResponseStream();
        StreamReader sr = new StreamReader(stream);

        string sLine = ""; 
        List<string> strings = new List<string>();
        while (sLine != null)
        {
            sLine = sr.ReadLine();
            if (sLine != null)
            {
                strings.Add(sLine);
            }
        }

        if (strings.Count < 4)
        {
            Debug.LogError("File data is not correct");
            Debug.Break();
            return;
        }

        loop = int.Parse(strings[0].Split(':')[1]);
        random = bool.Parse(strings[1].Split(':')[1]);
        time = float.Parse(strings[2].Split(':')[1]);

        if (random)
        {
            dots = CrosslessPath.CreatePath(int.Parse(strings[3]));
            dots.Insert(0, transform.position);
        }
        else
        {
            dots.Add(transform.position);

            for (int i = 3; i < strings.Count; i++)
            {
                dots.Add(
                    new Vector3(
                        float.Parse(strings[i].Split(',')[0]),
                        float.Parse(strings[i].Split(',')[1]),
                        float.Parse(strings[i].Split(',')[2])));
            }
        }

        if (loop == 1)
        {
            dots.Add(transform.position);
        }
    }

    private void CreateDots()
    {
        foreach (Vector3 dot in dots)
        {
            Instantiate(dotObject, dot, Quaternion.identity);
        }
    }

    private void CalculateDistance()
    {
        for (int i = 1; i < dots.Count; i++)
        {
            Vector3 dot = dots[i];
            Vector3 prevDot = dots[i - 1];
            distance += Mathf.Sqrt(Mathf.Pow(dot.x - prevDot.x, 2) + Mathf.Pow(dot.y - prevDot.y, 2) + Mathf.Pow(dot.z - prevDot.z, 2));
        }
    }

    private void DrawDotPath()
    {
        for (int i = 1; i < dots.Count; i++)
        {
            DrawLine(dots[i - 1], dots[i], linkMaterial);
        }
    }

    private void DrawBeziePath()
    {
        int segmentCount = (int)distance;
        Vector3 prevDot = dots[0];

        for(int i = 1; i <= segmentCount; i++)
        {
            Vector3 point = Bezie.GetCords(dots, (float)i / segmentCount);
            DrawLine(prevDot, point, bezieMaterial);
            prevDot = point;
        }
    }

    private void DrawLine(Vector3 start, Vector3 end, Material lineMaterial)
    {
        GameObject line = new GameObject();
        line.transform.position = start;
        line.AddComponent<LineRenderer>();
        LineRenderer lr = line.GetComponent<LineRenderer>();

        lr.material = lineMaterial;
        lr.startColor = Color.white;
        lr.startWidth = 0.1f;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
    }
}
