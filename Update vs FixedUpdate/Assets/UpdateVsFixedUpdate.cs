using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateVsFixedUpdate : MonoBehaviour
{
    public int maxFps = 60;
    int updateCalls = 0;
    int fixedUpdateCalls = 0;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = maxFps;
    }

    // Update is called once per frame
    void Update()
    {
        // Llamada una vez por Frame
        // 60 FPS = llamada 60 veces por segundo
        fixedUpdateCalls = 0;
        updateCalls++;
        Debug.Log($"<color=red><b>Update: { updateCalls} llamada/s</b></color>");
    }

    private void FixedUpdate()
    {
        // Se llama de forma constante con independecia de los FPS
        // Por defecto se llama cada 0.02 segundos
        // 50 veces por segundo
        updateCalls = 0;
        fixedUpdateCalls++;
        Debug.Log($"<color=blue><b>FixedUpdate: {fixedUpdateCalls} llamada/s</b></color>");
    }

}
