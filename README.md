# Parallel Script使用指南

![Image](https://github.com/EnderHorror/ParallelScript/blob/main/demo.gif)

```R
{speaker("服务员")}<link="Rainbow">{w(1)}Wow!{w(1)}</link>I <color=ff0000>love</color>card games!
名字标题设为"服务员"  标记为彩虹特效  等待1s                      标记颜色
```

## 一.前言
1. 为什么要使用剧本语言:因为游戏的很多行为和事件都是跟随剧本执行的,如果不使用剧本语言,会让写程序的人为许多琐碎的功能而操劳,用这个可以一定程度上分担一部分程序的工作
什么是剧本语言:剧本语言就是编剧可以直接在剧本里就可以直接控制游戏的行为,比如移动镜头,控制角色的表情,控制音效的播放等等
 
2. 原理是什么: 基于Lua实现脚本执行,基于TextMeshPro实现富文本文字特效
 
 
# 二.如何编写自己的脚本
所有文件统一放在在根目录ParalleScript_Data->StreamingAssets里面
Start为入口脚本

# 三.基本语法
1. 文件类型
Parallel Script脚本文件的标准扩展名为.r。您编写的Parallel Script的命名应遵循这个格式：
文件名.r
(用这个拓展名只是因为在Vscode里面打开好看一点)。
 
2. 语句
Parallel Script采用的是文本+{指令}的混合形式
比如:
```R
  不好!{shake(1)}
  {speaker("小明")}我们被袭击了
```
注意:{}块不支持嵌套
注意:不支持换行,所有语言必须在同一行,这就是为什么叫Parallel Script
3. 函数
所有的函数需要放到{}块中
调用方式 函数名(参数)
 
4. 变量
Parallel语言中一切都可以是“变量”，但是Parallel不提供保存某种数据或数据结构的类型。如果你需要存储一些变量或调用一些原生方法，您可以在Parallel的{}块内无缝地调用Lua接口。
 
5. 选择结构if
在{}块中使用
```R
{if(表达式) then 执行的语句 elseif(表达式) 
then 执行的语句 else 执行的语句}
```
注意:除了else 不需要then,其他的都需要
 
5. 多文件
使用load(path)函数加载一个脚本并且运行 

## 四.支持的html标签

[官方文档](http://digitalnativestudios.com/textmeshpro/docs/rich-text/)

|Tags	| Summary|
|----|----|
|align |	文字对齐方式
|alpha, color |	颜色和透明度
|b, i |	粗体和斜体
|cspace |	文字行间距
|font |	字体
|indent	| 缩进
|line-height |	行高
|line-indent |	行缩进
|link |	文本元数据
|lowercase, uppercase, smallcaps |	字母大小写化
|margin |	盒子模型Margin
|mark|	标记文字
|mspace|	单个字符间距
|noparse|	不转义
|nobr|	文字不会被Warpping
|page|	Page break.
|pos|	水平位置
|size|	字体大小
|space|	空格长度
|sprite|	插入图片
|s, u	| 划掉线和下划线
|style|	自定义styles.
|sub, sup	|S上标和下标
|voffset	| 高度偏移
|width	|区域文字宽度


目前支持的文字特效:
```xml
<link="Wave"></link> //文字摇摆
<link="Rainbow"></link> //文字颜色彩虹
```

## 五.目前提供的API函数
>void w(float t)
 t:前一个字符延迟t秒

>void speaker(string name)
name:改变游戏内对话框里面的名字

>void branch(parms string[] choise)
在屏幕上显示多个选项供玩家选择
choise可选选项,多参数

>void print(string s)
s:要log的内容

>void load(string path)
加载并执行一个脚本
path:打开的脚本的相对路径

>void messagebox(string n)
在屏幕中央显示一个消息框

>void character(string path,string side)
设置角色在画面中的进场/退场,以及位置
Path:角色的文件名字 如果为””空字符串则表示退场
Side:在那个位置 为枚举类型 可以为”Right” “Left” “Mid”

>void background(string path)
切换背景
path:背景的名称
目前可用背景


>void switchBGM(string name)
平滑切换BGM
Name:要切换的BGM的名字 如何为””空字符则代表停止当前BGM

目前可用选择的BGM

>void shake(float intensity)
摇晃屏幕
Intensity:摇晃强度
 
## 六.添加自己的Lua函数

利用LuaCenter里的Register函数可以注册

## 七.自定义文字特效
只需要创建一个新类然后实现ITextEffect接口
然后文本里Link="类名即可"
