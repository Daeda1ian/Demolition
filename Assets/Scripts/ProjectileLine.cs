using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour {

    static public ProjectileLine S;  //одиночка
    [Header("Set in Inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    void Awake() {
        S = this;                              //установить ссылку на объект-одиночку
        line = GetComponent<LineRenderer>();   //получить ссылку на LineRenderer
        line.enabled = false;                  //выключить LineRenderer, пока он не понадобится
        points = new List<Vector3>();          //инициализировать список точек
    }
    public GameObject poi {                    //это свойство (то есть метод, маскирующийся под поле)
        get {
            return (_poi);
        }
        set {
            _poi = value;
            if (_poi != null) {                 //если поле _poi содержит действительную ссылку
                line.enabled = false;          //сбросить все остальные параметры в исходное состояние
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear() {                      //этот метод можно вызвать непосредственно, чтобы стереть линию
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint() {                   //вызывается для добавления точки в линию
        Vector3 pt = _poi.transform.position;
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist) {  //если точка недостачно далека от предыдущей, просто выйти
            return;
        }
        if (points.Count == 0) {                                     //если это точка запуска
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS;
            //добавить дополнительный фрагмент линии, чтобы помочь лучше прицелиться в будущем
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            line.SetPosition(0, points[0]);          //установить первые две точки
            line.SetPosition(1, points[1]);
            line.enabled = true;                     //включить LineRenderer
        }
        else {                                  //обычная последовательность добавления точки
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    public Vector3 lastPoint {                     //возвращает местоположение последней добавленной точки
        get {
            if (points == null) {
                return (Vector3.zero);
            }
            return (points[points.Count - 1]);
        }
    }
    void Update() {
        if (poi == null) {                         //если свойство poi содержит пустое значение, найти интересующий объект
            if (FollowCam.POI != null) {
                if (FollowCam.POI.tag == "Projectile") {
                    poi = FollowCam.POI;
                }
                else {
                    return;                       //выйти, если интересующий объект не найден
                }
            }
            else {
                return;                           //выйти, если интересующий объект не найден
            }
        }
        //если интересующий объект найден, попытаться добавить точку с его координатами в каждом FixedUpdate
        AddPoint();
        if (FollowCam.POI == null) {
            poi = null;
        }
    }
}
