using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour {

    [Header("Set in Inspector")]
    public int numClouds = 40;                               //число облаков
    public GameObject cloudPrefab;                           //шаблон дл€ облаков
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;                          //минимальный масштаб каждого облака
    public float cloudScaleMax = 3;                          //максимальный масштаб каждого облака
    public float cloudSpeedMult = 0.5f;                     //коэффициент скорости облаков

    private GameObject[] cloudInstances;

    void Awake() {
        cloudInstances = new GameObject[numClouds];  //создать массив дл€ хранени€ всех экземпл€ров облаков
        GameObject anchor = GameObject.Find("CloudAnchor");  //найти родительский игровой объект CloudAnchor
        GameObject cloud;
        for (int i = 0; i < numClouds; i++) {                    //создать в цикле заданное количество облаков
            cloud = Instantiate<GameObject>(cloudPrefab);        //создать экземпл€р cloudPrefab
            //выбрать местоположение дл€ облака
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //масштабировать облако
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);

            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);   //меньшие облака (с меньшим значением scaleU) должны быть ближе к земле
            cPos.z = 100 - 90 * scaleU;                           //меньшие облака должны быть дальше
            //применить полученные значени€ координат и масштаба к облаку
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            cloud.transform.SetParent(anchor.transform);          //сделать облако дочерним по отношению к anchor
            cloudInstances[i] = cloud;                            //добавить облако в массив
        }
    }
    void Update() {
        foreach (GameObject cloud in cloudInstances) {            //обойти в цикле все созданные облака
            float scaleVal = cloud.transform.localScale.x;        //получить масштаб
            Vector3 cPos = cloud.transform.position;              //и координаты облака
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult; //увеличить скорость дл€ ближних облаков
            if (cPos.x <= cloudPosMin.x) {                        //если облако сместилось слишком далеко влево
                cPos.x = cloudPosMax.x;                           //то переместить его вправо
            }
            cloud.transform.position = cPos;                      //применить новые координаты к облаку
        }
    }
}
