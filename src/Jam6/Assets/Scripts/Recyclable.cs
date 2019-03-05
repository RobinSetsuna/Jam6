using System.Collections;
using UnityEngine;

public class Recyclable : MonoBehaviour
{
    public int id = -1;
    public float lifeSpan = 0;

    protected virtual void OnEnable()
    {
        if (lifeSpan > 0)
            StartCoroutine(RecycleAfter(lifeSpan));
    }

    public void Die()
    {
        StopAllCoroutines();
        gameObject.SetActive(false);
        ObjectRecycler.Singleton.Recycle(this);
    }

    private IEnumerator RecycleAfter(float t)
    {
        yield return new WaitForSeconds(t);
        Die();
    }
}
