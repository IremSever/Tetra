using UnityEngine;
using TMPro;
public class ScoreManager : MonoBehaviour {
	
	public int score = 0, bestScore = 0, lines;
	public int level = 1, linesPerLevel = 5;
	public TextMeshProUGUI textScore, textLines, textLevel, textBestScore, textScore2;
	const int minLines = 1, maxLines = 4;
	public bool didLevelUp = false;
	string bestScoreKey = "BestScore";
	public GameObject spawnVortex;
	void Awake()
	{
		LoadBestScore();
	}
	void Start()
	{
		Reset();
        spawnVortex.SetActive(false);
	}
    public void ScoreLines(int n)
	{
		didLevelUp = false;
		n = Mathf.Clamp(n, minLines, maxLines);
		switch (n)
		{
			case 1:
				score += 40 * level;
				break;
			case 2:
				score += 100 * level;
				break;
			case 3:
				score += 300 * level;
				break;
			case 4:
				score += 1200 * level;
				break;
		}
		lines -= n;
		if (lines <= 0)
			LevelUp();
		if (score > bestScore)
		{
			bestScore = score;
			SaveBestScore();
		}
		UpdateText();
	}
	public void Reset()
	{
		level = 1;
		lines = linesPerLevel * level;
		UpdateText();
	}
	void UpdateText()
	{
		if (textLines)
			textLines.text = lines.ToString();
		if (textScore)
		{
			textScore.text = PadZero(score, 1);
			textScore2.text = PadZero(score, 1);
		}
		if (textLevel)
			textLevel.text = level.ToString();

		if (textBestScore)
			textBestScore.text = PadZero(bestScore, 1);
	}
	string PadZero(int n, int padDigits)
	{
		return n.ToString().PadLeft(padDigits, '0');
	}
	public void LevelUp()
	{
		level++;
		lines = linesPerLevel * level;
		didLevelUp = true;
    }
    private void Update()
    {
		if (didLevelUp)
			spawnVortex.SetActive(true);
		else
			spawnVortex.SetActive(false);

	}
	void SaveBestScore()
	{
		PlayerPrefs.SetInt(bestScoreKey, bestScore);
		PlayerPrefs.Save();
	}
	void LoadBestScore()
	{
		if (PlayerPrefs.HasKey(bestScoreKey))
			bestScore = PlayerPrefs.GetInt(bestScoreKey);
	}
}
