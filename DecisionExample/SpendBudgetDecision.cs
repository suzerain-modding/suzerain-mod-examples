using SuzerainModdingKit;
using SuzerainModdingKit.StoryFragments.Decision;
using SuzerainModdingKit.StoryPack;

namespace DecisionExample;

public static class SpendBudgetDecision
{
    // Options for this decision.
    public const string SpendBudgetOptionText = "Spend budget [-1 Government Budget]";
    public const string NothingOptionText = "There is nothing we can do...";

    // Game variables for this decision.
    public const string SpendBudgetOptionVar = "DecisionExample.SpendBudgetDecision_SpendBudget";
    public const string NothingOptionVar = "DecisionExample.SpendBudgetDecision_Nothing";

    // The properties for this decision.
    public static readonly CustomDecisionData Data = new(
        // The unique name of the decision.
        name: "DecisionExample.MyDecision",
        // The story pack that this decision will be displayed in.
        storyPack: SuzerainStoryPackInfo.Sordland,
        // The token that this decision should be displayed on.
        assignedTokenName: SuzerainTokenName.SordlandCityHolsord,
        // The title of the decision when viewed in the decision panel.
        title: "The Budget Question",
        // The description of the decision when viewed in the decision panel.
        description: "We are faced with a choice: Spend government budget for no reason, or don't.",
        // The title of the decision when viewed under the assigned token.
        hubTitle: "The Budget Question",
        // The description of the decision when viewed under the assigned token.
        hubDescription: "Should we spend budget?");

    public static void Init()
    {
        // Suzerain Modding Kit needs to know that our variables exist.
        Variables.Register(SpendBudgetOptionVar);
        Variables.Register(NothingOptionVar);
    }

    public static void OnEvaluateStep()
    {
        // If we're not in Sordland on turn 1 step 2, return.
        if (!GameState.IsCurrentStoryPack(SuzerainStoryPackInfo.Sordland) ||
            GameState.CurrentTurnNum != 1 ||
            GameState.CurrentStepNum != 2)
        {
            return;
        }
        // Why turn 1 step 2? Turn 1 step 1 is only the presidential inauguration scene.
        // Attempting to add a story fragment during that step will throw an exception and may
        // break the game. Step 2 is the first step we can add story fragments to.

        // Return if the decision has already been added.
        if (GameState.StoryFragmentExistsInCurrentStep(Data.Name))
        {
            return;
        }

        // Return if the user has already made a choice.
        if (Variables.GetBool(SpendBudgetOptionVar) || Variables.GetBool(NothingOptionVar))
        {
            return;
        }

        // Add the story fragment to the game. We don't care about the return value, discard it.
        _ = GameState.AddCustomStoryFragment(Data);
    }

    public static void OnDecisionShow()
    {
        // If the current decision name does not match our decision name, return.
        if (!Data.Name.Equals(DecisionManager.CurrentDecisionName, StringComparison.Ordinal))
        {
            return;
        }

        // Only add the spend budget option if the user has enough budget to spend.
        if (Variables.GetInt("BaseGame.GovernmentBudget") >= 1)
        {
            DecisionManager.AddOption(SpendBudgetOptionText);
        }

        // Always add the nothing option.
        DecisionManager.AddOption(NothingOptionText);
    }

    public static void OnDecisionFinished(DecisionOptionInfo selectedOption)
    {
        // If the decision name of the selected option does not match our decision name, return.
        if (!Data.Name.Equals(selectedOption.DecisionName, StringComparison.Ordinal))
        {
            return;
        }

        // If the user selected the spend budget option, set the appropriate variables.
        if (SpendBudgetOptionText.Equals(selectedOption.Text, StringComparison.Ordinal))
        {
            // Subtract 1 from the government budget.
            int governmentBudget = Variables.GetInt("BaseGame.GovernmentBudget");
            Variables.Set("BaseGame.GovernmentBudget", governmentBudget - 1);

            // Set our spend budget option variable to true.
            Variables.Set(SpendBudgetOptionVar, true);

            // Early return.
            return;
        }

        // If we reach this point of the method, the user must have selected the nothing option.
        // Set the appropriate variables.
        Variables.Set(NothingOptionVar, true);
    }
}