using Valve.VR.InteractionSystem;

namespace ATT.Interaction.PuzzleSystem
{
    public class QueenOfHearts : PuzzleObjectBase
    {
        protected override void HandleAttachedToHand(Hand hand)
        {
            if(!isComplete)
            {
                SetPuzzleObjectiveComplete();
            }
        }
    }
}