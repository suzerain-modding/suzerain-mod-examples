using MelonLoader;
using SuzerainModdingKit;

[assembly: MelonInfo(typeof(BillExample.Core), "Bill Example", "1.0.0", "Fluffyalien1422", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]

namespace BillExample;

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        // Listen for these events.
        Events.OnEvaluateStep += OnEvaluateStep;
        Events.OnBillSigned += OnBillSigned;
        Events.OnBillVetoed += OnBillVetoed;

        // Initialize our bill.
        SpendBudgetBill.Init();

        LoggerInstance.Msg("Initialized.");
    }

    public void OnEvaluateStep(object sender, EventArgs e)
    {
        // Forward to SpendBudgetBill.
        SpendBudgetBill.OnEvaluateStep();
    }

    public void OnBillSigned(object sender, Events.BillEventArgs e)
    {
        // Forward to SpendBudgetBill.
        SpendBudgetBill.OnBillSigned(e.BillName);
    }

    public void OnBillVetoed(object sender, Events.BillEventArgs e)
    {
        // Forward to SpendBudgetBill.
        SpendBudgetBill.OnBillVetoed(e.BillName);
    }
}