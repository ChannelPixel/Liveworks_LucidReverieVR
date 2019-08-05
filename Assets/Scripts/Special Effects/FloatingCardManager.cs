using UnityEngine;

public class FloatingCardManager : MonoBehaviour
{
    [SerializeField] private int timeUntilFly;
    [SerializeField] private GameObject cardPack;
    public void FloatAndRotateCards()
    {
        gameObject.SetActive(true);
        cardPack.SetActive(true);

        foreach (Transform child in transform)
        {
            child.GetComponent<FloatingCard>().StartFloatAndRot();
        }

        Invoke("FlyCardsToPack", timeUntilFly);
    }

    private void FlyCardsToPack()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<FloatingCard>().StartFlyToCardPack();
        }
    }


}
