using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextManager : MonoBehaviour
{
    public TMP_Text settingText, pauseText, startText, gameOverText,
                    scoreText, bestScoreText, levelText, forceUpdateText;

    private LanguageManager languageManager;

    public Button turkishButton, englishButton, spanishButton, 
                  germanButton, frenchButton;

    private string currentLanguageKey = "CurrentLanguage";
    private void Start()
    {
        languageManager = GetComponent<LanguageManager>();

        if (!PlayerPrefs.HasKey(currentLanguageKey))
        {
            PlayerPrefs.SetString(currentLanguageKey, Application.systemLanguage.ToString());
            PlayerPrefs.Save();
        }

        string savedLanguage = PlayerPrefs.GetString(currentLanguageKey, Application.systemLanguage.ToString());

        SetTexts(savedLanguage);

        turkishButton.onClick.AddListener(() => ChangeLanguage("Turkish"));
        englishButton.onClick.AddListener(() => ChangeLanguage("English"));
        spanishButton.onClick.AddListener(() => ChangeLanguage("Spanish"));
        germanButton.onClick.AddListener(() => ChangeLanguage("German"));
        frenchButton.onClick.AddListener(() => ChangeLanguage("French"));
    }

    private void ChangeLanguage(string language)
    {
        PlayerPrefs.SetString(currentLanguageKey, language);
        PlayerPrefs.Save();
        SetTexts(language);
    }
    
     
    private void SetTexts(string currentLanguage)
    {
        settingText.text = languageManager.GetText("settings", currentLanguage);
        pauseText.text = languageManager.GetText("pause", currentLanguage);
        startText.text = languageManager.GetText("start", currentLanguage);
        gameOverText.text = languageManager.GetText("game_over", currentLanguage);
        scoreText.text = languageManager.GetText("score", currentLanguage);
        bestScoreText.text = languageManager.GetText("best_score", currentLanguage);
        levelText.text = languageManager.GetText("level", currentLanguage);
        forceUpdateText.text = languageManager.GetText("force_update", currentLanguage);
    }
}
