using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenu : MonoBehaviour
{
    public TMP_InputField wizardInput;
    public TMP_InputField clericInput;
    public TMP_InputField enemyInput;
    public TextMeshProUGUI errorWizardClericText;
    public TextMeshProUGUI errorEnemyText;

    public int numWizards;
    public int numClerics;
    public int numSkeletons;
    public int numWarSkeletons;

    public void startGameButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void quitGameButton()
    {
        Application.Quit();
    }

    public void backToMainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void nextButton()
    {
        if (int.Parse(wizardInput.text) + int.Parse(clericInput.text) != 5)
        {
            errorWizardClericText.enabled = true;
        }
        else
        {
            errorWizardClericText.enabled = false;
        }
        
        if (int.Parse(enemyInput.text) > 10 || int.Parse(enemyInput.text) < 1)
        {
            errorEnemyText.enabled = true;
        }
        else
        {
            errorEnemyText.enabled = false;
        }

        if (int.Parse(enemyInput.text) <= 10 && (int.Parse(wizardInput.text) + int.Parse(clericInput.text) == 5))
        {
            this.numWizards = int.Parse(wizardInput.text);
            this.numClerics = int.Parse(clericInput.text);

            int numEnemies = int.Parse(enemyInput.text);

            this.numSkeletons = numEnemies / 2;
            this.numWarSkeletons = numEnemies - numSkeletons;

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
