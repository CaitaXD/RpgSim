using UnityEngine;
public class Testing : MonoBehaviour
{
    [SerializeField] Sprite graphic;
    public int width, height;
    public float spacing;
    [SerializeField] Transform parent;
    [SerializeField] Camera _myCam;
    public Grid grid;
    private void Awake()
    {
        _myCam = _myCam ? _myCam : Camera.main;
    }
    void Start()
    {
        grid = new Grid(width, height, spacing, graphic, parent);
       _myCam.GetComponent<CameraControler>().borderLimit = new Vector2(width/2.5f, height/2.5f);
    }
    private void Update()
    {
      
    }
}