using System.Collections;
using UnityEngine;

public class Recyclable : MonoBehaviour
{
    private static Vector3 recyclePosition = Vector3.one * -100;

    public int id = -1;
    public float lifeSpan = 0;

    protected virtual void OnEnable()
    {
        if (lifeSpan > 0)
            StartCoroutine(RecycleAfter(lifeSpan));
    }

    protected void Die()
    {
        StopAllCoroutines();
        transform.position = recyclePosition;
        gameObject.SetActive(false);
        ObjectRecycler.Singleton.Recycle(this);
    }

    private IEnumerator RecycleAfter(float t)
    {
        yield return new WaitForSeconds(t);

        Die();
    }
}
