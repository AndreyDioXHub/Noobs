using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gun", menuName = "Guns/Create New Gun")]
public class GunType : ScriptableObject
{
    public string Name;

    public float BulletSpeed;
    public List<int> MagazineSize = new List<int>(4);
    public int BulletPerShoot;
    public float FireRate; //shoot per second
    public float ReloadingTime;

    public float Damage;
    public float HeadMultiplier;
    public float LegMultiplier;

    public float BulletSpred;
    public List<Vector2> BulletPattern;

}
