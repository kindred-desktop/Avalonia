#!/bin/bash

set -e

PROJECT_DIR="$(cd "$(dirname "$0")" && pwd)"
PUBLISH_DIR="$PROJECT_DIR/bin/Release/publish"
VERSION="1.0.0"
TEMP_DIR="$PUBLISH_DIR/osx-temp"

echo "=========================================="
echo "SimpleApp 发布脚本"
echo "版本: $VERSION"
echo "=========================================="

echo ""
echo "正在构建 macOS 版本..."
dotnet publish -c Release -r osx-x64 --self-contained true -o "$TEMP_DIR"

echo ""
echo "正在创建 macOS 应用包..."
rm -rf "$PUBLISH_DIR/SimpleApp.app"
mkdir -p "$PUBLISH_DIR/SimpleApp.app/Contents/MacOS"
mkdir -p "$PUBLISH_DIR/SimpleApp.app/Contents/Resources"

cp "$TEMP_DIR/SimpleApp" "$PUBLISH_DIR/SimpleApp.app/Contents/MacOS/"
cp -R "$TEMP_DIR/"* "$PUBLISH_DIR/SimpleApp.app/Contents/MacOS/"

cat > "$PUBLISH_DIR/SimpleApp.app/Contents/Info.plist" << 'PLISTEOF'
<?xml version="1.0" encoding="UTF-8"?>
<!DOCTYPE plist PUBLIC "-//Apple//DTD PLIST 1.0//EN" "http://www.apple.com/DTDs/PropertyList-1.0.dtd">
<plist version="1.0">
<dict>
    <key>CFBundleExecutable</key>
    <string>SimpleApp</string>
    <key>CFBundleIdentifier</key>
    <string>com.simpleapp</string>
    <key>CFBundleName</key>
    <string>SimpleApp</string>
    <key>CFBundleDisplayName</key>
    <string>SimpleApp</string>
    <key>CFBundlePackageType</key>
    <string>APPL</string>
    <key>CFBundleShortVersionString</key>
    <string>1.0.0</string>
    <key>CFBundleVersion</key>
    <string>1</string>
    <key>LSMinimumSystemVersion</key>
    <string>10.13</string>
    <key>NSHighResolutionCapable</key>
    <true/>
    <key>NSPrincipalClass</key>
    <string>NSApplication</string>
</dict>
</plist>
PLISTEOF

chmod +x "$PUBLISH_DIR/SimpleApp.app/Contents/MacOS/SimpleApp"

rm -rf "$TEMP_DIR"

echo ""
echo "正在构建 Windows 版本..."
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o "$PUBLISH_DIR/win-x64"

echo ""
echo "正在构建 Linux 版本..."
dotnet publish -c Release -r linux-x64 --self-contained true -p:PublishSingleFile=true -o "$PUBLISH_DIR/linux-x64"

echo ""
echo "=========================================="
echo "发布完成！输出目录: $PUBLISH_DIR"
echo "=========================================="
echo ""
echo "各平台文件:"
echo "  macOS: $PUBLISH_DIR/SimpleApp.app (应用包，双击运行)"
echo "  Windows: $PUBLISH_DIR/win-x64/SimpleApp.exe"
echo "  Linux: $PUBLISH_DIR/linux-x64/SimpleApp"
echo ""
