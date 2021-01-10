using UnityEngine;
public class Testing : MonoBehaviour
{
    [SerializeField] Sprite graphic;
    [SerializeField] int width, height;
    [SerializeField] float spacing;
    [SerializeField] Transform parent;
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void Start()
    {
        Grid grid = new Grid(width, height, spacing, graphic, parent);
    }
  
  
    private void Update()
    {
      
    }
}