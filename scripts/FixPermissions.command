#!/bin/bash
echo "========================================"
echo "  SimpleApp 权限修复工具"
echo "========================================"
echo ""
echo "请在弹出的对话框中选择包含 SimpleApp.app 的文件夹"
echo "(通常是下载文件夹中的 SimpleApp-osx-arm64 文件夹)"
echo ""

osascript -e 'tell app "Finder" to activate' -e 'set chosen_folder to POSIX path of (choose folder with prompt "选择包含 SimpleApp.app 的文件夹:")' 2>/dev/null

if [ -z "$chosen_folder" ]; then
    echo "已取消操作"
    read -p "按 Enter 键退出..."
    exit 0
fi

APP_FILE="$chosen_folder/SimpleApp.app"

if [ ! -d "$APP_FILE" ]; then
    echo "❌ 在所选文件夹中未找到 SimpleApp.app"
    read -p "按 Enter 键退出..."
    exit 1
fi

APP_NAME=$(basename "$APP_FILE")
echo "✅ 找到应用: $APP_NAME"
echo "📁 位置: $chosen_folder"
echo ""
echo "🔧 正在移除安全限制..."
xattr -rd com.apple.quarantine "$APP_FILE" 2>/dev/null
echo "✅ 安全限制已移除"
echo ""
echo "🔧 正在设置执行权限..."
chmod +x "$APP_FILE/Contents/MacOS/SimpleApp" 2>/dev/null
echo "✅ 执行权限已设置"
echo ""
echo "========================================"
echo "✅ 权限修复完成!"
echo "========================================"
echo ""
echo "即将启动应用..."
read -p "按 Enter 键启动，或直接关闭此窗口..."

open "$APP_FILE"
