# 3DSurvivalGame

一个基于 Unity 的 3D 生存游戏练习项目。

## 项目简介

当前项目已包含基础的玩家移动、AI 移动、交互选中等核心脚本，适合继续迭代生存玩法（采集、背包、战斗、建造等）。

## 当前功能

- 玩家移动控制
- 鼠标视角控制
- 简单 AI 移动
- 射线选中交互物体并显示名称

## 主要目录

- Assets/Scripts: 游戏核心脚本
- Assets/Scenes: 场景文件
- ProjectSettings: Unity 项目设置
- Packages: Unity 包管理配置

## 如何运行

1. 使用 Unity Hub 打开项目根目录。
2. 等待依赖导入完成。
3. 打开 Assets/Scenes 中的主场景。
4. 点击 Play 运行。

## Git 说明

仓库已配置 .gitignore，用于忽略 Unity 自动生成的大体积目录（如 Library、Temp、Logs 等）。

## 后续计划（可选）

- 增加物品拾取与背包系统
- 增加敌对 AI 与生命值系统
- 增加存档/读档