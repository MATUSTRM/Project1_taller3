using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [SerializeField] private TMP_Text timertext;
    [SerializeField, Tooltip("Tiempo en segundos")] private float segundos;
    private int minutes, seconds, cents;
    [Header("EVENTOS")]
    public UnityEvent QUEDAN_10_SEGUNDOS;
    public UnityEvent QUEDAN_5_SEGUNDOS;
    public UnityEvent TIEMPO_NORMAL;
    public UnityEvent EndTimer;
    public UnityEvent TiempoRestante;
    public int contador_restante;
    private int contador_restante_max;
    public GameObject ui_finjuego;
    public float tiempoTranscurrido = 0; // Nuevo

    private bool isPaused = false;
    private bool isRunning = false;
    private bool hasExecutedEndEvent = false; // Bandera para indicar si el evento de finalizaci�n del tiempo ya se ha ejecutado

    public static event System.Action<int> OnTiempoAgregado; // Evento est�tico que se dispara cuando se agrega tiempo
    void Start()
    {
        MostrarTiempoAsignado();
        contador_restante_max = contador_restante;
    }

    void Update()
    {
        if (isRunning && !isPaused)
        {
            segundos -= Time.deltaTime;
            tiempoTranscurrido += Time.deltaTime; // Actualiza tiempo transcurrido

            if (segundos < 0) segundos = 0;
            {
                minutes = (int)(segundos / 60f);
                seconds = (int)(segundos - minutes * 60f);
                cents = (int)((segundos - (int)segundos) * 100f);

                timertext.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);

                if (segundos == 0 && !hasExecutedEndEvent)
                {
                    EndTimer.Invoke();
                    contador_restante = contador_restante_max;
                    hasExecutedEndEvent = true; // Marca que el evento de finalizaci�n se ha ejecutado
                }
            }

            if (segundos <= 10 && segundos > 5)
            {
                QUEDAN_10_SEGUNDOS.Invoke();
            }
            else if (segundos <= 5 && segundos > 0)
            {
                QUEDAN_5_SEGUNDOS.Invoke();
            }
            else
            {
                TIEMPO_NORMAL.Invoke();
            }
            if (segundos < contador_restante)
            {
                TiempoRestante.Invoke();
                contador_restante--;
            }
        }
    }

    public void PauseTimer()
    {
        isPaused = true;
    }

    public void StartTimer()
    {
        isRunning = true;
        isPaused = false;
    }

    public void active_ui_endgame()
    {
        ui_finjuego.SetActive(true);
        Time.timeScale = 0;
    }

    public void reanude_time()
    {
        Time.timeScale = 1;
    }

    public void MostrarTiempoAsignado()
    {
        // Mostrar el tiempo inicial en timertext
        minutes = (int)(segundos / 60f);
        seconds = (int)(segundos - minutes * 60f);
        cents = (int)((segundos - (int)segundos) * 100f);

        timertext.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, cents);
    }

    public void MasTiempo(int tiempoAgregado)
    {
        segundos += tiempoAgregado;
        MostrarTiempoAsignado();

        // Dispara el evento de tiempo agregado con el valor de tiempoAgregado
        OnTiempoAgregado?.Invoke(tiempoAgregado);
    }

    // M�todo para obtener el tiempo transcurrido
    public float GetTiempoTranscurrido()
    {
        return tiempoTranscurrido;
    }

}
