using UnityEngine;

public class CameraControler : MonoBehaviour
{
    public float speed = 20f;
    public float borderTHIICness = 1f;
    public Vector2 borderLimit;
    [SerializeField]Camera _myCam;
    Transform _transform;

    private void Awake()
    {
        _myCam = _myCam ? _myCam : Camera.main;
        _transform = _myCam.transform;
    }
    private void Update()
    {
        Vector3 pos = transform.position;
        if(Input.GetKey(KeyCode.W) || Input.mousePosition.y >= Screen.height - borderTHIICness)
        {
            pos.y += Time.deltaTime* speed;
            Commands.ClearDropDownMenus();
        }
        if (Input.GetKey(KeyCode.A) || Input.mousePosition.x <= borderTHIICness)
        {
            pos.x -= Time.deltaTime*speed;
            Commands.ClearDropDownMenus();
        }
        if (Input.GetKey(KeyCode.D) || Input.mousePosition.x >= Screen.width- borderTHIICness)
        {
            pos.x += Time.deltaTime*speed;
            Commands.ClearDropDownMenus();
        }
        if (Input.GetKey(KeyCode.S) || Input.mousePosition.y <= borderTHIICness)
        {
            pos.y -= Time.deltaTime*speed;
            Commands.ClearDropDownMenus();
        }
        pos.x = Mathf.Clamp(pos.x, -borderLimit.x, borderLimit.x);
        pos.y = Mathf.Clamp(pos.y, -borderLimit.y, borderLimit.y);

        transform.position = pos;
    }
}
