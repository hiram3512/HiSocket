using System.Collections;
using UnityEngine;

public class TestPing : MonoBehaviour
{
    public int PingTime;
    private Ping p;
    private float timeOut = 1;
    private float lastTime;
    void Start()
    {
        StartCoroutine(Ping());
    }
    IEnumerator Ping()
    {
        p = new Ping("127.0.0.1");
        lastTime = Time.realtimeSinceStartup;
        while (!p.isDone && Time.realtimeSinceStartup - lastTime < 1)
        {
            yield return null;
        }
        PingTime = p.time;
        p.DestroyPing();
        yield return new WaitForSeconds(1);
        StartCoroutine(Ping());
    }
}
