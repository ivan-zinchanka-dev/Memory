using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RestartButton : MonoBehaviour {

    [SerializeField] private SceneController _controller = null;
    [SerializeField] private Color _highlightColor = Color.yellow;
    [SerializeField] private SpriteRenderer _sprite = null;

    private Vector2 _normalScale = default;
    private Vector2 _pressedScale = new Vector2(0.6f, 0.6f);

    private void Start()
    {
        _normalScale = transform.localScale;
    }

    public void OnMouseOver() {

        if (_sprite != null) {

            _sprite.color = _highlightColor;
        }
    }
    public void OnMouseExit() {

        if (_sprite != null) {

            _sprite.color = Color.white;
        }
    }
    public void OnMouseDown() {
 
        transform.localScale = _pressedScale;
    }
    public void OnMouseUp() {

        transform.localScale = _normalScale;
        _controller.Restart();
    }
}
