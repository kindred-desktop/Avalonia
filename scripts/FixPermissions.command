#!/bin/bash
echo "========================================"
echo "  SimpleApp 权限修复工具"
echo "========================================"
echo ""
echo "请在弹出的对话框中选择包含 SimpleApp.app 的文件夹"
echo ""

osascript -e 'tell app "Finder" to activate' -e 'set chosen_folder to POSIX path of (choose folder with prompt "选择包含 SimpleApp.app 的文件夹:")' 2>/dev/null

if [ -z "$chosen_folder" ]; then
    echo "已取消"
    exit 0
fi

APP_FILE="$chosen_folder/SimpleApp.app"

if [ ! -d "$APP_FILE" ]; then
    echo "未找到 SimpleApp.app"
    exit 1
fi

xattr -rd com.apple.quarantine "$APP_FILE" 2>/dev/null
chmod +x "$APP_FILE/Contents/MacOS/SimpleApp" 2>/dev/null
echo "完成！"
open "$APP_FILE"
