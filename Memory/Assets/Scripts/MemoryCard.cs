using System.Collections;
using UnityEngine;

public class MemoryCard : MonoBehaviour {

    [SerializeField] private SceneController _controller = null;
    [SerializeField] private SpriteRenderer _spriteRend = null;

    public int Id { get; private set; }

    public int Group { get; set; } = default;

    private bool _isReveal = true;

    private float _deltaAngle = 0.0f;
    private float _targetRotation = 180.0f;
    private float _rotationSpeed = 250.0f;

    private bool _isRotate = false;

    public bool IsRevealed
    {
        get {

            return _isReveal;
        }

        set
        {
            if ((IsRevealed == true && value == false) || (IsRevealed == false && value == true))      
            {
                //StartCoroutine(RotateCard());

                _isRotate = true;
            }        

            _isReveal = value;
        }
    }

    public IEnumerator StartAction()
    {
        IsRevealed = true;
        yield return _controller.TimeToMemorize;
        IsRevealed = false;
    }

    public void SetCard(int id, Sprite image)
    {
        Id = id;
        _spriteRend.sprite = image;
    }

    public void OnMouseDown(){

        if (IsRevealed == false && _controller.CanReveal)
        { 
            _controller.CardReveal(this, delegate() { IsRevealed = true; });
        }
    }

    void Start()
    {
        StartCoroutine(StartAction());
    }

    //private IEnumerator RotateCard()
    //{
    //    float deltaAngle = 0.0f;
    //    const float RotationSpeed = 1.5f;
    //    WaitForSeconds waitingTime = new WaitForSeconds(0.001f);

    //    while (deltaAngle <= 180.0f)
    //    {
    //        deltaAngle += RotationSpeed;
    //        transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime, Space.Self);
    //        yield return waitingTime;
    //    }

    //    yield return null;
    //}

    private void Update()
    {       
        if (_isRotate) {

            if (_deltaAngle < _targetRotation)
            {
                _deltaAngle += _rotationSpeed * Time.deltaTime;
                transform.Rotate(Vector3.up, _rotationSpeed * Time.deltaTime, Space.Self);
            }
            else
            {
                _isRotate = false;
                _deltaAngle = 0.0f;
            }
        }
    }

}
