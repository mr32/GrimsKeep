using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleCard : BoardTarget
{
    public override CardTypes CardType => CardTypes.OBSTACLE;
    public ObstacleCard() : base()
    {
        cardOwner = PlayTypes.NEUTRAL;
    }
}
