本次大作业中，我选择的主题是制作一款简单的[坦克大战](https://so.csdn.net/so/search?q=坦克大战&spm=1001.2101.3001.7020)小游戏，实现此项目借鉴并使用了BiliBili教程及资源和学姐的博，利用Unity自带的3D导航技术实现敌人坦克的自动导航。

演示视频：[Unity大作业-坦克大战_演示](https://www.bilibili.com/video/BV1BP411F76P/?vd_source=4bf91179b97450a67d87e468e26f9978)

项目地址：[坦克大战](https://github.com/hn666666/unity)

参考视频：[坦克大战教程](https://www.bilibili.com/video/BV1ua4y1L7oY/?spm_id_from=333.337.search-card.all.click&vd_source=4bf91179b97450a67d87e468e26f9978)

学姐博客：[第15周-坦克](https://yuandi-sherry.github.io/2018/06/19/Week15-Tanks/)

***



[TOC]

## 游戏设计

- 坦克大战，必须包含3D导航技术（Enemy NPC使用unity的NavMeshAgent自动导航）
- 坦克分为Player和Enemy（用两套逻辑脚本控制行为）。Player通过键盘输入控制移动和发射子弹，Enemy使用AI导航移动和利用协程发射子弹
- 子弹伤害设置
- 血条控制
- 相机视野跟随
- 美化（爆炸使用粒子特效，音效)

## 构建场景

首先准备游戏资源，在这

https://pan.baidu.com/s/1UXRHwSMr5DMDoPOCuv6beg 
提取码：ksv7

构建的地图像这样![image-20230107174504132](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107174301274.png)

## 具体实现

### Player 坦克

#### Tank_Movement.cs (移动)

![image-20230107174504132](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107174504132.png)

```
public class Tank_Movement : MonoBehaviour
{
    public float speed = 5;//坦克移动速度
    public float angularSpeed = 10;//坦克旋转速度
    public AudioClip idleAudio;//静止声音
    public AudioClip drivingAudio;//发动声音
    private new AudioSource audio;//声音组件
    private new Rigidbody rigidbody;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody = this.GetComponent<Rigidbody>();//获得刚体组件
        audio = this.GetComponent<AudioSource>();//获得声音组件
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");//对应键盘上下箭头，按下触发
        rigidbody.velocity = transform.forward*v*speed;//利用刚体加速度

        float h = Input.GetAxis("Horizontal");//对应键盘左右箭头，按下触发
        rigidbody.angularVelocity = transform.up*h*angularSpeed;

        if(Mathf.Abs(h) > 0.1||Mathf.Abs(v)>0.1){
            audio.clip = drivingAudio;
            if(audio.isPlaying==false)
                audio.Play();
        }
        else{
            audio.clip = idleAudio;
            if(audio.isPlaying==false)
                audio.Play();
        }
    }
}
```

#### Tank_Attack.cs (攻击)

![image-20230107180247181](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107180247181.png)

小技巧：在坦克上添加emptygameobject 作为发射点(在坦克上绑定位置)

transform.Find()  在**当前transform层级**找子项

```
public class Tank_Attack : MonoBehaviour
{
    public GameObject shellPrefab;//子弹预制体
    public KeyCode fireKey = KeyCode.Space;//发射子弹键盘按键
    public float shellSpeed = 10;//子弹发射速度
    public AudioClip shotAudio;
    private Transform firePoint;
    // Start is called before the first frame update
    void Start()
    {
        firePoint = transform.Find("FirePoint");//找到发射点
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(fireKey)){//空格键按下后
            AudioSource.PlayClipAtPoint(shotAudio, transform.position);
            GameObject go = GameObject.Instantiate(shellPrefab, firePoint.position, firePoint.rotation);//在发射点位置实例化子弹
            go.GetComponent<Rigidbody>().velocity = go.transform.forward*shellSpeed;
            go.name = "Player";
        }
    }
}
```

### Enemy 坦克

#### Npc_Movement.cs （使用NavMeshAgent 进行 AI 寻路）

先为gameobject添加NavMeshAgent component，再将参数调合适

![image-20230107181300296](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107181300296.png)

![image-20230107181505627](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107181505627.png)

3d导航很简单，只要将Enemy控件NavMeshAgent的目的地destination设置为Player position即可

```
 void Start()
    {
        sceneController = this.GetComponent<FirstController>();//获得
    }

    // Update is called once per frame
    void Update () {
        if(sceneController.getState()) {
            target = sceneController.getPlayerPosition();
            // 追踪玩家
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            agent.destination = target;
        
        }
        else {
            // 游戏结束，停止寻路
            UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent> ();
            agent.velocity = Vector3.zero;
            agent.ResetPath();
        }
    }
```

#### Npc_Attack.cs （利用协程发射子弹）

IEnumerator 本质上 是C#的迭代器。

IEnumerator 可以理解为一个函数对象的容器（函数对象1 函数对象2 .....）,通过使用yield关键字，将yield关键字部分的函数代码抽出并生成一个函数，放到IEnumerator容器中。

yield return null;//暂停一帧
yield return new WaitForSeconds(1f);//1秒后继续执行

IEnumerator 可以依次执行容器中的函数，其实就是将函数进行分块处理

协程中Whille循环可以理解成一种状态，这里我们用[协程](https://so.csdn.net/so/search?q=协程&spm=1001.2101.3001.7020)+while循环来做状态机

场景一运行就在等待某个指令，一旦指令到达，就开始触发某个事（Player进入攻击范围发射子弹）

StartCoroutine(shoot()); 开启协程

```
void Start()
    {
        firePoint = transform.Find("FirePoint");//找到发射点
        sceneController = this.GetComponent<FirstController>();
        StartCoroutine(shoot());
    }

    // Update is called once per frame
    void Update()
    {
        if(sceneController.player){
            target = sceneController.getPlayerPosition();
        }
    }

    IEnumerator shoot(){ //协程实现npc坦克射击
        while(sceneController.player){
            if(Vector3.Distance(transform.position, target) < 10) {
            Debug.Log("fire");
            yield return new WaitForSeconds(1);
            GameObject go = GameObject.Instantiate(shellPrefab, firePoint.position, firePoint.rotation);//在发射点位置实例化子弹
            go.GetComponent<Rigidbody>().velocity = go.transform.forward*shellSpeed;
            AudioSource.PlayClipAtPoint(shotAudio, transform.position);
            go.name = "NPC";
            yield return 0;
            // yield return null;
            // yield return new WaitForSeconds(1f);//1秒后继续执行for循环
            }
            yield return 0;
        }
    }
```

上面代码实际就是在满足发射条件情况下将发射子弹分割为多个帧执行，避免在while里面符合发射条件的短时间内一直发射

### Shell 子弹

#### Shell.cs  

将Shell.cs代码add到Shell预制体上

![image-20230107191935929](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107191935929.png)

![image-20230107191952706](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107191952706.png)

Shell、Player、Enemy都有刚体和碰撞体，Shell开启碰撞检测

![image-20230107192440951](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107192440951.png)

OnTriggerEnter(Collier collider) Shell碰撞到的物体作为参数传入 

xx.SendMessage("methodName")调用此游戏对象中的每个 [MonoBehaviour](https://docs.unity3d.com/cn/2019.4/ScriptReference/MonoBehaviour.html) 上名为 `methodName` 的方法

小技巧：collider.tag区分碰撞体的类别。我们设置Player游戏对象的tag为Playear, Enemy游戏对象的tag为Enemy![image-20230107192945452](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107192945452.png)

若是NPC发射的子弹碰撞到玩家，玩家减血；若是玩家发射的子弹碰撞到NPC，NPC减血

而NPC互相攻击不减血

```
    public void OnTriggerEnter( Collider collider){//子弹碰撞触发器
        AudioSource.PlayClipAtPoint(shellExplosionAudio, transform.position);
        GameObject.Instantiate(shellExplosionPrefab, transform.position, transform.rotation);//实例化爆炸效果
        GameObject.Destroy(this.gameObject);//删除子弹
        if(collider.tag == "Player"){//NPC发射子弹碰撞到玩家时
            collider.SendMessage("TankDamage");//给坦克发送消息 调用Tank里面函数
        }
        else if(this.name=="Player" && collider.tag=="Enemy"){//玩家发射子弹碰撞到NPC
            collider.SendMessage("TankDamage");
        }
    }
```

### Slider 血条

![image-20230107194014593](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107194014593.png)

#### 血量显示

通过以上步骤和资源将血条作为子对象绑定到tank对象上

![image-20230107194247170](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107194247170.png)

#### Tank_Health.cs (减少血量)

这个脚本挂在所有坦克上，hp表示当前血量，实现上一部分内容中子弹碰撞到坦克所调用的TankDamage()方法，将hp减小的同时要将绑定该脚本的游戏对象上的血条同步更新显示

```
bool TankDamage(){
        if(hp<=0)
            return false;
        hp-=Random.Range(10,20);
        hpSlider.value = (float)hp/hpTotal;//血条值范围0~1
        if(hp<=0){
            AudioSource.PlayClipAtPoint(tankExplosionAudio,transform.position);
            GameObject.Instantiate(tankExplosion, transform.position + Vector3.up, transform.rotation);
            GameObject.Destroy(this.gameObject);
            return true;
        }
        return false;
    }
```

### MainCamera（主摄）

#### FollowTarget.cs (相机视野跟随)

```
	void Start () {
		offset = transform.position - player.transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if(player)
		transform.position = player.transform.position + offset;
	}
```

### 美化部分

#### 粒子特效

##### 子弹爆炸

Shell.cs的OnTriggerEnter()里

```
GameObject.Instantiate(shellExplosionPrefab, transform.position, transform.rotation);//实例化爆炸效果
```

##### 坦克爆炸

Tank_Health的TankDamage()里

```
GameObject.Instantiate(tankExplosion, transform.position + Vector3.up, transform.rotation);
```

#### 音效

##### 背景音

Main场景创建空对象添加Audio Source组件

![image-20230107201012851](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107201012851.png)

##### 子弹爆炸音

Shell.cs的OnTriggerEnter()里

```
AudioSource.PlayClipAtPoint(shellExplosionAudio, transform.position);
```

![image-20230107201200793](https://github.com/hn666666/unity/blob/main/Blogger.assets/image-20230107201200793.png)

同理实现了坦克爆炸音和启动音，静止音

## 不足

时间紧没完成工厂模式生成和管理Enemy.pre和Shell.pre，导演场景模式也未完成，还有就是UI和玩法比较单一
