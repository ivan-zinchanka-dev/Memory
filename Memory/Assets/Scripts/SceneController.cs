using System.Collections;
using UnityEngine;

public class SceneController : MonoBehaviour {

    [SerializeField] private MemoryCard _originalCard = null;
    [SerializeField] private Sprite[] _images = null;
    [SerializeField] private TextMesh _healthLabel = null;
    [SerializeField] private TextMesh _victoryLabel = null;
    [SerializeField] private TextMesh _overLabel = null;
    [SerializeField] private float _timeToMemorize = 1.5f;
    
    public WaitForSeconds TimeToMemorize { get; set; }

    public const int GridRows = 2;
    public const int GridCols = 6;
    public const float OffsetX = 2.3f;
    public const float OffsetY = 4.4f;
    
    private int _score = 0;
    private int _health = 5;
    private MemoryCard _firstRevealed = null;
    private MemoryCard _secondRevealed = null;

    public bool CanReveal {

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
            }
            else
            {
                _healthLabel.text = "Health: " + _health;
                yield return TimeToMemorize;
                _firstRevealed.IsRevealed = false;
                _secondRevealed.IsRevealed = false;
            }        
        }

        _firstRevealed = null;
        _secondRevealed = null;

    }

    public void CardReveal(MemoryCard card, System.Action callback = null)
    {
        if (_firstRevealed == null) {

            _firstRevealed = card;
            callback?.Invoke();
        }
        else {              

            if (_firstRevealed.Group != card.Group)
            {
                _secondRevealed = card;
                callback?.Invoke();
                StartCoroutine(CheckMatch());
            }         
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

    private void Awake()
    {
        TimeToMemorize = new WaitForSeconds(_timeToMemorize);
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

                card.Group = j;
            }
        }
    }

    public void Restart() {

        Application.LoadLevel(0);      
    }

}

