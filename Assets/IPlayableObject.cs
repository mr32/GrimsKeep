using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayableObject
{
    List<Card> CardsPlayedOnObject { get; }
}
