# 简答 & 程序验证

- 游戏对象运动的本质是什么？

  是游戏对象的位置属性在每个时刻的变化。

  ```cs
  // 示例代码：沿y轴向上匀速运动
  // 打开项目文件夹下Object Motion以查看完整代码
  void Update () {
    this.transform.position += Vector3.up * Time.deltaTime;
  }
  ```

- 请用三种方法以上方法，实现物体的抛物线运动。

  1. 直接修改Transform.position方法

      简单来说，每一帧通过获取间隔时间Time.deltaTime，来根据抛物线函数计算下一帧物体的位置，由此达成物体的抛物线运动。

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
  1. 通过Rigidbody借由物理引擎实现
  1.

- 写一个程序，实现一个完整的太阳系， 其他星球围绕太阳的转速必须不一样，且不在一个法平面上。