using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

    static public bool goalMet = false;
    void OnTriggerEnter(Collider other) {                   //когда в область действия триггера попадает что-то проверить, является ли это "что-то" снарядом
        if (other.gameObject.tag == "Projectile") {             //если это снаряд
            Goal.goalMet = true;                                //присвоить полю goalMet значение true
            Material mat = GetComponent<Renderer>().material;   //также изменить альфа-канал цвета, чтобы увеличить непрозрачность
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }

    void Start() {

    }
    void Update() {

    }
}
