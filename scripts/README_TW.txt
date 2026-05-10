SimpleApp 安装说明
====================

【重要】首次运行需要修复权限，请按以下步骤操作：

第一步：打开终端
   按 Command + Space 键，输入 "终端"，按回车

第二步：复制以下命令（直接全选复制即可）
   xattr -rd com.apple.quarantine

第三步：将 SimpleApp.app 文件夹拖入终端窗口
   终端中会自动填充完整路径

第四步：按回车执行

第五步：关闭终端，双击 SimpleApp.app 即可运行

---

如果遇到问题，在终端中运行：
   sudo xattr -rd com.apple.quarantine /Applications/SimpleApp.app

然后输入你的电脑密码即可。
