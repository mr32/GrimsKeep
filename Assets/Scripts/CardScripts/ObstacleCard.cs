using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleCard : BoardTarget
{
    public override CardTypes CardType => CardTypes.OBSTACLE;
    public override PlayTypes cardOwner { get => PlayTypes.NEUTRAL; set => base.cardOwner = value; }
}
