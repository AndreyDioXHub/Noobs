using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platforms : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> _platforms = new List<GameObject>();
    [SerializeField]
    private Animator _animator;

    void Start()
    {
        if (transform.position.y > 250 && transform.position.y < 500)
        {
            int r = Random.Range(1, 5);

            if (r < 3)
            {
                _animator.SetTrigger("t1");
            }
            return;
        }

        if (transform.position.y > 500)
        {
            int r = Random.Range(1,5);

            if (r < 3)
            {
                _animator.SetTrigger($"t{r}");
            }
            return;
        }
    }

    void Update()
    {
    }

    internal void Init(int index)
    {
        _platforms[index].SetActive(true);
    }

    public void DestroyPlatform()
    {
        StartCoroutine(DestroyPlatformCoroutine());
    }

    IEnumerator DestroyPlatformCoroutine()
    {
        _animator.SetTrigger("d");
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
