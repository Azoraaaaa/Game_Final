# Boss战斗系统使用说明

## 概述
这是一个完整的海底怪物Boss战斗系统，包含三个阶段、多种攻击方式和完整的动画控制。

## 脚本文件说明

### 1. BossController.cs - 主控制器
- **功能**: 管理Boss的HP、阶段切换、动画控制和攻击逻辑
- **主要特性**:
  - 三阶段战斗系统（HP > 75%, 75% ≥ HP > 30%, HP ≤ 30%）
  - 自动巡逻和攻击AI
  - 多种攻击模式（近战、远程、特殊攻击）
  - 使用OverlapBox进行碰撞检测
- **公共属性**:
  - `CurrentHP`: 当前血量
  - `MaxHP`: 最大血量
  - `IsDead`: 是否死亡
  - `ProjectileSpawnPoint`: 弹体生成点

### 2. Projectile.cs - 弹体系统
- **功能**: 处理水团和冰团的移动、碰撞和特效
- **特性**:
  - 水团：缓慢移动，命中时生成水花
  - 冰团：较慢移动，命中时爆炸并减速玩家
- **枚举**: `ProjectileType` (Water, Ice)

### 3. BossAttackTrigger.cs - 攻击触发器
- **功能**: 处理特定攻击部位的碰撞检测
- **支持的攻击类型**: 爪子攻击、突刺攻击、海浪冲击

### 4. BossAnimationEvents.cs - 动画事件处理器
- **功能**: 在动画播放时触发攻击检测和特效
- **使用方法**: 在Animator中设置动画事件调用相应方法

### 5. BossSystemTest.cs - 测试脚本
- **功能**: 验证Boss系统是否正常工作
- **测试按键**:
  - F1: 对Boss造成伤害
  - F2: 对玩家造成伤害
  - F3: 检查Boss阶段

### 6. CompilationTest.cs - 编译测试脚本
- **功能**: 验证所有组件引用是否正确
- **测试内容**: ProjectileType枚举、BossController、GameManager

### 7. BossDeathTest.cs - 死亡测试脚本
- **功能**: 专门测试Boss死亡功能
- **测试按键**:
  - F1: 对Boss造成100点伤害
  - F2: 对Boss造成500点伤害（快速死亡）
  - F3: 检查Boss状态
  - F4: 重置Boss（如果可能）

### 8. BossHealthBarSimple.cs - Boss血条UI（简化版）
- **功能**: 实时显示Boss血量、阶段和状态
- **特性**: 血条颜色渐变、阶段自动检测、死亡状态显示
- **依赖**: 无外部依赖，简单易用

### 10. PlayerHealth.cs - 玩家健康系统
- **功能**: 接收Boss攻击的伤害，处理受击效果

### 11. PlayerMovement.cs - 玩家移动系统
- **功能**: 处理玩家移动和减速效果

### 12. GameManager.cs - 游戏管理器
- **功能**: 管理游戏状态、UI显示和胜负判定
- **修复内容**:
  - 更新过时的`FindObjectOfType`为`FindFirstObjectByType`
  - 修复单例模式属性名称
  - 添加缺失的公共属性

## 错误修复记录

### 已修复的问题：
1. **ProjectileType未定义错误**
   - 原因：Projectile.cs文件被清空，BossController中引用ProjectileType时没有正确指定命名空间
   - 解决：重新创建Projectile.cs文件，包含完整的ProjectileType枚举，并修复BossController中的引用为`Projectile.ProjectileType`

2. **FindObjectOfType过时警告**
   - 原因：Unity 2022.3+中FindObjectOfType已被弃用
   - 解决：替换为FindFirstObjectByType

3. **GameManager属性缺失错误**
   - 原因：其他脚本引用GameManager中不存在的属性
   - 解决：添加缺失的公共属性（player, isNearTeleporter, discoveredLocations）

4. **反射访问私有字段**
   - 原因：GameManager使用反射访问BossController的私有字段
   - 解决：为BossController添加公共属性，直接访问

5. **Boss死亡时卡住问题**
   - 原因：死亡时Update方法直接返回，协程未停止，动画状态未正确设置
   - 解决：添加死亡动画更新方法，停止所有协程，禁用攻击触发器

6. **Boss血条UI系统**
   - 功能：实时显示Boss血量、阶段和状态
   - 特性：跟随Boss移动、阶段变化动画、淡入淡出效果
   - 提供两个版本：完整版（需要LeanTween）和简化版（无依赖）

## 设置步骤

### 1. Boss模型设置
1. 将Boss模型添加到场景中
2. 添加Animator组件并设置动画控制器
3. 添加BossController脚本
4. 设置所有必要的引用（动画器、玩家、攻击部位等）

### 2. 动画控制器设置
确保Animator Controller包含以下参数：
- `IsMoving` (bool) - Boss是否在移动
- `IsAttacking` (bool) - 是否正在攻击
- `AttackIndex` (int) - 当前攻击动作编号
- `Phase` (int) - Boss当前阶段（1、2、3）
- `IsDead` (bool) - Boss是否死亡
- `GetHit` (trigger) - 受击触发
- `DoSpecialAttack` (trigger) - 特殊攻击触发

### 3. 攻击触发器设置
1. 在Boss的爪子、突刺等攻击部位添加子对象
2. 为每个攻击部位添加BossAttackTrigger脚本
3. 设置攻击类型和伤害值
4. 添加碰撞器并设置为触发器

### 4. 动画事件设置
在Animator中为相关动画添加事件：
- 攻击开始/结束事件
- 特效生成事件
- 音效播放事件

### 5. 弹体预制体设置
1. 创建水团和冰团预制体
2. 添加Projectile脚本
3. 设置碰撞器和触发器
4. 配置特效和音效

### 6. 测试设置
1. 添加BossSystemTest脚本到场景中的空对象
2. 运行游戏并查看Console输出
3. 使用F1-F3按键测试功能

## 动画片段对应关系

### 攻击类动画
- `ClawAttackLeft` - 左爪攻击
- `ClawAttackRight` - 右爪攻击
- `Claws2HitCombo` - 双爪连击
- `ClawsStinger3HitCombo` - 爪刺三连击
- `StingerAttack` - 突刺攻击

### 爬行类动画
- `CrawlForward` - 向前爬行
- `CrawlBackwards` - 向后爬行
- `CrawlLeft` - 向左爬行
- `CrawlRight` - 向右爬行
- `CrawlForwardAttack` - 向前爬行攻击
- `CrawlLeftAttack` - 向左爬行攻击
- `CrawlRightAttack` - 向右爬行攻击

### 其他动画
- `Idle` - 待机
- `GetHit1` - 受击1
- `GetHit2` - 受击2
- `Death` - 死亡

## 阶段特性

### 第一阶段（HP > 75%）
- 缓慢巡逻（CrawlForward、CrawlLeft、CrawlRight）
- 偶尔使用ClawAttackLeft或ClawAttackRight进行近战攻击

### 第二阶段（75% ≥ HP > 30%）
- 使用Claws2HitCombo和ClawsStinger3HitCombo进行连击
- 发射冰团（Ice Orb）造成范围爆炸和减速
- 发射水团（Water Orb）远程攻击
- 使用StingerAttack突刺攻击

### 第三阶段（HP ≤ 30%）
- 使用CrawlBackwards撤退后发动海浪冲击（Tidal Rush）
- 攻击频率提升
- HP为0时播放Death动画并生成水花特效

## 注意事项

1. **标签设置**: 确保玩家对象有"Player"标签
2. **碰撞器设置**: 所有攻击触发器必须设置为触发器模式
3. **音效和特效**: 所有音效和粒子特效都标记为TODO，需要根据项目需求实现
4. **性能优化**: 使用对象池管理弹体和特效以提高性能
5. **调试**: 使用Gizmos可视化攻击范围和检测区域
6. **测试**: 使用BossSystemTest脚本验证系统功能

## 扩展建议

1. 添加更多攻击模式
2. 实现Boss的弱点系统
3. 添加环境交互效果
4. 实现多人战斗支持
5. 添加Boss的AI行为树
6. 实现Boss的装备掉落系统 