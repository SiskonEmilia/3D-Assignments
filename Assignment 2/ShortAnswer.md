# 简答 & 程序验证

注意：本简答题作业所有代码／实现／实验均包含在SolarSystem项目中，由不同的场景呈现，通过选择不同的场景，您可以快速查看各题目实现效果

- 游戏对象运动的本质是什么？

  是游戏对象的位置属性在每个时刻的变化。

  ```cs
  // 示例代码：沿y轴向上匀速运动
  // 打开项目文件夹下Object Motion以查看完整代码
  void Update () {
    this.transform.position += Vector3.up * Time.deltaTime;
  }
  ```

- 请用三种方法以上方法，实现物体的抛物线运动。（在Parabola文件夹下三个场景分别对应三种方法的实现）

  1. 直接对 Transform.position 赋值

      简单来说，每一帧通过获取间隔时间Time.deltaTime，来根据抛物线函数计算下一帧物体的位置，将其赋值给Transform.position，由此达成物体的抛物线运动。

      ```cs
      public class parabola1 : MonoBehaviour {

        static private float g = 2.38F;
        private float speedx;
        private float speedy;
        private float x;
        private float y;

        // Use this for initialization
        void Start () {
          speedx = 1.5F;
          speedy = 4;
        }

        // Update is called once per frame
        void Update () {
          float newSpeedy = speedy - g * Time.deltaTime;
          x = this.transform.position.x + speedx * Time.deltaTime;
          y = (speedy + newSpeedy) / 2 * Time.deltaTime + this.transform.position.y;
          if (y <= 0) {
            y = -y;
            newSpeedy = -newSpeedy;
          }
          speedy = newSpeedy;
          this.transform.position = new Vector3(
            x, y, 0.0F
          );
        }
      }
      ```
  1. 通过 Rigidbody 借由物理引擎实现

      Rigidbody（刚体）组件可以让对象受Unity3D的物理引擎控制。因而，在开启重力影响后，赋予物体初速度，就可以看到抛物线运动的效果。

      ```cs
      [RequireComponent(typeof(Rigidbody))]
      public class Parabola2 : MonoBehaviour {

        private Rigidbody rigidbody;
        private Vector3 initSpeed;

        // Use this for initialization
        void Start () {
          rigidbody = this.GetComponent<Rigidbody> ();
          initSpeed = new Vector3 (3, 10, 0);
          rigidbody.velocity = initSpeed;
        }

        // Update is called once per frame
        void Update () {

        }
      }
      ```

  1. 通过 CharacterController 通过函数实现

      CharacterController（角色控制器）是用于第一人称／第三人称角色的碰撞模型，免去了繁琐的刚体设置，并提供了Simple Move和Move两种方法，因为Simple Move会屏蔽y方向速度，因此此处使用Move并结合函数计算速度方向来实现抛物线运动。

      ```cs
      [RequireComponent(typeof(CharacterController))]
      public class Parabola3 : MonoBehaviour {

        private CharacterController charactercontroller;
        private Vector3 speed;

        // Use this for initialization
        void Start () {
          charactercontroller = this.GetComponent<CharacterController> ();
          speed = new Vector3 (2, 5, 0);
        }

        // Update is called once per frame
        void Update () {
          speed -= 3.0F * Time.deltaTime * Vector3.up;
          charactercontroller.Move (speed * Time.deltaTime);
        }
      }
      ```

- 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。

  既然要做太阳系，为了较为真实，自然要用到太阳系的数据～

  |星球名|半径(公里)|轨道半径(千万公里)|自转周期(天)|公转周期(天)
  |-|-|-|-|-|
  |太阳 Sun|696000|0|26.9|0
  |水星 Mercury|2439|57.9|58.65|87.70
  |金星 Venus|6052|108.2|243.01|224.70
  |地球 Earth|6378|149.6|0.9973|365.26
  |火星 Mars|3398|227.9|1.0260|686.98
  |木星 Jupiter|71398|778.3|0.410|4332.71
  |土星 Saturn|60330|1427.0|0.426|10759.5
  |天王星 Uranus|25400|2882.3|0.646|30685
  |海王星 Neptune|24600|4523.9|0.658|60190
  |冥王星 Pluto|1500|5917.1|6.39|90800

  不过，这些数据过于庞大，经过一些奇妙的处理（归一化）之后使用会更加好看。不过不管表面上的东西怎么变，核心代码始终是不变的。

  ```cs
  using System.Collections;
  using System.Collections.Generic;
  using UnityEngine;

  public class SolarSystem : MonoBehaviour {

    public Transform Sun;
    public Transform Mercury;
    public Transform Venus;
    public Transform Earth;
    public Transform Moon;
    public Transform Mars;
    public Transform Jupiter;
    public Transform Saturn;
    public Transform Uranus;
    public Transform Neptune;
    public Transform Pluto;
    private float speed = 5;

    // Use this for initialization
    void Start () {
      
    }
    
    // Update is called once per frame
    void Update () {
      Sun.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 30);
      Mercury.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 30);
      Venus.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 80);
      Earth.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 20);
      Moon.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 30);
      Mars.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 20);
      Jupiter.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 10);
      Saturn.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 10);
      Uranus.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 10);
      Neptune.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 10);
      Pluto.Rotate (speed * Vector3.up * 360 * Time.deltaTime / 15);

      Mercury.RotateAround (Sun.transform.position, Vector3.up + 0.1F * Vector3.left, speed * 360 * Time.deltaTime / 87);
      Venus.RotateAround (Sun.transform.position, Vector3.up - 0.05F * Vector3.left, speed * 360 * Time.deltaTime / 224);
      Earth.RotateAround (Sun.transform.position, Vector3.up + 0.13F * Vector3.left, speed * 360 * Time.deltaTime / 365);
      Moon.RotateAround (Earth.transform.position, Vector3.up + 0.2F * Vector3.left, speed * 360 * Time.deltaTime / 30);
      Mars.RotateAround (Sun.transform.position, Vector3.up - 0.18F * Vector3.left, speed * 360 * Time.deltaTime / 687);
      Jupiter.RotateAround (Sun.transform.position, Vector3.up + 0.09F * Vector3.left, speed * 360 * Time.deltaTime / 1000);
      Saturn.RotateAround (Sun.transform.position, Vector3.up - 0.21F * Vector3.left, speed * 360 * Time.deltaTime / 1300);
      Uranus.RotateAround (Sun.transform.position, Vector3.up + 0.1F * Vector3.left, speed * 360 * Time.deltaTime / 1500);
      Neptune.RotateAround (Sun.transform.position, Vector3.up + 0.2F * Vector3.left, speed * 360 * Time.deltaTime / 1800);
      Pluto.RotateAround (Sun.transform.position, Vector3.up + 0.15F * Vector3.left, speed * 360 * Time.deltaTime / 2000);


    }
  }
  ```
