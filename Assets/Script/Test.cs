using UnityEngine;

namespace Script
{
    public class Test : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hitInfo))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    print(hitInfo.transform.name);
                }
            }
        }
    }
}
