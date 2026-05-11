using MelonLoader;
using SuzerainModdingKit;

[assembly: MelonInfo(typeof(DecisionExample.Core), "Decision Example", "1.0.0", "Fluffyalien1422", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]

namespace DecisionExample;

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        // Listen for these events.
        Events.OnEvaluateStep += OnEvaluateStep;
        Events.OnDecisionShow += OnDecisionShow;
        Events.OnDecisionFinished += OnDecisionFinished;

        // Initialize our decision.
        SpendBudgetDecision.Init();

        LoggerInstance.Msg("Initialized.");
    }

    public void OnEvaluateStep(object sender, EventArgs e)
    {
        // Forward to SpendBudgetDecision.
        SpendBudgetDecision.OnEvaluateStep();
    }

    public void OnDecisionShow(object sender, EventArgs e)
    {
        // Forward to SpendBudgetDecision.
        SpendBudgetDecision.OnDecisionShow();
    }

    public void OnDecisionFinished(object sender, Events.DecisionFinishedEventArgs e)
    {
        // Forward to SpendBudgetDecision.
        SpendBudgetDecision.OnDecisionFinished(e.SelectedOptionInfo);
    }
}