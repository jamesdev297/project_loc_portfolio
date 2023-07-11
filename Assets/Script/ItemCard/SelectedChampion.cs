

using System.Collections.Generic;
using UnityEngine;

public class SelectedChampion : MonoBehaviour
{
    public List<GameObject> championPreFabs;
    private SpriteRenderer sprite;
    private void Start()
    {
        foreach (var gameObject in championPreFabs)
        {
            if (ChampionModel.getIdByPrefabName(gameObject.name) == GameManager.instance.playerModel.selectChampionModel.id)
            {
                Instantiate(gameObject, transform.position, Quaternion.identity).transform.parent = transform;
                //prefabInstanceClones.Add(Instantiate(gameObject, transform.position, Quaternion.identity));
                break;
            }
        }
        
        sprite = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (sprite)
        {
            sprite.sortingOrder = 4;
        }
    }
}
