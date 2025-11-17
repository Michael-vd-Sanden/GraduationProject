using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMazeMovement : MonoBehaviour
{
    public bool isOnMobile;

    private Touch theTouch;
    private Vector2 touchStartPosition, touchEndPosition;
    //private string direction;

    [SerializeField] private GameObject player;
    [SerializeField] private float travelDistance = 2f;

    private void Update()
    {
        if(isOnMobile) 
        { 
            if(Input.touchCount > 0)
            {
                theTouch = Input.GetTouch(0);
                if(theTouch.phase == TouchPhase.Began)
                {
                    touchStartPosition = theTouch.position;
                }
                else if (theTouch.phase == TouchPhase.Moved || theTouch.phase == TouchPhase.Ended) 
                { 
                    touchEndPosition = theTouch.position;
                    float x = touchEndPosition.x - touchStartPosition.x;
                    float y = touchEndPosition.y - touchStartPosition.y;

                    if (Mathf.Abs(x) == 0 && Mathf.Abs(y) == 0)
                    {
                        //direction = "Tapped";
                    }

                    else if (Mathf.Abs(x) > Mathf.Abs(y))
                    {
                        if (x > 0) 
                        { //right
                            player.transform.position += new Vector3(travelDistance, 0);
                        }
                        else
                        {//left
                            player.transform.position += new Vector3(-travelDistance, 0);
                        }
                        //direction = x > 0 ? "Right" : "Left";
                    }

                    else
                    {
                        if (y > 0)
                        {//up
                            player.transform.position += new Vector3(0, travelDistance);
                        }
                        else
                        {//down
                            player.transform.position += new Vector3(0, -travelDistance);
                        }
                        //direction = y > 0 ? "Up" : "Down";
                    }
                }
            }
        }
    }
}
