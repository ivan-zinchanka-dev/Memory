using System.Collections;
using UnityEngine;

public class MemoryCard : MonoBehaviour {

    [SerializeField] private GameObject _cardBack;
    [SerializeField] private SceneController _controller;
    [SerializeField] private SpriteRenderer _spriteRend = null;

    public int Id { get; private set; }

    private bool _isReveal = true;

    public bool IsReveal
    {
        get {

            return _isReveal;
        }

        set
        {
            if ((IsReveal && value == false) || (!IsReveal && value == true))
            {
                StartCoroutine(RotateCard());
            }        

            _isReveal = value;
        }
    }

    public IEnumerator StartAction()
    {
        IsReveal = true;
        yield return new WaitForSeconds(_controller.TimeToMemorize);
        IsReveal = false;
    }

    public void SetCard(int id, Sprite image)
    {
        Id = id;
        _spriteRend.sprite = image;
    }

    public void OnMouseDown(){
     
        if (IsReveal == false && _controller.canReveal)
        {
            IsReveal = true;
            _controller.CardRevealed(this);
        }
    }

    void Start()
    {
        StartCoroutine(StartAction());
    }

    private IEnumerator RotateCard()
    {
        float deltaAngle = 0.0f;
        const float RotationSpeed = 2.0f;

        while (deltaAngle <= 180.0f)
        {
            deltaAngle += RotationSpeed;
            transform.Rotate(Vector3.up, RotationSpeed, Space.Self);
            yield return new WaitForSeconds(0.001f);
        }

        yield return null;
    }

}
