using UnityEngine;

public class VidaEnemigo : MonoBehaviour
{
    public int Vida = 100;
    public int VidaActual;
    public float contador = 0f;
    private DropeoItem drop;

    private void Start()
    {
        VidaActual = Vida;
        drop = GetComponent<DropeoItem>();
    }

    public void DanoRecibido(int dano)
    {
        VidaActual -= dano;
        Debug.Log($"{gameObject.name} recibio {dano} de dano. Vida restante: {VidaActual}");

        if(VidaActual <= 0)
        {
            drop.DropearItem();
           Muere(); 
            
        }
    }

    private void Muere()
    {
        
        
       

        
            Destroy(gameObject);

        
    }
}
