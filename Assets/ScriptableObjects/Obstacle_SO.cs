using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Obstacle_SO : ScriptableObject
{
    [SerializeField] private ObstacleSettings _positive;
    [SerializeField] private ObstacleSettings _neutral;
    [SerializeField] private ObstacleSettings _hazardous;
    [SerializeField] private ObstacleSettings _fixed;

    public void BuildObject(Obstacle obstacle)
    {
        switch (obstacle.ObstacleType)
        {
            case ObstacleType.Positive:
                SetPositive(obstacle);
                break;
            case ObstacleType.Harzadous:
                SetHazardous(obstacle);
                break;
            case ObstacleType.Neutral:
                SetNeutral(obstacle);
                break;
            case ObstacleType.Fixed:
                SetFixed(obstacle);
                break;
        }
    }

    private void SetPositive(Obstacle obstacle)
    {
        obstacle._rb.isKinematic = false;
        obstacle._collisionBox.material = _positive._collisionMaterial;
        obstacle._renderer.material = _positive._material;
        obstacle._rb.mass = _positive._mass;
        obstacle._collisionBlocker.enabled = false;

        SetLayer(obstacle.gameObject, 0);
        SetTag(obstacle.gameObject, "Obstacle");
        obstacle.gameObject.name = "Positive";
    }

    private void SetHazardous(Obstacle obstacle)
    {
        obstacle._rb.isKinematic = false;
        obstacle._collisionBox.material = _hazardous._collisionMaterial;
        obstacle._renderer.material = _hazardous._material;
        obstacle._rb.mass = _hazardous._mass;
        obstacle._collisionBlocker.enabled = false;

        SetLayer(obstacle.gameObject, 0);
        SetTag(obstacle.gameObject, "Hazardous");
        obstacle.gameObject.name = "Hazardous";
    }

    private void SetNeutral(Obstacle obstacle)
    {
        obstacle._rb.isKinematic = false;
        obstacle._collisionBox.material = _neutral._collisionMaterial;
        obstacle._renderer.material = _neutral._material;
        obstacle._rb.mass = _neutral._mass;
        obstacle._collisionBlocker.enabled = true;

        SetLayer(obstacle.gameObject, LayerMask.NameToLayer("Neutral"));
        SetTag(obstacle.gameObject, "Obstacle");
        obstacle.gameObject.name = "Neutral";

    }

    private void SetFixed(Obstacle obstacle)
    {
        obstacle._collisionBox.material = _fixed._collisionMaterial;
        obstacle._renderer.material = _fixed._material;
        obstacle._rb.isKinematic = true;

        SetLayer(obstacle.gameObject, 0);
        SetTag(obstacle.gameObject, "Obstacle");
        obstacle.gameObject.name = "Fixed";

    }

    private void SetTag(GameObject gameObject ,string tag)
    {
        foreach (Transform t in gameObject.transform)
            t.gameObject.tag = tag;
        gameObject.tag = tag;
    }

    private void SetLayer(GameObject gameObject, int layer)
    {
        foreach (Transform t in gameObject.transform)
            t.gameObject.layer = layer;
        gameObject.layer = layer;
    }

    [System.Serializable]
    private class ObstacleSettings
    {
        public Material _material;
        public PhysicMaterial _collisionMaterial;
        public float _mass;
    }
}

public enum ObstacleType
{
    Positive,
    Harzadous,
    Neutral,
    Fixed
}