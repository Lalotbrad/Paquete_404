using UnityEngine;

public class DropeoItem : MonoBehaviour
{
    public GameObject DropItem;

    public void DropearItem()
    {
        if(DropItem == null)
        {
            Debug.Log("No hay item");
            return;
        }

        Instantiate(DropItem, transform.position, Quaternion.identity);
    }
}
