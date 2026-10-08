using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorManager : MonoBehaviour
{
    public bool real_time;
    public Color main_color;
    public RawImage fondo;
    public List<SpriteRenderer> plataformas;

    public float fondo_oscurecimiento = 0.5f;

    void Start()
    {
        update_color();
    }

    void Update()
    {
        if (real_time)
        {
            update_color();
        }
    }

    public void update_color()
    {
        foreach (SpriteRenderer plataforma in plataformas)
        {
            plataforma.color = main_color;
        }

        Color.RGBToHSV(main_color, out float h, out float s, out float v);

        v *= fondo_oscurecimiento;

        fondo.color = Color.HSVToRGB(h, s, v);
    }
}