using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class MenuManager
{
    [SerializeField] private List<Menu> _menus = new List<Menu>();

    /// <summary>
    /// Set the return/start values of all animated objects
    /// </summary>
    public void InitLoopbacks()
    {
        foreach (Menu m in _menus)
            m._menuAnimation.InitLoopbackpoint();
    }

    public void ActivateMenu(int index)
    {
        for (int i = 0; i < _menus.Count; i++)
        {
            if (i != index)
            {
                _menus[i]._menu.SetActive(false);
                continue;
            }

            _menus[i]._menu.SetActive(true);
            _menus[i]._menuAnimation.PlayAnimation();
        }
    }

    public void DeactivateAll()
    {
        foreach (var item in _menus)
            item._menu.SetActive(false);
    }

    /// <summary>
    /// Returns true if given menu-index is active
    /// </summary>
    public bool IsMenuActive(int menuIndex)
    {
        if (_menus[menuIndex]._menu.activeSelf)
            return true;
        return false;
    }
}

[System.Serializable]
public class Menu
{
    public string _name;
    public GameObject _menu;
    public AnimationSequence _menuAnimation;
}

[System.Serializable]
public class AnimationSequence
{
    [SerializeField] private List<Animation> _animations = new List<Animation>();

    public void PlayAnimation()
    {
        foreach (var animation in _animations)
            animation.Animate();
    }

    public void InitLoopbackpoint()
    {
        foreach (Animation animation in _animations)
            animation.SetLoopbackPoints();
    }
}

[System.Serializable]
public class Animation
{
    public string _name;
    [SerializeField] private float _lenght;
    [SerializeField] private float _delay;

    //All Transforms go through all AnimationKeys
    [SerializeField] private List<Transform> _toAnimate = new List<Transform>();
    [SerializeField] private List<AnimationKeys> _animationKeys = new List<AnimationKeys>();

    public void Animate()
    {
        ResetAnimations();
        foreach (AnimationKeys key in _animationKeys)
        {
            int index = 0;
            foreach (var transform in _toAnimate)
                AnimateKey(transform, key, index++);
        }
    }

    private void AnimateKey(Transform transform, AnimationKeys key, int index, bool isLoopback = false)
    {
        Vector3 transition = isLoopback ? key._loopBackPoint[index] : key._transition;
        Action loopBack = () => { if (key._loopBack && !isLoopback) AnimateKey(transform, key, index, true); };

        switch (key._type)
        {
            case AnimationType.scale:
                transform.
                    DOScale(transition, _lenght).
                    SetEase(key._ease).
                    SetDelay(_delay).OnComplete(() => { loopBack(); });
                break;
            case AnimationType.rotate:
                transform.
                    DORotate(transition, _lenght).
                    SetEase(key._ease).
                    SetDelay(_delay).OnComplete(() => { loopBack(); });
                break;
            case AnimationType.move:
                transform.
                    DOMove(transition, _lenght).
                    SetEase(key._ease).
                    SetDelay(_delay).OnComplete(() => { loopBack(); });
                break;
            case AnimationType.alpha:
                Color color = isLoopback ? Color.clear : Color.black;
                transform.GetComponent<Image>().
                    DOColor(color, _lenght).
                    SetEase(key._ease).
                    SetDelay(_delay).OnComplete(() => { loopBack(); });
                break;
        }
    }

    private void ResetAnimations()
    {
        foreach (AnimationKeys key in _animationKeys)
        {
            int index = 0;
            foreach (var transform in _toAnimate)
                ResetAnimateKey(transform, key, index++);
        }
    }

    private void ResetAnimateKey(Transform transform, AnimationKeys key, int index)
    {
        switch (key._type)
        {
            case AnimationType.scale:
                transform.localScale = key._loopBackPoint[index];
                break;
            case AnimationType.rotate:
                transform.eulerAngles = key._loopBackPoint[index];
                break;
            case AnimationType.move:
                transform.position = key._loopBackPoint[index];
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Retrieve start value of all animated transforms and set as LoopBackPoint
    /// </summary>
    public void SetLoopbackPoints()
    {
        for (int i = 0; i < _animationKeys.Count; i++)
        {
            foreach (Transform item in _toAnimate)
            {
                switch (_animationKeys[i]._type)
                {
                    case AnimationType.scale:
                        _animationKeys[i]._loopBackPoint.Add(item.localScale);
                        break;
                    case AnimationType.rotate:
                        _animationKeys[i]._loopBackPoint.Add(item.eulerAngles);
                        break;
                    case AnimationType.move:
                        _animationKeys[i]._loopBackPoint.Add(item.position);
                        break;
                    case AnimationType.alpha:
                        _animationKeys[i]._loopBackPoint.Add(Vector3.zero);
                        break;
                }
            }
        }
    }


    [System.Serializable]
    private class AnimationKeys
    {
        public AnimationType _type;
        public Vector3 _transition;
        public Ease _ease;
        public bool _loopBack;

        //Each animated transform has it's own loopback point
        [HideInInspector] public List<Vector3> _loopBackPoint = new List<Vector3>();
    }

    [System.Serializable]
    private enum AnimationType
    {
        scale = 0,
        rotate,
        move,
        alpha,
    }
}

