# Parallel Script使用指南

## 一.前言

1. 为什么要使用剧本语言:因为游戏的很多行为和事件都是跟随剧本执行的,如果不使用剧本语言,会让写程序的人为许多琐碎的功能而操劳,用这个可以一定程度上分担一部分程序的工作
什么是剧本语言:剧本语言就是编剧可以直接在剧本里就可以直接控制游戏的行为,比如移动镜头,控制角色的表情,控制音效的播放等等
 
2. 原理是什么: Parallel Script是我专为编写剧本而开发的一种指令式的程序语言，它的语法比较特殊。Parallel Script不可以单独作为一段程序执行，它是一种顶层语言，必须由其他语言作为载体，目前是使用C#和Lua作为载体
 

 
## 二.如何编写自己的脚本

所有文件统一放在在根目录ParalleScript_Data->StreamingAssets里面
 Start.r为入口脚本
ParalleScript.exe为主程序

异常:如果游戏过程中出现一个MessageBox,就说明编写的脚本有问题


## 三.基本语法

1. 文件类型
Parallel Script脚本文件的标准扩展名为.r。您编写的Parallel Script的命名应遵循这个格式：
文件名.r
(用这个拓展名只是因为在Vscode里面打开好看一点)。
 
2. 语句
Parallel Script采用的是文本+{指令}的混合形式
比如:
  不好!{shake(1)}
  {speaker("小明")}我们被袭击了
注意:{}块不支持嵌套
注意:不支持换行,所有语言必须在同一行,这就是为什么叫Parallel Script
3. 函数
所有的函数需要放到{}块中
调用方式 函数名(参数)
 
4. 变量
Parallel语言中一切都可以是“变量”，但是Parallel不提供保存某种数据或数据结构的类型。如果你需要存储一些变量或调用一些原生方法，您可以在Parallel的{}块内无缝地调用Lua接口。
 
5.选择结构if
在{}块中使用
{if(表达式) then 执行的语句 elseif(表达式) then 执行的语句 else 执行的语句}
注意:除了else 不需要then,其他的都需要
 
5.多文件
使用load(path)函数加载一个脚本并且运行 
 
# 四.目前提供的API函数

void w(float t)
 t:前一个字符延迟t秒
 
void speaker(string name)
name:改变游戏内对话框里面的名字

void branch(parms string[] choise)
在屏幕上显示多个选项供玩家选择
choise可选选项,多参数

void print(string s)
s:要log的内容

void load(string path)
加载并执行一个脚本
path:打开的脚本的相对路径

void messagebox(string n)
在屏幕中央显示一个消息框

void character(string path,string side)
设置角色在画面中的进场/退场,以及位置
Path:角色的文件名字 如果为””空字符串则表示退场
Side:在那个位置 为枚举类型 可以为”Right” “Left” “Mid”

目前可以选择的character有2个人

void background(string path)
切换背景
path:背景的名称
目前可选背景如下

void switchBGM(string name)
平滑切换BGM
Name:要切换的BGM的名字 如何为””空字符则代表停止当前BGM
目前可选BGM

void shake(float intensity)
摇晃屏幕
Intensity:摇晃强度
