using System.Collections;
using UnityEngine;

public class SceneController : MonoBehaviour {

    [SerializeField] private MemoryCard _originalCard = null;
    [SerializeField] private Sprite[] _images = null;
    [SerializeField] private TextMesh _healthLabel = null;
    [SerializeField] private TextMesh _victoryLabel = null;
    [SerializeField] private TextMesh _overLabel = null;
    [SerializeField] private float _timeToMemorize = 2.0f;

    public float TimeToMemorize { get { return _timeToMemorize; } set { _timeToMemorize = value; } }

    public const int GridRows = 2;
    public const int GridCols = 6;
    public const float OffsetX = 2.3f;
    public const float OffsetY = 4.4f;
    
    private int _score = 0;
    private int _health = 5;
    private MemoryCard _firstRevealed = null;
    private MemoryCard _secondRevealed = null;
    
    public bool canReveal {

        get { return _secondRevealed == null; }
    }

    private IEnumerator CheckMatch()
    {
        if (_firstRevealed.Id == _secondRevealed.Id)
        {
             _score++;

            if (_score >= GridCols)
            {
                _victoryLabel.text = "Victory!";
            }
        }
        else
        {          
            _health--;  

            if(_health <= 0)
            {
                _overLabel.text = "Game Over";
                yield return new WaitForSeconds(1.2f);
                Restart();
            }
            else
            {
                _healthLabel.text = "Health: " + _health;
                yield return new WaitForSeconds(_timeToMemorize);
                _firstRevealed.IsReveal = false;
                _secondRevealed.IsReveal = false;
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

    private void Shuffle(int[] numbers)
    {
        int i, firstNumberValue, secondNumberIndex;

        for (i = 0; i < numbers.Length / 2; i++)
        {
            firstNumberValue = numbers[i];
            secondNumberIndex = Random.Range(i, numbers.Length / 2);
            numbers[i] = numbers[secondNumberIndex];
            numbers[secondNumberIndex] = firstNumberValue;
        }

        for (i = numbers.Length / 2; i < numbers.Length; i++)
        {
            firstNumberValue = numbers[i];
            secondNumberIndex = Random.Range(i, numbers.Length);
            numbers[i] = numbers[secondNumberIndex];
            numbers[secondNumberIndex] = firstNumberValue;
        }
        
    }

    void Start() {

        Vector3 startPos = _originalCard.transform.position;
      
        int[] numbers = { 0, 1, 2, 3, 4, 5, 0, 1, 2, 3, 4, 5 };
        Shuffle(numbers);

        for (int i = 0; i < GridCols; i++)
        {
            for (int j = 0; j < GridRows; j++)
            {
                MemoryCard card;

                if (i == 0 && j == 0) {

                    card = _originalCard;
                }
                else {

                    card = Instantiate(_originalCard) as MemoryCard;
                }

                int index = j * GridCols + i;               
                int id = numbers[index];
                card.SetCard(id, _images[id]);

                float posX = (OffsetX * i) + startPos.x;
                float posY = -(OffsetY * j) + startPos.y;
                card.transform.position = new Vector3(posX, posY, startPos.z);
               
            }
        }
    }

    public void Restart() {

        Application.LoadLevel(0);      
    }

}

