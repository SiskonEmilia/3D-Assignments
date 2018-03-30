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