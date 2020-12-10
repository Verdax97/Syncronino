using UnityEngine;

public class MoveElementController : MonoBehaviour
{
    public void MoveUp()
    {
        if (transform.GetSiblingIndex() > 0)
            transform.SetSiblingIndex(transform.GetSiblingIndex() - 1);
    }
    public void MoveDown()
    {
         if (transform.GetSiblingIndex() < transform.parent.childCount - 1)
            transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
    }
    public void Delete()
    {
        Destroy(gameObject);
    }
}
