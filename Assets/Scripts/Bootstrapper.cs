using UnityEngine;

public class Bootstrapper : MonoBehaviour
{
    // 10 x 10 map
    int[] firstmap = new int[100]
{
     50,  50,  50,  50,  50,  50,  50,  50,  50,  50,
     50,  50,  52,  55,  55,  55,  55,  52,  50,  50,
     50,  52,  60,  75,  85,  85,  75,  60,  52,  50,
     50,  55,  75, 110, 135, 135, 110,  75,  55,  50,
     50,  55,  85, 135, 155, 155, 135,  85,  55,  50,
     50,  55,  85, 135, 155, 155, 135,  85,  55,  50,
     50,  55,  75, 110, 135, 135, 110,  75,  55,  50,
     50,  52,  60,  75,  85,  85,  75,  60,  52,  50,
     50,  50,  52,  55,  55,  55,  55,  52,  50,  50,
     50,  50,  50,  50,  50,  50,  50,  50,  50,  50
};

    void Awake()
    {
        CellGridController cgc = GetComponent<CellGridController>();
        if (cgc != null)
        {
            cgc.Initialize(firstmap, 10);
            Debug.Log($"CGC initialized");
        }
        else
        {
            Debug.LogError($"CGC is missing");
        }
    }

    
    void Update()
    {
        
    }
}
