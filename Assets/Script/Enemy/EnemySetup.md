# Enemy系统设置指南

## 概述
本指南将帮助您正确设置Enemy系统，避免NullReferenceException错误。

## 修复内容

### ✅ 已修复的问题
1. **EnemyAttack.cs中的NullReferenceException**
   - 移除了对`EnemyController.instance`的静态引用
   - 改为通过组件引用来获取EnemyController
   - 添加了空值检查和错误处理

2. **EnemyController.cs中的静态实例问题**
   - 移除了静态`instance`变量
   - 每个Enemy对象现在有独立的控制器

## 正确的Enemy设置步骤

### 1. Enemy GameObject设置
```
Enemy (GameObject)
├── EnemyController (Script)
├── EnemyAttack (Script) - 可选，用于攻击检测
├── Animator (Component)
├── NavMeshAgent (Component)
├── Collider (Component)
└── Rigidbody (Component) - 可选
```

### 2. EnemyController组件设置
- **Move Speed**: 移动速度
- **Distance To Chase**: 追击距离
- **Distance To Lose**: 丢失目标距离
- **Distance To Stop**: 停止距离
- **Keep Chasing Time**: 持续追击时间
- **Enable Patrol**: 是否启用巡逻
- **Patrol Point A/B**: 巡逻点（如果启用巡逻）

### 3. EnemyAttack组件设置（可选）
- **Attack Damage**: 攻击伤害
- **Player Tag**: 玩家标签（默认为"Player"）

### 4. 攻击检测设置
如果使用EnemyAttack脚本进行攻击检测：

1. **创建攻击触发器**：
   - 在Enemy下创建子对象，命名为"AttackTrigger"
   - 添加Collider组件，设置为Trigger
   - 添加EnemyAttack脚本

2. **设置碰撞层**：
   - 确保AttackTrigger只与Player层碰撞
   - 在Project Settings → Physics中设置Layer Collision Matrix

## 常见问题解决

### 1. NullReferenceException错误
**原因**：EnemyAttack脚本在初始化时无法找到EnemyController
**解决**：
- 确保EnemyAttack和EnemyController在同一个GameObject上
- 或者EnemyAttack在EnemyController的子对象上

### 2. 攻击不触发
**原因**：碰撞检测设置不正确
**解决**：
- 检查AttackTrigger的Collider设置
- 确认Player对象有正确的Tag
- 检查Layer Collision Matrix设置

### 3. 多个Enemy冲突
**原因**：使用了静态实例
**解决**：
- ✅ 已修复：现在每个Enemy都有独立的控制器
- 确保每个Enemy对象都有独立的EnemyController组件

## 测试方法

### 使用EnemyTest脚本
1. 将`EnemyTest.cs`添加到场景中的空GameObject
2. 在Inspector中勾选"Find Enemies"按钮
3. 查看Console输出，确认所有Enemy都被正确识别

### 手动测试
1. 运行游戏
2. 让Player接近Enemy
3. 观察Enemy是否开始追击
4. 检查攻击是否正常触发

## 性能优化建议

1. **对象池**：对于大量Enemy，考虑使用对象池
2. **LOD系统**：为远处的Enemy使用简化的AI
3. **NavMesh优化**：合理设置NavMesh的分辨率和区域

## 扩展功能

### 添加Enemy类型
1. 继承EnemyController创建新的Enemy类型
2. 重写攻击方法实现不同的攻击模式
3. 添加特殊技能和状态效果

### 添加Enemy状态系统
1. 实现状态机管理Enemy的不同状态
2. 添加受伤、眩晕、狂暴等状态
3. 实现状态转换逻辑

## 调试技巧

1. **使用Gizmos**：在Scene视图中显示Enemy的检测范围
2. **Console日志**：添加详细的调试信息
3. **Inspector调试**：在运行时查看Enemy的状态变量 