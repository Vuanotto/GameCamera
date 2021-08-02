
//-FOR DETERMINING THE ORIENTATION OF THE RAIL-
public enum EnumCameraRail
{
    VERTICAL,
    HORIZONTAL
}

//-TO BE ATTACHED TO A BOX COLLIDER-
//USE SCALE TO DETERMINE THE SIZE OF THE COLLIDER, NOT THE ACTUAL NUMBERS THAT YOU CAN SET UP ON THE COLLIDERS
public class WorldPieceCameraRail : MonoBehaviour
{
    public EnumCameraRail rail = EnumCameraRail.HORIZONTAL;
}


//-COLLIDER THAT WILL BE CREATED FOR THE CAMERA-
//NOTE: [GameCamera] is the name of my camera script, feel free to change whatever is appropriate
public class CameraCollider : MonoBehaviour
{
    public [GameCamera] camera;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.AddComponent<CircleCollider2D>();
        gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        if (camera == null) camera = GetComponentInParent<[GameCamera]>();
		
		//SET THE SCALE
        cameraCollider.transform.localScale = new Vector2(6f, 6f);
    }
	
	void Update()
	{
		//GET THE COLLIDER TO FOLLOW THE CAMERA
		cameraCollider.transform.position = camera.transform.position;
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<WorldPieceCameraRail>() && camera != null)
        {
            WorldPieceCameraRail rail = collision.GetComponent<WorldPieceCameraRail>();
            if (rail.rail == EnumCameraRail.HORIZONTAL)
            {
                camera.horizontalRails.Add(rail.gameObject);
            }
            else if (rail.rail == EnumCameraRail.VERTICAL)
            {
                camera.verticalRails.Add(rail.gameObject);
            }
        }        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<WorldPieceCameraRail>() && camera != null)
        {
            WorldPieceCameraRail rail = collision.GetComponent<WorldPieceCameraRail>();
            if (rail.rail == EnumCameraRail.HORIZONTAL)
            {
                camera.horizontalRails.Remove(rail.gameObject);
            }
            else if (rail.rail == EnumCameraRail.VERTICAL)
            {
                camera.verticalRails.Remove(rail.gameObject);
            }
        }
    }
}

//-CODE SNIPPET FOR MOVING-
//ADAPT THIS INTO YOUR CAMERA CODE IG
{
			Vector3 positionToMoveTo = new Vector3(destination.x, destionation.y, -10);

			//INITIALISE THE X AND Y LIMITS
            float lowestXLimit = transform.position.x;
            List<float> lowerXLimits = new List<float>();
            float highestXLimit = transform.position.x;
            List<float> higherXLimits = new List<float>();
            float lowestYLimit = transform.position.y;
            List<float> lowerYLimits = new List<float>();
            float highestYLimit = transform.position.y;
            List<float> higherYLimits = new List<float>();
            float xToMoveTo = 0;
            float yToMoveTo = 0;

			//CHECK WHERE CAMERA COULD BE MOVING
            if (positionToMoveTo.x < transform.position.x) xToMoveTo = transform.position.x - (cameraSpeed * Time.deltaTime);
            else xToMoveTo = transform.position.x + (cameraSpeed * Time.deltaTime);
			
			//CHECK AGAIN
            if (positionToMoveTo.y < transform.position.y) yToMoveTo = transform.position.y - (cameraSpeed * Time.deltaTime);
            else yToMoveTo = transform.position.y + (cameraSpeed * Time.deltaTime);

			//DEPENDING ON YOUR GAME, YOU CAN COMBINE THESE NEXT TWO STEPS
			//INTO IF YOU WANT TO USE A BOX INSTEAD OF RAILS
			
			//GET OUR X AND Y LIMITS FROM THE HORIZONTAL RAILS
            if (horizontalRails.Count > 0)
            {
                foreach (GameObject rail in horizontalRails)
                {
                    lowerXLimits.Add(rail.transform.position.x - rail.transform.localScale.x / 2);
                    higherXLimits.Add(rail.transform.position.x + rail.transform.localScale.x / 2);
                    lowerYLimits.Add(rail.transform.position.y);
                    higherYLimits.Add(rail.transform.position.y);
                }
            }
            
			//GET OUR X AND Y LIMITS FROM THE VERTICAL RAILS
            if (verticalRails.Count > 0)
            {
                foreach (GameObject rail in verticalRails)
                {
                    lowerYLimits.Add(rail.transform.position.y - rail.transform.localScale.y / 2);
                    higherYLimits.Add(rail.transform.position.y + rail.transform.localScale.y / 2);
                    lowerXLimits.Add(rail.transform.position.x);
                    higherXLimits.Add(rail.transform.position.x);
                }
            }

			//CHECK WHETHER OR NOT WE ARE WITHIN LIMIT
            if(lowerYLimits.Count > 0) foreach(float limit in lowerYLimits)
                    if (limit < lowestYLimit) lowestYLimit = limit;

            if (lowerXLimits.Count > 0) foreach (float limit in lowerXLimits)
                    if (limit < lowestXLimit) lowestXLimit = limit;

            if (higherYLimits.Count > 0) foreach (float limit in higherYLimits)
                    if (limit > highestYLimit) highestYLimit = limit;

            if (higherXLimits.Count > 0) foreach (float limit in higherXLimits)
                    if (limit > highestXLimit) highestXLimit = limit;

            if (xToMoveTo < lowestXLimit) xToMoveTo = lowestXLimit;
            if (xToMoveTo > highestXLimit) xToMoveTo = highestXLimit;
            if (yToMoveTo < lowestYLimit) yToMoveTo = lowestYLimit;
            if (yToMoveTo > highestYLimit) yToMoveTo = highestYLimit;

			//AND UPDATE WITH THE NEW VALUES
            positionToMoveTo = new Vector3(xToMoveTo, yToMoveTo, -10);
}
