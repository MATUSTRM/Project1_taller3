using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ControlesPlayer : MonoBehaviour
{

    public UnityEvent onShoot; // Evento que se dispara al realizar un disparo
    //Public
    public bool puedeDisparar, puedeMoverse, puedeSaltar;
    public bool isOnPlatform;
    public Rigidbody2D platformRB;
    public KeyCode botonDisparo, botonSalto;
 
    public LayerMask layerPiso;

    public DatosSalto datosSalto;
    public Animator anim;

    public float velocidadMovimiento;
    public AudioClip sonidoSalto, sonidoAterrizaje;
    public LibreriaDeSonidos sonidosPasos;
    //Private

    Rigidbody2D rb2d;
    Disparar disparar;
    float horizontal;
    float gravedad;
    public float tiempoEntrePasos;
    float tiempoUltimoPaso;

    bool grounded;


    Collider2D col2D;
    PrevenirDispararPiso prevenirDispararPiso;

    bool checkCayendo;
    bool saltando;

    // M�todos para acceder al script ControlesPlayer desde cualquier lugar del juego
    private static ControlesPlayer _instance;
    public static ControlesPlayer Instance { get { return _instance; } }
    private void Awake()
    {

        rb2d = GetComponent<Rigidbody2D>();
        disparar = GetComponent<Disparar>();
        gravedad = Physics2D.gravity.y;
        prevenirDispararPiso = GetComponentInChildren<PrevenirDispararPiso>();
        CheckPointSystem.instance.ActualizarUltimaPos(transform.position);

    }

    private void Update()
    {
        Saltar();
        Moverse();
        Disparar();
        DatosAnimator();
    }
    //DISPARO RAPIDO INPUT
    public bool GetShootInput()
    {
        return Input.GetKey(botonDisparo);
    }
    void DatosAnimator()
    {
        anim.SetBool("ground", grounded);
        anim.SetFloat("velocidadX", (rb2d.linearVelocity.x != 0)&&(horizontal !=0) ? 1 : 0);
    }

    void Saltar()
    {
        if (!puedeSaltar) return;


        if(grounded && Input.GetKeyDown(botonSalto))
        {
            rb2d.linearVelocity = new Vector2(rb2d.linearVelocity.x,  datosSalto.velocidadSalto);
            SoundFXManager.instance.ReproducirSFX(sonidoSalto);
            StartCoroutine(CheckAterrizaje());
            saltando = true;
        }


        if (rb2d.linearVelocity.y < 0)
            rb2d.linearVelocity += new Vector2(0,  gravedad * (datosSalto.multiplicadorCaida - 1) * Time.deltaTime);
        else if (rb2d.linearVelocity.y > 0 && !Input.GetKey(botonSalto))
            rb2d.linearVelocity += new Vector2(0, gravedad * (datosSalto.multiplicadorSaltoBajo - 1) * Time.deltaTime);



    }

    void Disparar()
    {
        if (!puedeDisparar) return;

        if (Input.GetKeyDown(botonDisparo) && disparar.t > disparar.tiempoCadencia && !prevenirDispararPiso.ArmaEnPiso)
        {
            disparar.Shoot();
            anim.Play(AnimacionesPlayer.disparar,1,0);
            rb2d.AddForce(-transform.right * 10, ForceMode2D.Impulse);

            // Disparar el evento de disparo
            onShoot.Invoke();
        }
    }

    void Moverse()
    {
        if (!puedeMoverse) return;

        if(!saltando && !grounded && !checkCayendo)
        {
            checkCayendo = true;
            StartCoroutine(CheckAterrizaje());
        }

        horizontal = Input.GetAxis("Horizontal") * velocidadMovimiento ;
        SonidosPaso();
    }

    private void FixedUpdate()
    {

        rb2d.linearVelocity = new Vector2(horizontal , rb2d.linearVelocity.y) ;
        if (isOnPlatform)
        {

        }
        else
        {

        }
    }

    void SonidosPaso()
    {
        if(Time.time > tiempoUltimoPaso + tiempoEntrePasos && horizontal != 0 && grounded)
        {
            SoundFXManager.instance.ReproducirSFX(sonidosPasos);
            tiempoUltimoPaso = Time.time;
        }
    }


    ///////////////////////////////////////

    public void HaySuelo(bool state)
    {
        grounded = state;
    }
    public void activar_Salto()
    {
        puedeSaltar = true;
    }
    public void desactivar_salto()
    {
        puedeSaltar = false;
    }
    public void activar_disparo()
    {
        puedeDisparar = true;
    }

    public void desactivar_disparo()
    {
        puedeDisparar = false;
    }

    public void activar_moverse()
    {
        puedeMoverse = true;
    }
    public void desactivar_mover()
    {
        puedeMoverse = false;

    }
   public void Detener()
    {
        horizontal = 0f;
    }


    IEnumerator CheckAterrizaje()
    {
        yield return new WaitForSeconds(0.1f);

        while(!grounded)
        {
            yield return null;
        }

        SoundFXManager.instance.ReproducirSFX(sonidoAterrizaje);
        anim.Play(AnimacionesPlayer.aterrizar);
        saltando = false;
        checkCayendo = false;
    }

}
