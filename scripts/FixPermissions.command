#!/bin/bash
echo "========================================"
echo "  SimpleApp 权限修复工具"
echo "========================================"
echo ""

# 获取脚本所在目录
SCRIPT_DIR="$( cd "$( dirname "$0" )" && pwd )"

# 查找 .app 文件
APP_FILE=$(find "$SCRIPT_DIR" -maxdepth 1 -name "*.app" -type d 2>/dev/null | head -1)

if [ -z "$APP_FILE" ]; then
    echo "❌ 未找到 .app 文件"
    echo "请确保此脚本与 SimpleApp.app 在同一目录下"
    read -p "按 Enter 键退出..."
    exit 1
fi

APP_NAME=$(basename "$APP_FILE")
echo "✅ 找到应用: $APP_NAME"
echo ""

# 移除 quarantine 属性
echo "🔧 正在移除安全限制..."
xattr -rd com.apple.quarantine "$APP_FILE" 2>/dev/null

# 设置执行权限
echo "🔧 正在设置执行权限..."
chmod +x "$APP_FILE/Contents/MacOS/SimpleApp" 2>/dev/null

echo ""
echo "========================================"
echo "✅ 权限修复完成!"
echo "========================================"
echo ""
echo "现在可以运行应用了"
echo ""
read -p "按 Enter 键启动应用，或直接关闭此窗口..."

# 尝试打开应用
open "$APP_FILE"
