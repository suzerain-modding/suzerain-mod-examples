using MelonLoader;
using SuzerainModdingKit;

[assembly: MelonInfo(typeof(ConversationExample.Core), "Conversation Example", "1.0.0", "Fluffyalien1422", null)]
[assembly: MelonGame("Torpor Games", "Suzerain")]

namespace ConversationExample;

internal sealed class Core : MelonMod
{
    public override void OnInitializeMelon()
    {
        // Listen for the OnEvaluateStep event.
        Events.OnEvaluateStep += OnEvaluateStep;

        // Initialize our conversation.
        ExampleConversation.Init();

        LoggerInstance.Msg("Initialized.");
    }

    public void OnEvaluateStep(object sender, EventArgs e)
    {
        // Forward to ExampleConversation.
        ExampleConversation.OnEvaluateStep();
    }
}