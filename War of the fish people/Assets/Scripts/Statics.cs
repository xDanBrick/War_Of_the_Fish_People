public class Statics
{
	public static int playerTurn = 1;
	public static int turns = 0;
	public static void SwitchTurns()
	{
		if (playerTurn == 1) {
			playerTurn = 2;
		} else 
		{
			playerTurn = 1;
	
		}
		turns++;
		gameTime = 30.0f;
	}
	public static int deployer = 0;
	public static float gameTime = 30.0f;
	public static int moveAction = 0;
	public static int attackAction = 1;
	public static int cancelAction = 2;
	public static int aimState = 0;
	public static int idleState = 1;
	public static int moveState = 2;
	public static int selectActionState = 3; 
	public static bool isWalking = false;
}


