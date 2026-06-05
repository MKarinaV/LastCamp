using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Top UI")]
    public TMP_Text dayText;
    public TMP_Text foodText;
    public TMP_Text woodText;
    public TMP_Text metalText;

    [Header("Survivors UI")]
    public TMP_Text hunterHPText;
    public TMP_Text hunterHungerText;
    public TMP_Text engineerHPText;
    public TMP_Text engineerHungerText;
    public TMP_Text medicHPText;
    public TMP_Text medicHungerText;

    [Header("Info UI")]
    public TMP_Text selectedText;
    public TMP_Text eventLogText;

    private int day = 1;
    private int maxDays = 10;

    private int food = 15;
    private int wood = 10;
    private int metal = 5;

    private int hunterHP = 100;
    private int engineerHP = 100;
    private int medicHP = 100;

    private int hunterHunger = 100;
    private int engineerHunger = 100;
    private int medicHunger = 100;

    private string selectedSurvivor = "NONE";

    private bool hasAxe = false;
    private bool hasCampfire = false;

    private bool hunterActed = false;
    private bool engineerActed = false;
    private bool medicActed = false;

    void Start()
    {
        UpdateUI();
        selectedText.text = "SELECTED: NONE";
        eventLogText.text = "Choose survivor and action.";
    }

    void UpdateUI()
    {
        dayText.text = "DAY: " + day + "/" + maxDays;
        foodText.text = "FOOD: " + food;
        woodText.text = "WOOD: " + wood;
        metalText.text = "METAL: " + metal;

        hunterHPText.text = "HP: " + hunterHP + (hunterActed ? " | DONE" : "");
        hunterHungerText.text = "Hunger: " + hunterHunger;

        engineerHPText.text = "HP: " + engineerHP + (engineerActed ? " | DONE" : "");
        engineerHungerText.text = "Hunger: " + engineerHunger;

        medicHPText.text = "HP: " + medicHP + (medicActed ? " | DONE" : "");
        medicHungerText.text = "Hunger: " + medicHunger;
    }

    public void SelectHunter()
    {
        selectedSurvivor = "HUNTER";
        selectedText.text = "SELECTED: HUNTER";
        eventLogText.text = hunterActed ? "Hunter already acted today." : "Hunter selected.";
    }

    public void SelectEngineer()
    {
        selectedSurvivor = "ENGINEER";
        selectedText.text = "SELECTED: ENGINEER";
        eventLogText.text = engineerActed ? "Engineer already acted today." : "Engineer selected.";
    }

    public void SelectMedic()
    {
        selectedSurvivor = "MEDIC";
        selectedText.text = "SELECTED: MEDIC";
        eventLogText.text = medicActed ? "Medic already acted today." : "Medic selected.";
    }

    private bool CanSelectedSurvivorAct()
    {
        if (selectedSurvivor == "NONE")
        {
            eventLogText.text = "Choose a survivor first.";
            return false;
        }

        if (selectedSurvivor == "HUNTER" && hunterActed)
        {
            eventLogText.text = "Hunter already acted today.";
            return false;
        }

        if (selectedSurvivor == "ENGINEER" && engineerActed)
        {
            eventLogText.text = "Engineer already acted today.";
            return false;
        }

        if (selectedSurvivor == "MEDIC" && medicActed)
        {
            eventLogText.text = "Medic already acted today.";
            return false;
        }

        return true;
    }

    private void MarkSelectedSurvivorActed()
    {
        if (selectedSurvivor == "HUNTER")
        {
            hunterActed = true;
        }
        else if (selectedSurvivor == "ENGINEER")
        {
            engineerActed = true;
        }
        else if (selectedSurvivor == "MEDIC")
        {
            medicActed = true;
        }
    }

    private void ResetDailyActions()
    {
        hunterActed = false;
        engineerActed = false;
        medicActed = false;
    }

    public void SearchFood()
    {
        if (!CanSelectedSurvivorAct()) return;

        int foundFood = 2;

        if (selectedSurvivor == "HUNTER")
        {
            foundFood += 2;
        }

        food += foundFood;
        MarkSelectedSurvivorActed();

        eventLogText.text = selectedSurvivor + " found " + foundFood + " food.";
        UpdateUI();
    }

    public void GatherWood()
    {
        if (!CanSelectedSurvivorAct()) return;

        int foundWood = 3;

        if (selectedSurvivor == "ENGINEER")
        {
            foundWood += 2;
        }

        if (hasAxe)
        {
            foundWood += 2;
        }

        wood += foundWood;
        MarkSelectedSurvivorActed();

        eventLogText.text = selectedSurvivor + " gathered " + foundWood + " wood.";
        UpdateUI();
    }

    public void GatherMetal()
    {
        if (!CanSelectedSurvivorAct()) return;

        int foundMetal = 2;

        if (selectedSurvivor == "ENGINEER")
        {
            foundMetal += 2;
        }

        metal += foundMetal;
        MarkSelectedSurvivorActed();

        eventLogText.text = selectedSurvivor + " found " + foundMetal + " metal.";
        UpdateUI();
    }

    public void FeedSurvivor()
    {
        if (!CanSelectedSurvivorAct()) return;

        if (food <= 0)
        {
            eventLogText.text = "No food left.";
            return;
        }

        food -= 1;

        if (selectedSurvivor == "HUNTER")
        {
            hunterHunger = Mathf.Min(hunterHunger + 25, 100);
        }
        else if (selectedSurvivor == "ENGINEER")
        {
            engineerHunger = Mathf.Min(engineerHunger + 25, 100);
        }
        else if (selectedSurvivor == "MEDIC")
        {
            medicHunger = Mathf.Min(medicHunger + 25, 100);
        }

        MarkSelectedSurvivorActed();

        eventLogText.text = selectedSurvivor + " was fed. Hunger restored by 25.";
        UpdateUI();
    }

    public void Rest()
    {
        if (!CanSelectedSurvivorAct()) return;

        int restoreAmount = 12;

        if (selectedSurvivor == "MEDIC")
        {
            restoreAmount = 25;
        }

        if (selectedSurvivor == "HUNTER")
        {
            hunterHP = Mathf.Min(hunterHP + restoreAmount, 100);
            hunterHunger = Mathf.Min(hunterHunger + restoreAmount, 100);
        }
        else if (selectedSurvivor == "ENGINEER")
        {
            engineerHP = Mathf.Min(engineerHP + restoreAmount, 100);
            engineerHunger = Mathf.Min(engineerHunger + restoreAmount, 100);
        }
        else if (selectedSurvivor == "MEDIC")
        {
            medicHP = Mathf.Min(medicHP + restoreAmount, 100);
            medicHunger = Mathf.Min(medicHunger + restoreAmount, 100);
        }

        MarkSelectedSurvivorActed();

        eventLogText.text = selectedSurvivor + " rested and restored " + restoreAmount + " points.";
        UpdateUI();
    }

    public void CraftAxe()
    {
        if (hasAxe)
        {
            eventLogText.text = "Axe is already crafted.";
            return;
        }

        if (wood >= 3 && metal >= 1)
        {
            wood -= 3;
            metal -= 1;
            hasAxe = true;
            eventLogText.text = "Axe crafted. Wood gathering increased.";
        }
        else
        {
            eventLogText.text = "Not enough resources. Need 3 wood and 1 metal.";
        }

        UpdateUI();
    }

    public void CraftMedkit()
    {
        if (selectedSurvivor == "NONE")
        {
            eventLogText.text = "Choose a survivor first.";
            return;
        }

        if (wood >= 2 && metal >= 2)
        {
            wood -= 2;
            metal -= 2;

            if (selectedSurvivor == "HUNTER")
            {
                hunterHP = Mathf.Min(hunterHP + 35, 100);
            }
            else if (selectedSurvivor == "ENGINEER")
            {
                engineerHP = Mathf.Min(engineerHP + 35, 100);
            }
            else if (selectedSurvivor == "MEDIC")
            {
                medicHP = Mathf.Min(medicHP + 35, 100);
            }

            eventLogText.text = "Medkit used. " + selectedSurvivor + " restored 35 HP.";
        }
        else
        {
            eventLogText.text = "Not enough resources. Need 2 wood and 2 metal.";
        }

        UpdateUI();
    }

    public void BuildCampfire()
    {
        if (hasCampfire)
        {
            eventLogText.text = "Campfire is already built.";
            return;
        }

        if (wood >= 5)
        {
            wood -= 5;
            hasCampfire = true;
            eventLogText.text = "Campfire built. Hunger loss reduced.";
        }
        else
        {
            eventLogText.text = "Not enough resources. Need 5 wood.";
        }

        UpdateUI();
    }

    public void EndDay()
    {
        day++;

        int hungerLoss = hasCampfire ? 15 : 25;

        hunterHunger -= hungerLoss;
        engineerHunger -= hungerLoss;
        medicHunger -= hungerLoss;

        eventLogText.text = "The group survived another harsh day.";

        ApplyDailyHealthLoss();
        CheckHungerDamage();

        ResetDailyActions();

        if (day > maxDays)
        {
            SceneManager.LoadScene("Win");
            return;
        }

        if (hunterHP <= 0 && engineerHP <= 0 && medicHP <= 0)
        {
            SceneManager.LoadScene("Lose");
            return;
        }

        UpdateUI();
    }

    private void ApplyDailyHealthLoss()
    {
        hunterHP -= 7;
        engineerHP -= 7;
        medicHP -= 7;
    }

    private void CheckHungerDamage()
    {
        if (hunterHunger <= 30)
        {
            hunterHP -= 10;
        }

        if (hunterHunger <= 0)
        {
            hunterHunger = 0;
            hunterHP -= 20;
        }

        if (engineerHunger <= 30)
        {
            engineerHP -= 10;
        }

        if (engineerHunger <= 0)
        {
            engineerHunger = 0;
            engineerHP -= 20;
        }

        if (medicHunger <= 30)
        {
            medicHP -= 10;
        }

        if (medicHunger <= 0)
        {
            medicHunger = 0;
            medicHP -= 20;
        }

        hunterHP = Mathf.Max(hunterHP, 0);
        engineerHP = Mathf.Max(engineerHP, 0);
        medicHP = Mathf.Max(medicHP, 0);
    }
}