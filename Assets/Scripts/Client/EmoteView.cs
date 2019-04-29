using UnityEngine;

public class EmoteView : MonoBehaviour
{
    public Animator Animator;
    public GameObject Emote;

    public MeshRenderer MeshRenderer;

    public Material EmoteAngry;
    public Material EmoteBuying;

    public void ShowEmote(EmoteType type)
    {
        Material material = null;

        switch (type) {
            case EmoteType.Angry:
                material = EmoteAngry;
                break;
            case EmoteType.Buying:
                material = EmoteBuying;
                break;
            default:
                break;
        }

        MeshRenderer.material = material;
        Animator.SetTrigger("Show");
    }
}

public enum EmoteType
{
    Buying,
    Angry
}