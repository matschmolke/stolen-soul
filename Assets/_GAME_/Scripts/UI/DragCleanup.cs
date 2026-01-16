using UnityEngine;

public class DragCleanup : MonoBehaviour
{
    public static void ClearDragImage()
    {
        var dragImage = GameObject.Find("DragImage");

        if (dragImage != null) 
        {
            Destroy(dragImage);
        }
    }
}
