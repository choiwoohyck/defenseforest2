using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnumManager : MonoBehaviour
{
    // Start is called before the first frame update

   

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}


namespace EnumManagerSpace
{

    public enum ElementType
    {
        FIRE,
        ICE,
        LEAF,
        STONE,
        ICEROAD,
        LEAFROAD
    };

    public enum MonsterType
    {
        ZOMBIE,
        FRANKSTEIN
    };

    public enum EnemyState
    {
        SEARCH,
        RUN,
        ATTACK,
        DIE,
        READY
    };

    public enum EffectType
    {
        BULLET1HIT,
        BULLET1HITWALL,
        FIREELEMENTHIT,
        FIREELEMENTEXPLOSION,
        ICICLE,
        LEAFATTACK,
        GATHERINGENERGY
    };

    public enum OwnerType
    {
        PLAYER,
        FIRELEMENT,
        ICEELEMENT,
        ROADICEELEMENT,
        LEAFELEMENT,
        ROADLEAFELEMENT,
        ENEMY
    }
}