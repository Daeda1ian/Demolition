using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour {

    [Header("Set in Inspector")]
    public GameObject cloudSphere;
    public int numSpheresMin = 6;   //����������� � ������������ ���������� ����������� ����������� CloudSphere
    public int numSpheresMax = 10;
    public Vector3 sphereOffsetScale = new Vector3(5, 2, 1);  //������������ ���������� (������������� ��� �������������) CloudSphere �� ������ Cloud ����� ������ ���
    public Vector2 sphereScaleRangeX = new Vector2(4, 8);     //�������� �������� ��� ������ ���
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);     //�� ��������� ��������� ����������, ������ ������� ������ ������
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public float scaleYMin = 2f;                         //���������� ��������� ������� �� ��� Y (����� �������� ��������� ������� ������ �������)
    private List<GameObject> spheres;                    //������ ��� ���������� �������
    void Start() {
        spheres = new List<GameObject>();
        int num = Random.Range(numSpheresMin, numSpheresMax);  //���������� ��������� ���������� ����������� CloudSphere
        for (int i = 0; i < num; i++) {
            GameObject sp = Instantiate<GameObject>(cloudSphere);  //������� ���������� ������� 
            spheres.Add(sp);                                       //� ��������� � ������
            Transform spTrans = sp.transform;                      //�������� transform ������� ���������� CloudSphere ������������� spTrans
            spTrans.SetParent(this.transform);                     //������ ������� ������ ������� �������� � ������� Cloud, �� ���� � ������� �������

            //������� ��������� ��������������
            Vector3 offset = Random.insideUnitSphere;    //���������� ��������� ����� ������ ��������� ����� (����� � �������� 1, � � ������� � ������ ��������� [0, 0, 0])
            offset.x *= sphereOffsetScale.x;             //������ ���������� ���� ����� ���������� �� ��������������� �������� sphereOffsetScale
            offset.y *= sphereOffsetScale.y;
            offset.z *= sphereOffsetScale.z;
            spTrans.localPosition = offset;              //�������� CloudSphere.transform.localPosition ������������� �������� offset

            //������� ��������� �������
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeY.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeZ.y);

            //��������������� ������� � �� ���������� � �� ������
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            spTrans.localScale = scale;                  //������� ������ ������������ ������������ ������������� ���������� Transform, ������� ��� ���� transform.scale
        }
    }
    void Update() {
        //if(Input.GetKeyDown(KeyCode.Space)) {
        //    Restart();
        //}
    }

    void Restart() {
        foreach (GameObject sp in spheres) {   //������� ������ �����, ������������ ������
            Destroy(sp);
        }
        Start();
    }
}
