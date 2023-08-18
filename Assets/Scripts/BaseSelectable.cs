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

    public bool IsDropOffPoint;

    private GameObject _Active2D;

    private int _Team;

    private bool _Movable;

    private int _Health = 5;

    protected System.Action OnClick;

    public bool IsMovable()
    {
        return _Movable;
    }

    public virtual void Setup(int teamNumber)
    {
        _Team = teamNumber;
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
        Debug.Log("Take damage");
        _Health -= amt;
        if(_Health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
