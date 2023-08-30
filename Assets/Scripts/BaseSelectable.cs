using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UnitSelectionState
{
    NONE,
    SELECTED
}

public class BaseSelectable : MonoBehaviour
{
    [SerializeField] GameObject _Selected2D;

    private GameObject _Active2D;

    public int Team;

    private bool _Movable;

    protected int Health;
    protected int Damage;

    protected System.Action OnClick;

    public bool IsMovable()
    {
        return _Movable;
    }

    public virtual void Setup(int teamNumber,int health,int damage)
    {
        Team = teamNumber;
        Health = health;
        Damage = damage;

        if (DebugHelper.Instance.UseTeamNumberOverride)
        {
            Team = DebugHelper.Instance.TeamNumberOverride;
        }
    }

    public virtual void Awake()
    {
        _Selected2D.SetActive(false);
    }

    public void SetSelectionState(UnitSelectionState uss,bool singleSelect)
    {
        if (_Active2D)
        {
            _Active2D.SetActive(false);
        }

        switch (uss)
        {
            case UnitSelectionState.SELECTED:
                _Active2D = _Selected2D;
                if(singleSelect)
                {
                    OnClick?.Invoke();
                }
                break;
            case UnitSelectionState.NONE:
                return;
        }

        if (_Active2D)
        {
            _Active2D.SetActive(true);
        }
    }

    public void TakeDamage(int amt)
    {
        Health -= amt;
        if(Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
