using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageManager : MonoBehaviour
{
    Dictionary<string, Dictionary<string, string>> languages;

    private void Awake()
    {
        languages = new Dictionary<string, Dictionary<string, string>>();

        Dictionary<string, string> turkish = new Dictionary<string, string>();
        turkish.Add("start", "BA�LA");
        turkish.Add("pause", "DURDURULDU");
        turkish.Add("game_over", "OYUN B�TT�");
        turkish.Add("settings", "AYARLAR");
        turkish.Add("score", "Skor");
        turkish.Add("best_score", "En Y�ksek Skor");
        turkish.Add("level", "Seviye");
        turkish.Add("download", "G�ncelle");
        turkish.Add("new_version", "Yeni S�r�m");
        turkish.Add("language", "T�rk�e");

        Dictionary<string, string> english = new Dictionary<string, string>();
        english.Add("start", "START");
        english.Add("pause", "PAUSE");
        english.Add("game_over", "GAME OVER");
        english.Add("settings", "SETTINGS");
        english.Add("score", "Score"); 
        english.Add("best_score", "Best Score");
        english.Add("level", "Level");
        english.Add("download", "Update");
        english.Add("new_version", "New Version");
        english.Add("language", "English");

        Dictionary<string, string> spanish = new Dictionary<string, string>();
        spanish.Add("start", "COMENZAR");
        spanish.Add("pause", "PAUSA");
        spanish.Add("game_over", "JUEGO TERMINADO");
        spanish.Add("settings", "CONFIGURACI�N");
        spanish.Add("score", "Puntuaci�n");
        spanish.Add("best_score", "Mejor Puntuaci�n");
        spanish.Add("level", "Nivel");
        spanish.Add("download", "Actualizar");
        spanish.Add("new_version", "Nueva Versi�n");
        spanish.Add("language", "Espa�ol");

        Dictionary<string, string> german = new Dictionary<string, string>();
        german.Add("start", "SPIEL");
        german.Add("pause", "PAUSE");
        german.Add("game_over", "SPIEL VORBEI");
        german.Add("settings", "EINSTELLUNGEN");
        german.Add("score", "Punktestand");
        german.Add("best_score", "Bestes Punktestand");
        german.Add("level", "Niveau");
        german.Add("download", "Neuausgabe");
        german.Add("new_version", "Die Neue Fassung");
        german.Add("language", "Deutsche");

        Dictionary<string, string> french = new Dictionary<string, string>();
        french.Add("start", "JOUER");
        french.Add("pause", "PAUSE");
        french.Add("game_over", "JEU TERMIN�");
        french.Add("settings", "PARAM�TRES");
        french.Add("score", "Score");
        french.Add("best_score", "Meilleur Score");
        french.Add("level", "Niveler");
        french.Add("download", "Actualiser");
        french.Add("new_version", "Nouvelle Version");
        french.Add("language", "Fran�a�s");

        languages.Add("Turkish", turkish);
        languages.Add("English", english);
        languages.Add("Spanish", spanish);
        languages.Add("German", german);
        languages.Add("French", french);
    }

    public string GetText(string key, string language)
    {
        if (languages.ContainsKey(language))
        {
            Dictionary<string, string> selectedLanguage = languages[language];
            
            if (selectedLanguage.ContainsKey(key))
                return selectedLanguage[key];
            else
                return "Text not found!";
        }
        else
        {
            if (languages.ContainsKey("English"))
            {
                Dictionary<string, string> defaultLanguage = languages["English"];
                if (defaultLanguage.ContainsKey(key))
                    return defaultLanguage[key];
                else
                    return "Text not found in English!";
            }
            else
                return "Default language not found!";
        }
    }
}


