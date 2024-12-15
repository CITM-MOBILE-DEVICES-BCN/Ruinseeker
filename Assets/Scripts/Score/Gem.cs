using UnityEngine;

public class Gem : Collectable
{
    protected override void OnCollect()
    {
        ScoreManagerRuin.Instance.AddGems(value);
        base.OnCollect();
    }
}
