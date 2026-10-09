using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Collision2DEvent : UnityEvent<Collision2D> { }

public class PlayerHealth : MonoBehaviour
{
    public VariableInt playerHealth;
    public UnityEvent AL_PERDER_VIDA;
    public Collision2DEvent FueTocado_Collider;
    public UnityEvent CERO_VIDAS_Collider;
    public Panel UI_Gameover;

    public AudioClip sfx_game_over;
    public AudioClip splat;

    [SerializeField]private Timer timer;
    [SerializeField]private ChangeCursor cursorManager;
    [SerializeField]private PanelManager panelManager;
    [SerializeField]private CameraShaker cameraShaker;
    [SerializeField]private Muertes_player muertes_Player;

    [SerializeField]UI_Manager ui_Manager;
    private void Start()
    {
        
    }

    public void ReducirVida()
    {
        // Reducir la vida del jugador
        playerHealth.valor--;

        if (playerHealth.valor <= 0)
        {
            CERO_VIDAS_Collider.Invoke();
            // Manejar la muerte del jugador aqu� (reiniciar nivel, mostrar pantalla de game over, etc.)
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //COLISION GLOBAL
        FueTocado_Collider.Invoke(collision);

        if (collision.collider.CompareTag("Enemy"))
        {
            AL_PERDER_VIDA.Invoke();
            //REPRODUCIMOS EL SONIDO
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }
    //------------------------------------------------------------
    //FUNCIONES

        
    public void muerto(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // Si colision� con un enemigo, reducir la vida del jugador
            playerHealth.valor--;
            transform.position = CheckPointSystem.instance.UltimaPos;
            // Actualizar la UI
            UpdateUI();

            // Verificar si el jugador ha perdido todas sus vidas
            if (playerHealth.valor <= 0)
            {
                timer.reanude_time();
                ui_Manager.ActualizarPuntaje();
                timer.PauseTimer();
                cursorManager.On_cursor_texture();
                gameObject.SetActive(false);
                panelManager.CloseAllPanels();
                UI_GAMEOVER();
                muertes_Player.aumentar_muerte();
                cameraShaker.ShakeCamerasStronger();
                CERO_VIDAS_Collider.Invoke();

                // Manejar la muerte del jugador aqu� (reiniciar nivel, mostrar pantalla de game over, etc.)
            }
        }
    }
    public void muerto_trigger(Collider2D collision)
    {
        if (collision.CompareTag("Lava"))
        {
            // Si colision� con un enemigo, reducir la vida del jugador
            playerHealth.valor--;
            transform.position = CheckPointSystem.instance.UltimaPos;
            // Actualizar la UI
            UpdateUI();

            // Verificar si el jugador ha perdido todas sus vidas
            if (playerHealth.valor <= 0)
            {
                CERO_VIDAS_Collider.Invoke();

                // Manejar la muerte del jugador aqu� (reiniciar nivel, mostrar pantalla de game over, etc.)
            }
        }
    }

    private void UpdateUI()
    {
        // Actualizar el texto de la UI con la nueva cantidad de vidas del jugador
        FindObjectOfType<UI_Manager>().UpdateHealthText();
    }

    public void UI_GAMEOVER()
    {
        UI_Gameover.gameObject.SetActive(true);
    }
}
