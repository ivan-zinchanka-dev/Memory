using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour {

    public const int gridRows = 2;
    public const int gridCols = 6;
    public const float offsetX = 2.3f;
    public const float offsetY = 4.4f;
    private int _score = 0;
    private int _health = 5;

    [SerializeField] private MemoryCard originalCard; 
    [SerializeField] private Sprite[] images;
    [SerializeField] private TextMesh HealthLabel;
    [SerializeField] private TextMesh VictLabel;
    [SerializeField] private TextMesh OverLabel;
    
    private MemoryCard _firstRevealed;
    private MemoryCard _secondRevealed;
    public bool canReveal {

        get { return _secondRevealed == null; }

    }

    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.id == _secondRevealed.id)
        {
             _score++;

            if (_score >= 6)
            {
                VictLabel.text = "Victory!";
            }
        }
        else
        {          
            _health--;  

            if(_health <= 0)
            {
                OverLabel.text = "Game Over";
                yield return new WaitForSeconds(1.2f);
                Restart();
            }
            else
            {
                HealthLabel.text = "Health: " + _health;
                yield return new WaitForSeconds(0.8f);
                _firstRevealed.Unreveal();
                _secondRevealed.Unreveal();
            }        
        }

        _firstRevealed = null;
        _secondRevealed = null;

    }

    public void CardRevealed(MemoryCard card)
    {
        if (_firstRevealed == null) {

            _firstRevealed = card;
        }
        else {

            _secondRevealed = card;          
            StartCoroutine(CheckMatch());
        }

    }

    private int[] ShuffleArray(int[] numbers)
    {       
        int i, tmp, r;
        int[] newArray = numbers.Clone() as int[];

        for (i = 0; i < newArray.Length/2; i++)
        {
            tmp = newArray[i];
            r = Random.Range(i, newArray.Length/2);
            newArray[i] = newArray[r];
            newArray[r] = tmp;          
        }

        for (i = newArray.Length / 2; i < newArray.Length; i++)
        {
            tmp = newArray[i];
            r = Random.Range(i, newArray.Length);
            newArray[i] = newArray[r];
            newArray[r] = tmp;
        }

        return newArray;
    }


    void Start() {

        Vector3 startPos = originalCard.transform.position;

        int i, j;
          
        int[] numbers = { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5 };

        numbers = ShuffleArray(numbers);

        
        for (i = 0; i < gridCols; i++)
        {
            for (j = 0; j < gridRows; j++)
            {
                MemoryCard card;

                if (i == 0 && j == 0) {

                    card = originalCard;
                }
                else {

                    card = Instantiate(originalCard) as MemoryCard;
                }

                int index = j * gridCols + i;               
                int id = numbers[index];
                card.SetCard(id, images[id]);

                float posX = (offsetX * i) + startPos.x;
                float posY = -(offsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
               
            }
        }
    }

    public void Restart() {

        Application.LoadLevel("SampleScene");
       
    }

}

