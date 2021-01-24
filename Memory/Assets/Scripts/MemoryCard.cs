using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryCard : MonoBehaviour {

    [SerializeField] private GameObject cardBack;
    [SerializeField] private SceneController controller;

    void Start()
    {
        StartCoroutine(StartAction());
    }

    public void OnMouseDown(){
     
        if (cardBack.activeSelf && controller.canReveal)
        {
            cardBack.SetActive(false);
            controller.CardRevealed(this);
        }
    }

    public void Unreveal() {

        cardBack.SetActive(true);
    }

    private int _id;

    public int id {

        get { return _id; }
    }

    public void SetCard(int id, Sprite image) {

        _id = id;
        GetComponent<SpriteRenderer>().sprite = image;
    }


    public IEnumerator StartAction()
    {
        cardBack.SetActive(false);
        yield return new WaitForSeconds(2.5f);
        cardBack.SetActive(true);      
    }
    
}
