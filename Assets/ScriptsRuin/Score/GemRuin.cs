using UnityEngine;

public class GemRuin : CollectableRuin
{
    protected override void OnCollect()
    {
        ScoreManagerRuin.Instance.AddGems(value);
        base.OnCollect();
    }
}
