using SuzerainModdingKit;
using SuzerainModdingKit.Character;
using SuzerainModdingKit.StoryFragments.Conversation;
using SuzerainModdingKit.StoryFragments.Conversation.NodeSelectors;
using SuzerainModdingKit.StoryPack;
using SuzerainModdingKit.Utils;

namespace ConversationExample;

public static class ExampleConversation
{
    /// <summary>
    /// Variable used to check if the player has seen the conversation.
    /// </summary>
    public const string SeenVar = "ConversationExample.SeenExampleConversation";

    /// <summary>
    /// The data for the story fragment.
    /// </summary>
    public static readonly CustomConversationFragment Fragment = new(
        name: "ConversationExample.ExampleConversationFragment",
        conversationName: "ConversationExample.ExampleConversation",
        storyPack: SuzerainStoryPackInfo.Sordland,
        assignedTokenName: SuzerainTokenName.SordlandCityHolsord,
        hubTitle: "Example Conversation",
        hubDescription: "An example conversation.",
        type: Il2Cpp.ConversationData.ConversationType.Personal);

    /// <summary>
    /// Initialize this conversation. Should be called in OnInitializeMelon.
    /// </summary>
    internal static void Init()
    {
        // Register variables.
        Variables.Register(SeenVar);

        // Register the custom conversation.
        ConversationRegistry.RegisterConversation(Fragment.ConversationName);

        // The conversation has been registered, but it is empty.
        // Use a ConversationInjection to inject nodes into it.
        new ConversationInjection(Fragment.ConversationName)
            .AddNode(new("FirstLine",
                text: "This is the first line, spoken by the narrator.",
                speakerSelector: new CharacterNameSelector("Narrator"),
                // We have to hook the node to another node so SMK knows where to put it.
                hooks: [
                    // All conversations contain an empty START node.
                    // Use ConversationStartNodeSelector to select that node.
                    new(new ConversationStartNodeSelector())
                ],
                nextNodes: [
                    new ConversationNodeModdedNameSelector("DeanaPapaYoureHome"),
                ]))
            // The following dialogue is copied from 'Sordland/Turn02/Personal_FamilyDinner'.
            .AddNode(new("DeanaPapaYoureHome",
                text: "Papa! You're home!",
                speakerSelector: new CharacterNameSelector("Deana"),
                nextNodes: [
                    new ConversationNodeModdedNameSelector("PlayerTheresMyPrincess"),
                    new ConversationNodeModdedNameSelector("PlayerTheresMyDaughter"),
                    new ConversationNodeModdedNameSelector("PlayerLiftAndHug"),
                ]))
            .AddNode(new("PlayerTheresMyPrincess",
                text: "There's my beautiful princess!",
                nextNodes: [
                    new ConversationNodeModdedNameSelector("End"),
                ]))
            .AddNode(new("PlayerTheresMyDaughter",
                text: "There's my bright little daughter!",
                nextNodes: [
                    new ConversationNodeModdedNameSelector("End"),
                ]))
            .AddNode(new("PlayerLiftAndHug",
                text: "I lifted Deana up and hugged her while trying to put the keys in the tray next to the entrance.",
                menuText: "Lift and hug her.",
                speakerSelector: new CharacterNameSelector("Player_Italic"),
                nextNodes: [
                    new ConversationNodeModdedNameSelector("End"),
                ]))
            // Add an end node that sets 'ConversationExample.SeenExampleConversation' to true.
            .AddNode(new("End",
                luaScript: $"Variable['{SeenVar}'] = true;",
                // Add a continue sequence. Continue automatically continues, so it will
                // trigger the Lua script but the player will not need to manually press continue.
                // This node also has no text, so the player will not see it.
                sequence: new ConversationNodeSequenceBuilder()
                    .Continue()
                    .ToString(),
                // We need to explicitly set the speaker, otherwise
                // it will show up as a choice for the player.
                speakerSelector: new CharacterNameSelector("Narrator")))
            // Register the injection.
            .Register();
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

        // Return if the decision has already been added.
        if (GameState.StoryFragmentExistsInCurrentStep(Fragment.Name))
        {
            return;
        }

        // Return if the user has already seen the conversation.
        if (Variables.GetBool(SeenVar))
        {
            return;
        }

        // Add the story fragment to the game. We don't care about the return value, discard it.
        _ = GameState.AddCustomStoryFragment(Fragment);
    }
}