using UnityEngine;
using UnityEngine.UI;

//References:
//https://www.youtube.com/watch?v=oBkfujKPZw8

public class GoalDetect : MonoBehaviour
{
    //public float targetDist = 0f;
    public Image waypointMarker;    //image of the waypoint marker
    public Text distText;   //numeric text

    public Transform target;    //transform of the waypoint target

    // Update is called once per frame
    void Update()
    {
        float minX = waypointMarker.GetPixelAdjustedRect().width / 2;
            //gets the smallest x-value of the screen
        float maxX = Screen.width - minX;   //gets the minimum x-value
        
        float minY = waypointMarker.GetPixelAdjustedRect().height / 2;
            //gets the smallest x-value of the screen
        float maxY = Screen.height - minY;   //gets the maximum x-value
        
        //waypointMarker.transform.position = Camera.main.WorldToScreenPoint(target.position);
            //puts the waypoint image in the camera's view/sets it based on the camera
       
        Vector2 wayPos = Camera.main.WorldToScreenPoint(target.position);
            //puts the waypoint image in the camera's view/sets it based on the camera as a Vector2

        //the following if statement returns -1 if waypointMarker is behind the player, 
        //1 if waypointMarker is in front, 
        //and 0 if player and target are side-by-side each other
        if(Vector3.Dot((target.position - transform.position), transform.forward) < 0)
        {
            //left side of the player
            if(wayPos.x < Screen.width / 2)
            {
                wayPos.x = maxX;
            }
            else
            {
                wayPos.x = minX;
            }

        }

        wayPos.x = Mathf.Clamp(wayPos.x, minX, maxX);   //limits the values of the min and max x-values
        wayPos.y = Mathf.Clamp(wayPos.y, minY, maxY);   //limits the values of the min and max y-values

        waypointMarker.transform.position = wayPos;
            //places the waypoint marker on the screen within wayPos's parameters

        distText.text = "Goal Distance: " + ((int)Vector3.Distance(target.position, transform.position)).ToString() + "m";
    }

}
