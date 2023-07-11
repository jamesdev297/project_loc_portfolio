using System;
using System.Collections;
using System.Collections.Generic;
using Json;
using Script.Model;
using UnityEngine;
using UnityEngine.UI;

public class DimScript : MonoBehaviour
{
    public GameObject proficiencyBar;

    public GameObject firstAnim;
    public GameObject secondAnim;

    public GameObject rewardCardImage;

    public List<GameObject> rewardCardList;

    public List<GameObject> rewardCardImageList = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        secondAnim.SetActive(false);
        StartCoroutine(StartProficiencyGuage());
        GameManager.instance.dimObject = gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator StartProficiencyGuage()
    {
        UserChamp userChamp = UserInfoManager.Instance.userChampionMap[1];

        if(userChamp == null)
            yield break;

        List<int> maxProficiencyByLevelList = GameManager.instance.maxProficiencyByLevelList;
        int prev = userChamp.exp;
        int after = userChamp.nextExp;
        int lvUp = 0;
        while (true)
        {
            if (after >= maxProficiencyByLevelList[userChamp.level])
            {
                lvUp++;
                after -= maxProficiencyByLevelList[userChamp.level];
            }
            else
            {
                break;
            }
        }

        float duration = 1.5f;
        for (int i = 0; i < lvUp; i++)
        {
            int maxValue = maxProficiencyByLevelList[userChamp.level + i];
            StartCoroutine(AnimationGuage(proficiencyBar.transform, userChamp.exp, maxValue, maxValue,duration));
            yield return new WaitForSeconds(duration);
            proficiencyBar.transform.localScale = new Vector3(0, 1, 1);
            yield return new WaitForSeconds(0.6f);  
        }
        
        StartCoroutine(AnimationGuage(proficiencyBar.transform, userChamp.exp, after, maxProficiencyByLevelList[userChamp.level + lvUp],duration));
        yield return new WaitForSeconds(duration);

        yield return new WaitForSeconds(1.0f);
        
        firstAnim.GetComponent<Animator>().SetTrigger("hide");
        yield return new WaitForSeconds(1.5f);
        firstAnim.SetActive(false);
        secondAnim.SetActive(true);

        CardModel card = new DaggerModel();
        foreach (var rewardCard in rewardCardList)
        {
            //Debug.Log("MAKE REWARDCARD");
            GameObject temp = Instantiate(rewardCardImage, Vector3.zero, Quaternion.identity, rewardCard.transform);
            temp.transform.localPosition = Vector3.zero;
            temp.transform.localScale = new Vector3(-1, 1, 1);
            temp.GetComponent<Image>().sprite = Resources.Load<Sprite>("Image/ItemCard/" + card.GETName());
            temp.SetActive(false);
            rewardCardImageList.Add(temp);
        }

    }

    public void SelectRewardCard(GameObject gameObject)
    {
        StartCoroutine(FlipRewardCard(gameObject));
        
    }

    IEnumerator FlipRewardCard(GameObject card)
    {
        int index = -1;
        for (var i = 0; i < rewardCardList.Count; i++)
        {
            if (rewardCardList[i] == card)
            {
                index = i;
                break;
            }
        }
        var value = 0.0f;
        while (card.transform.localRotation.eulerAngles.y < 90.0f)
        {
            card.transform.Rotate(new Vector3(0, 1.0f, 0));
           yield return null;
        }
        rewardCardImageList[index].SetActive(true);
        while (card.transform.localRotation.eulerAngles.y < 180.0f)
        {
            card.transform.Rotate(new Vector3(0, 1.0f, 0));
            yield return null;
        }
        
        yield return new WaitForSeconds(1.0f);
        secondAnim.GetComponent<Animator>().SetTrigger("hide");
        yield return new WaitForSeconds(0.6f);
        secondAnim.SetActive(false);

        yield break;
    }

    IEnumerator AnimationGuage(Transform transform, int initialValue, int goalValue, int maxValue, float duration)
    {
        AnimationCurve animationCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, duration, 1.0f);

        var time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            var lerpT = animationCurve.Evaluate(time);
            
            float scaleX = (float)(initialValue + (goalValue-initialValue)*lerpT) / maxValue;
            transform.localScale = new Vector3(scaleX,1,1);
            yield return null;
        }
    }
}
