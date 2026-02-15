using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeansContainer : MonoBehaviour
{
    [Header("Grid Settings")]
    public int gridSizeX = 3;
    public int gridSizeY = 3;
    public float spacing = 0.33f;

    [Header("Stack Settings")]
    public int sackLayers = 3;
    public float layerOffsetY = 0.05f; // altura visual de cada capa
    public int maxSacks = 45;           // máximo de sacos visibles
    [Range(0, 45)]
    public int currentCount = 0;        // cuántos sacos mostrar

    [Header("Prefab")]
    public GameObject BeanSackPrefab;

    private List<GameObject> BeanSacksList = new List<GameObject>();

    void Start()
    {
        GenerateSacks();
        UpdateSacksVisual();
    }
    // Genera todos los sacos (desactivados inicialmente)
    public void GenerateSacks()
    {
        ClearSacks();

        float offsetX = (gridSizeX - 1) * spacing * 0.5f;
        float offsetY = (gridSizeY - 1) * spacing * 0.5f;

        for (int layer = 0; layer < sackLayers; layer++)
        {
            for (int x = 0; x < gridSizeX; x++)
            {
                for (int y = 0; y < gridSizeY; y++)
                {
                    GameObject beanSack = Instantiate(BeanSackPrefab, transform, false);

                    Vector2 localPos = new Vector2(x * spacing - offsetX,y * spacing - offsetY + layer * layerOffsetY);
                    beanSack.transform.localPosition = localPos;

                    // Sorting Order relativo al prefab
                    SpriteRenderer sr = beanSack.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        int baseOrder = sr.sortingOrder;
                        sr.sortingOrder = baseOrder + layer;
                    }

                    // Desactivar render inicialmente
                    if (sr != null)
                        sr.enabled = false;


                    BeanSacksList.Add(beanSack);

                    // Limite máximo de sacos
                    if (BeanSacksList.Count >= maxSacks)
                        return;
                }
            }
        }
    }

    // Actualiza la visibilidad según el contador
    public void UpdateSacksVisual()
    {
        for (int i = 0; i < BeanSacksList.Count; i++)
        {
            SpriteRenderer sr = BeanSacksList[i].GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                sr.enabled = i < currentCount; // activa solo los primeros 'currentCount'
            }
        }
    }

    void ClearSacks()
    {
        foreach (var sack in BeanSacksList)
        {
            if (sack != null)
                Destroy(sack);
        }

        BeanSacksList.Clear();
    }
}

