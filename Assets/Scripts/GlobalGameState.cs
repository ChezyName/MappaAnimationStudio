using UnityEngine;

public class GlobalGameState : MonoBehaviour
{
    private int Money;
    private static GlobalGameState GGS;
    
    private void Awake()
    {
        if (GGS != null && GGS != this)
        {
            Destroy(this.gameObject);
        } else {
            GGS = this;
            DontDestroyOnLoad(this);
        }
    }

    public static GlobalGameState getGameState()
    {
        return GGS;
    }

    public int getMoney()
    {
        return Money;
    }

    public void addMoney(int Dollars)
    {
        Money += Dollars;
    }
}