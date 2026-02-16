using System.Collections;
using UnityEngine;

public class CoffeeDoor : MonoBehaviour
{
    public bool isUnlocked;
    [Header("Spawn Settings")]
    [Range(1, 10)]
    public int tier = 1; // 1 a 10
    private float spawnInterval; // calculado según tier

    private BeansContainer beansContainer;

    private Coroutine spawnCoroutine;

    void Start()
    {
        beansContainer = GetComponentInChildren<BeansContainer>();

        // Clamp del tier
        tier = Mathf.Clamp(tier, 1, 10);

        // Inicia el spawn automático
        spawnCoroutine = StartCoroutine(SpawnBeanSacksRoutine());
    }

    private IEnumerator SpawnBeanSacksRoutine()
    {
        while (true)
        {
            SpawnBeanSack();

            // recalcular spawnInterval cada vez según el tier actual
            // tier 1 = 10 s, tier 10 = 0.1 s
            float interval = Mathf.Lerp(10f, 0.1f, (tier - 1) / 9f);

            yield return new WaitForSeconds(interval);
        }
    }

    private void SpawnBeanSack()
    {
        if (beansContainer == null)
            return;

        // Incrementa el contador solo si no excede el máximo
        if (beansContainer.currentCount < beansContainer.maxSacks)
        {
            beansContainer.currentCount++;
            beansContainer.UpdateSacksVisual();
        }
    }

    public void StopSpawning()
    {
        if (spawnCoroutine != null)
            StopCoroutine(spawnCoroutine);
    }
    public void ResumeSpawning()
    {
        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnBeanSacksRoutine());
    }

}
