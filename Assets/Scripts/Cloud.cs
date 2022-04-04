using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    [Header("Set in Inspector")]
    public GameObject cloudSphere;
    public int numSpheresMin = 6;   //минимальное и максимальное количество создаваемых экземпл€ров CloudSphere
    public int numSpheresMax = 10;
    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);  //максимальное рассто€ние (положительное или отрицательное) CloudSphere от центра Cloud вдоль каждой оси
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);     //диапазон масштаба дл€ каждой оси
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);     //по умолчанию создаютс€ экземпл€ры, ширина которых больше высоты
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public float scaleYMin = 2f;                         //наименьший возможный масштаб по оси Y (чтобы избежать по€влени€ слишком тонких облаков)
    private List<GameObject> spheres;                    //хранит все экземпл€ры облаков
    void Start() {
        spheres = new List<GameObject>();
        int num = Random.Range(numSpheresMin, numSpheresMax);  //выбираетс€ случайное количество экземпл€ров CloudSphere
        for (int i = 0; i < num; i++) {
            GameObject sp = Instantiate<GameObject>(cloudSphere);  //создаем экземпл€ры облаков 
            spheres.Add(sp);                                       //и добавл€ем в список
            Transform spTrans = sp.transform;                      //свойство transform каждого экземпл€ра CloudSphere присваиваетс€ spTrans
            spTrans.SetParent(this.transform);                     //каждый кусочек облака сделать дочерним к объекту Cloud, то есть к данному объекту

            //выбрать случайное местоположение
            Vector3 offset = Random.insideUnitSphere;    //выбираетс€ случайна€ точка внутри единичной сферы (сферы с радиусом 1, и с центром в начале координат [0, 0, 0])
            offset.x *= sphereOffsetScale.x;             //кажда€ координата этой точки умножаетс€ на соответствующее значение sphereOffsetScale
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            spTrans.localPosition = offset;              //свойству CloudSphere.transform.localPosition присваиваетс€ смещение offset

            //выбрать случайный масштаб
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            //скорректировать масштаб у по рассто€нию х от центра
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            spTrans.localScale = scale;                  //масштаб всегда определ€етс€ относительно родительского компонента Transform, поэтому нет пол€ transform.scale
        }
    }
    void Update() {
        //if(Input.GetKeyDown(KeyCode.Space)) {
        //    Restart();
        //}
    }

    void Restart() {
        foreach (GameObject sp in spheres) {   //удалить старые сферы, составл€ющие облако
            Destroy(sp);
        }
        Start();
    }
}
