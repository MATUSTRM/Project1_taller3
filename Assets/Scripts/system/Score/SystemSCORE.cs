using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class SystemSCORE : MonoBehaviour
{
    public int score = 0;
    public int pointsPerEvent = 100;
    public int pointsPerFlag = 300;

    public static int comboMultiplier = 1;
    public int maxComboMultiplier = 5;

    public float comboTimeLimit = 5f;

    [Header("UI COMBO")]
    public Image comboTimerImage;
    public TextMeshProUGUI comboText;

    private float comboTimer = 0f;
    private bool isComboActive = false;

    [Header("EVENTOS")]
    public UnityEvent onScoreChange;
    public UnityEvent double_Combo;
    public UnityEvent Triple_Combo;
    public UnityEvent quadra_Combo;
    public UnityEvent quintu_Combo;
    public UnityEvent end_Combo;

    internal Action<int> onAddPoint;

    // Sumar puntos y gestionar combo
    public void AddPoints()
    {
        // Añadir puntos base
        score += pointsPerEvent * comboMultiplier;

        // Comprobar si se mantiene el combo
        if (comboTimer > 0)
        {
            comboMultiplier = Mathf.Min(
                comboMultiplier + 1,
                maxComboMultiplier
            );

            // Actualizar texto
            ActualizarComboText();

            // Eventos dependiendo del combo
            if (comboMultiplier == 2)
            {
                double_Combo.Invoke();
            }
            else if (comboMultiplier == 3)
            {
                Triple_Combo.Invoke();
            }
            else if (comboMultiplier == 4)
            {
                quadra_Combo.Invoke();
            }
            else if (comboMultiplier == 5)
            {
                quintu_Combo.Invoke();
            }
        }
        else
        {
            // Reiniciar combo
            comboMultiplier = 1;

            ActualizarComboText();
        }

        // Reiniciar temporizador
        comboTimer = comboTimeLimit;
        isComboActive = true;

        // Evento de cambio de puntuación
        onScoreChange.Invoke();
    }

    // Puntos por recoger una bandera
    public void AddPointsForFlag()
    {
        score += pointsPerFlag;

        onScoreChange.Invoke();
    }

    private void Update()
    {
        // Actualizar temporizador de combo
        if (comboTimer > 0)
        {
            comboTimer -= Time.deltaTime;

            // Actualizar barra de combo
            if (comboTimerImage != null)
            {
                comboTimerImage.fillAmount =
                    comboTimer / comboTimeLimit;
            }
        }
        else if (isComboActive)
        {
            // El combo terminó
            end_Combo.Invoke();

            isComboActive = false;

            // Reiniciar multiplicador
            comboMultiplier = 1;

            // Quitar texto
            ActualizarComboText();
        }
    }

    // Cambiar directamente el combo
    public void Combo_plus(int number_combo)
    {
        comboMultiplier = number_combo;

        comboTimer = comboTimeLimit + 10;

        maxComboMultiplier = number_combo;

        isComboActive = true;

        // Actualizar texto
        ActualizarComboText();
    }

    // Actualizar texto del combo
    public void ActualizarComboText()
    {
        if (comboText == null)
            return;

        switch (comboMultiplier)
        {
            case 1:
                comboText.text = "";
                break;

            case 2:
                comboText.text = "DOUBLE COMBO";
                break;

            case 3:
                comboText.text = "TRIPLE COMBO";
                break;

            case 4:
                comboText.text = "HYPER COMBO";
                break;

            case 5:
                comboText.text = "ULTRA COMBO";
                break;

            default:
                comboText.text = "x" + comboMultiplier + " COMBO";
                break;
        }
    }

    // Actualizar puntaje con TextMesh
    public void actualizar_puntaje_entext(TextMesh textmesh)
    {
        textmesh.text = score.ToString();
    }

    // Actualizar puntaje con TextMeshPro
    public void actualizar_puntaje_entextPRO(TextMeshProUGUI textmesh)
    {
        textmesh.text = score.ToString();
    }
}