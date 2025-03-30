using UnityEngine;
using System.Collections;

public class SwipeScroller : MonoBehaviour
{
    public GameObject[] items; // Assign 8 GameObjects in the Inspector
    public float swipeSpeed = 5f; // Speed of movement
    public float itemSpacing = 2f; // Distance between items

    private Vector2 startTouchPosition, endTouchPosition;
    private float targetX;
    private float minX, maxX;
    public float moveAmount = 2f; // Amount to move objects to the right

    private Vector3[] startPositions;
    private int currentItemIndex = 0;

    void Start()
    {
        StartCoroutine(MoveNextItemRight());
    }

    void Update()
    {
       
   

        // Smoothly move items to the target position
        MoveItems();
    }



    void MoveItems()
    {
        foreach (GameObject item in items)
        {
            Vector3 targetPosition = item.transform.position;
            targetPosition.x = Mathf.Lerp(item.transform.position.x, item.transform.position.x - 1, Time.deltaTime * swipeSpeed);
            item.transform.position = targetPosition;
        }
    }

    IEnumerator MoveNextItemRight()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.5f);
            if (items.Length > 0)
            {
                items[currentItemIndex].transform.position += new Vector3(moveAmount, 0, 0);
                currentItemIndex = (currentItemIndex + 1) % items.Length;
            }
        }
    }
}