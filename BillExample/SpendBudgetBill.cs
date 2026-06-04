using SuzerainModdingKit;
using SuzerainModdingKit.StoryFragments.Bill;
using SuzerainModdingKit.StoryPack;

namespace BillExample;

public static class SpendBudgetBill
{
    // Game variables for this decision.
    public const string VetoedVar = "BillExample.SpendBudgetBill_Vetoed";
    public const string SignedVar = "BillExample.SpendBudgetBill_Signed";

    // The properties for this bill.
    public static readonly CustomBillData Data = new(
        // The unique name of the bill.
        name: "BillExample.MyBill",
        // The story pack that this bill will be displayed in.
        storyPack: SuzerainStoryPackInfo.Sordland,
        // The token that this bill should be displayed on.
        assignedTokenName: SuzerainTokenName.SordlandCityHolsord,
        // The title of the bill when viewed in the bill panel.
        title: "The Budget Act",
        // The description of the bill when viewed in the bill panel.
        description: "Sign this bill to spend budget. [-1 Government Budget]",
        // The title of the decision when viewed under the assigned token.
        hubTitle: "The Budget Act",
        // The description of the decision when viewed under the assigned token.
        hubDescription: "The GNA has passed The Budget Act.");

    internal static void Init()
    {
        // Suzerain Modding Kit needs to know that our variables exist.
        Variables.Register(VetoedVar);
        Variables.Register(SignedVar);
    }

    internal static void OnEvaluateStep()
    {
        // If we're not in Sordland on turn 1 step 2, return.
        if (!GameState.IsCurrentStoryPack(SuzerainStoryPackInfo.Sordland) ||
            GameState.CurrentTurnNum != 1 ||
            GameState.CurrentStepNum != 2)
        {
            return;
        }

        // Return if the bill has already been added.
        if (GameState.StoryFragmentExistsInCurrentStep(Data.Name))
        {
            return;
        }

        // Return if the user has already made a choice.
        if (Variables.GetBool(VetoedVar) || Variables.GetBool(SignedVar))
        {
            return;
        }

        // Add the story fragment to the game. We don't care about the return value, discard it.
        _ = GameState.AddCustomStoryFragment(Data);
    }

    internal static void OnBillSigned(string billName)
    {
        // If the bill name does not match our bill name, return.
        if (!Data.Name.Equals(billName, StringComparison.Ordinal))
        {
            return;
        }

        // Subtract 1 from the government budget.
        int governmentBudget = Variables.GetInt("BaseGame.GovernmentBudget");
        Variables.Set("BaseGame.GovernmentBudget", governmentBudget - 1);

        // Set our signed variable to true.
        Variables.Set(SignedVar, true);
    }

    internal static void OnBillVetoed(string billName)
    {
        // If the bill name does not match our bill name, return.
        if (!Data.Name.Equals(billName, StringComparison.Ordinal))
        {
            return;
        }

        // Set our vetoed variable to true.
        Variables.Set(VetoedVar, true);
    }
}