using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameController : MonoBehaviour {

	Board gameBoard;
	Spawner spawner;
	SoundManager soundManager;
	ScoreManager scoreManager;
	Shape activeShape;
	Ghost ghost;
	public float dropInterval = 0.1f;
	float dropIntervalModded, timeToDrop, timeToNextKeyLeftRight, timeToNextKeyDown, timeToNextKeyRotate;
	[Range(0.02f,1f)] public float keyRepeatRateLeftRight = 0.25f, keyRepeatRateRotate = 0.25f;
	[Range(0.01f,0.5f)] public float keyRepeatRateDown = 0.01f;
	public GameObject gameOverPanel, mainMenuPanel, settingsPanel, infoPanel, mainMenuBg, pausePanel;
	bool gameOver = false,  clockwise = true, didTap = false, gameStarted, wasPausedBefore = false;
	public bool isPaused = false;
	public GameObject gameUpper, pauseBack, buttonBack, boardCode, levelBoard, particleBoard, spawnerBoard, ghostBoard;
	enum Direction {none, left, right, up, down}
	Direction dragDirection = Direction.none, swipeDirection = Direction.none;
	float timeToNextDrag, timeToNextSwipe;
    [Range(0.05f,1f)] public float minTimeToDrag = 0.15f, minTimeToSwipe = 0.3f;
    void OnEnable()
	{
		TouchController.DragEvent += DragHandler;
		TouchController.SwipeEvent += SwipeHandler;
        TouchController.TapEvent += TapHandler;
	}
	void OnDisable()
	{
		TouchController.DragEvent -= DragHandler;
		TouchController.SwipeEvent -= SwipeHandler;
        TouchController.TapEvent -= TapHandler;
	}
    void Start () 
	{
		gameStarted = PlayerPrefs.GetInt("GameStarted", 0) == 1;
		if (!gameStarted)
			ToggleMainMenu();
        if (gameStarted)
        {
			mainMenuBg.SetActive(false);
			mainMenuPanel.SetActive(false);
			StartGame();
        }
	}
	void Update () 
	{
		if(gameStarted)
        {
			if (!spawner || !gameBoard || !activeShape || gameOver || !soundManager || !scoreManager)
				return;
			PlayerInput();
		}
	}
	public void StartGame()
    {
			gameStarted = true;
			PlayerPrefs.SetInt("GameStarted", 1);
			PlayerPrefs.Save();

			if (mainMenuPanel)
				mainMenuPanel.SetActive(false);
			if (mainMenuBg)
				mainMenuBg.SetActive(false);
			if (infoPanel)
				infoPanel.SetActive(false);
			boardCode.SetActive(true);
			levelBoard.SetActive(true);
			gameUpper.SetActive(true);
			spawnerBoard.SetActive(true);
			particleBoard.SetActive(true);
			ghostBoard.SetActive(true);
			gameBoard = GameObject.FindObjectOfType<Board>();
			spawner = GameObject.FindObjectOfType<Spawner>();
			soundManager = GameObject.FindObjectOfType<SoundManager>();
			scoreManager = GameObject.FindObjectOfType<ScoreManager>();
			ghost = GameObject.FindObjectOfType<Ghost>();
			timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;
			timeToNextKeyRotate = Time.time + keyRepeatRateRotate;
			timeToNextKeyDown = Time.time + keyRepeatRateDown;
			if (!gameBoard)
				Debug.Log("No Game Board");
			if (!soundManager)
				Debug.Log("No Game Sound");
			if (!spawner)
				Debug.Log("No Game Spawner");
			if (!scoreManager)
				Debug.Log("No Game Score");
			else
			{
				spawner.transform.position = Vectorf.Round(spawner.transform.position);
				if (!activeShape)
					activeShape = spawner.SpawnShape();
			}
			if (gameOverPanel)
				gameOverPanel.SetActive(false);
			if (pausePanel)
				pausePanel.SetActive(false);
			dropIntervalModded = dropInterval;
	}
	void LateUpdate()
	{
		if (ghost)
			ghost.DrawGhost(activeShape,gameBoard);
	}
	void MoveRight ()
	{
		activeShape.MoveRight ();
		timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

		if (!gameBoard.IsValidPosition (activeShape)) 
		{
			activeShape.MoveLeft ();
			PlaySound (soundManager.errorSound, 0.5f);
		}
		else 
			PlaySound (soundManager.moveSound, 0.5f);
	}
	void MoveLeft ()
	{
		activeShape.MoveLeft ();
		timeToNextKeyLeftRight = Time.time + keyRepeatRateLeftRight;

		if (!gameBoard.IsValidPosition (activeShape)) 
		{
			activeShape.MoveRight ();
			PlaySound (soundManager.errorSound, 0.5f);
		}
		else 
			PlaySound (soundManager.moveSound, 0.5f);
	}
	void Rotate ()
	{
		activeShape.RotateClockwise (clockwise);
		timeToNextKeyRotate = Time.time + keyRepeatRateRotate;

		if (!gameBoard.IsValidPosition (activeShape)) 
		{
			activeShape.RotateClockwise (!clockwise);
			PlaySound (soundManager.errorSound, 0.5f);
		}
		else 
			PlaySound (soundManager.moveSound, 0.5f);
	}
	void MoveDown ()
	{
		timeToDrop = Time.time + dropIntervalModded;
		timeToNextKeyDown = Time.time + keyRepeatRateDown;
		activeShape.MoveDown ();
		if (!gameBoard.IsValidPosition (activeShape)) 
		{
			if (gameBoard.IsOverLimit (activeShape))
				GameOver ();
			else 
				LandShape ();
		}
	}
	void PlayerInput ()
	{
		if ((Input.GetButton ("MoveRight") && (Time.time > timeToNextKeyLeftRight)) || Input.GetButtonDown ("MoveRight")) 
			MoveRight ();
		else if  ((Input.GetButton ("MoveLeft") && (Time.time > timeToNextKeyLeftRight)) || Input.GetButtonDown ("MoveLeft"))
			MoveLeft ();
		else if  (Input.GetButtonDown ("Rotate") && (Time.time > timeToNextKeyRotate)) 
			Rotate ();
		else if  ((Input.GetButton ("MoveDown") && (Time.time > timeToNextKeyDown)) ||  (Time.time > timeToDrop))
			MoveDown ();
        else if ( (swipeDirection == Direction.right && Time.time > timeToNextSwipe) || 
            (dragDirection == Direction.right && Time.time > timeToNextDrag))
		{
			MoveRight();
            timeToNextDrag = Time.time + minTimeToDrag;
            timeToNextSwipe = Time.time + minTimeToSwipe;
		}
        else if ( (swipeDirection == Direction.left && Time.time > timeToNextSwipe) ||
            (dragDirection == Direction.left && Time.time > timeToNextDrag))
		{
			MoveLeft();
            timeToNextDrag = Time.time + minTimeToDrag;
            timeToNextSwipe = Time.time + minTimeToSwipe;
		}
        else if ((swipeDirection == Direction.up && Time.time > timeToNextSwipe) || (didTap))
		{
			Rotate();
            timeToNextSwipe = Time.time + minTimeToSwipe;
            didTap = false;
		}
        else if (dragDirection == Direction.down && Time.time > timeToNextDrag)
			MoveDown();
		else if (Input.GetButtonDown("Pause"))
			TogglePause();

        dragDirection = Direction.none;
        swipeDirection = Direction.none;
        didTap = false;
	}
	void LandShape ()
	{
		if(activeShape)
        {
			activeShape.MoveUp();
			gameBoard.StoreShapeInGrid(activeShape);

			activeShape.LandShapeFX();

			if (ghost)
				ghost.Reset();
			activeShape = spawner.SpawnShape();

			timeToNextKeyLeftRight = Time.time;
			timeToNextKeyDown = Time.time;
			timeToNextKeyRotate = Time.time;

			gameBoard.StartCoroutine("ClearAllRows");

			PlaySound(soundManager.dropSound);

			if (gameBoard.completedRows > 0)
			{
				scoreManager.ScoreLines(gameBoard.completedRows);

				if (scoreManager.didLevelUp)
				{
					dropIntervalModded = Mathf.Clamp(dropInterval - ((float)scoreManager.level * 0.05f), 0.05f, 1f);
					PlaySound(soundManager.levelUpVocalClip);
				}
				else
				{
					if (gameBoard.completedRows > 1)
					{
						AudioClip randomVocal = soundManager.GetRandomClip(soundManager.vocalClips);
						PlaySound(randomVocal);
					}
				}
				PlaySound(soundManager.clearRowSound);
			}
		}
	}
	void GameOver ()
	{
		activeShape.MoveUp();
		if (gameOverPanel)
			gameOverPanel.SetActive(true);
		PlaySound(soundManager.gameOverSound, 5f);
		gameOver = true;
		isPaused = true;
	}
	public void Restart()
	{
		gameStarted = true;
		PlayerPrefs.SetInt("GameStarted", 1);
		PlayerPrefs.Save();
		Time.timeScale = 1f;
		Scene currentScene = SceneManager.GetActiveScene();
		SceneManager.LoadScene(currentScene.buildIndex);
		if (mainMenuPanel)
			mainMenuPanel.SetActive(false);
		if (mainMenuBg)
			mainMenuBg.SetActive(false);
		pausePanel.SetActive(false);
	}
	void PlaySound (AudioClip clip, float volMultiplier = 1.0f)
	{
		if (soundManager.fxEnabled && clip)
			AudioSource.PlayClipAtPoint (clip, Camera.main.transform.position, Mathf.Clamp(soundManager.fxVolume*volMultiplier,0.05f,1f));
	}
	public void TogglePause()
	{
		gameOverPanel.SetActive(false);
		isPaused = !isPaused;
		
		if (pausePanel)
		{
			pausePanel.SetActive(isPaused);
			if (isPaused)
			{
				wasPausedBefore = Time.timeScale == 0; 
				Time.timeScale = 0;
			}
			else
			{
				if (!wasPausedBefore)
					Time.timeScale = 1;
			}
		}
	}
	void DragHandler(Vector2 dragMovement)
	{
		dragDirection = GetDirection(dragMovement);
	}
	void SwipeHandler(Vector2 swipeMovement)
	{
		swipeDirection = GetDirection(swipeMovement);
	}
    void TapHandler(Vector2 tapMovement)
    {
        didTap = true;
    }
	Direction GetDirection(Vector2 swipeMovement)
	{
		Direction swipeDir = Direction.none;
		if (Mathf.Abs(swipeMovement.x) > Mathf.Abs(swipeMovement.y))
			swipeDir = (swipeMovement.x >=0) ? Direction.right : Direction.left;
		else
			swipeDir = (swipeMovement.y >=0) ? Direction.up : Direction.down;
		return swipeDir;
	}
	public void ToggleSettings()
	{
		gameOverPanel.SetActive(false);
		settingsPanel.SetActive(true);
		pausePanel.SetActive(false);

		if (gameOver)
			buttonBack.SetActive(true);
	}
	public void ToggleSettingsBack()
	{
		if (gameOverPanel)
			gameOverPanel.SetActive(true);
		settingsPanel.SetActive(false);
	}
	public void TogglePauseSettingsBack()
	{
		if (pausePanel)
			pausePanel.SetActive(true);
		settingsPanel.SetActive(false);
	}
	public void ToggleMainMenu()
	{
		gameStarted = false;
		PlayerPrefs.SetInt("GameStarted", 0);
		PlayerPrefs.Save();
		mainMenuPanel.SetActive(true);
		mainMenuBg.SetActive(true);
		gameOverPanel.SetActive(false);
		infoPanel.SetActive(false);
		pausePanel.SetActive(false);
		gameUpper.SetActive(false);
		boardCode.SetActive(false);
		levelBoard.SetActive(false);
		spawnerBoard.SetActive(false);
		particleBoard.SetActive(false);
		ghostBoard.SetActive(false);
		settingsPanel.SetActive(false);
	}
	public void ToggleMainMenuInfo()
	{
		infoPanel.SetActive(true);
	}
	public void ToggleMainMenuSettings()
	{
		settingsPanel.SetActive(true);
		buttonBack.SetActive(false);
	}
	public void ToggleInfoExit()
	{
		infoPanel.SetActive(false);
	}
}

