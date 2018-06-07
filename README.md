# AutoLiveRecorder

[![Version](https://img.shields.io/github/release/LeoChen98/AutoLiveRecorder.svg?label=Version)](#)
[![GitHub issues](https://img.shields.io/github/issues/LeoChen98/AutoLiveRecorder.svg)](https://github.com/LeoChen98/AutoLiveRecorder/issues)
[![Language](https://img.shields.io/badge/%E8%AF%AD%E8%A8%80-%E4%B8%AD%E6%96%87-brightgreen.svg)](#)
[![DevLanguage](https://img.shields.io/badge/%E5%BC%80%E5%8F%91%E8%AF%AD%E8%A8%80-C%23-brightgreen.svg)](#)
[![.netVersion](https://img.shields.io/badge/.net-3.5-brightgreen.svg)](#)
[![Pull Request Welcome](https://img.shields.io/badge/Pull%20request-welcome-brightgreen.svg)](#)
[![GitHub license](https://img.shields.io/github/license/LeoChen98/AutoLiveRecorder.svg)](https://github.com/LeoChen98/AutoLiveRecorder/blob/master/LICENSE)

## 简述
* 本程序是直播录播软件，原理为扒流，通过极小的占用完成录播工作，无需打开直播页面也无需留守。目前只支持B站直播。
* 代码中FLV前缀的类均为FLV文件处理，但当前版本没有用到。原因是扒流得到的flv文件最后一个Tag不完整无法自动合并。
* 至于那堆只是修改readme的commits，原谅我强迫症。。。

## 安装和使用
* 本软件无需安装，下载（如果是含ffmpeg完整包需解压）后即可使用。
* 使用时设定房间号和保存路径即可开始录制。
* “录制预位”功能：激活后将待机等待直播间开播，开播后即开始录制。
* “自动转码”功能：将在录制结束（包含主播下播和用户手动停止）后将录制获得的flv流文件无损转码合并为mp4文件，以修复flv文件末尾部分播放器进度条失效的问题，在录制时间比较长时获得更小的文件。
    1. 转码结束会删除原有flv文件。
    2. 需要“ffmpeg.exe”。
    3. 不启用自动转码功能时，如果录制时有断流或者网络问题导致录制文件为多个时，软件会自动创建文件夹，并整理录制文件，文件名按时间顺序递增。

## 下载地址
[![Downloads](https://img.shields.io/badge/%E4%B8%8B%E8%BD%BD%E8%BD%AF%E4%BB%B6@1.0.2.16-35K-brightgreen.svg)](http://update.zhangbudademao.com/112/AutoLiveRecorder.exe)
[![DownloadsWithffmpeg](https://img.shields.io/badge/%E4%B8%8B%E8%BD%BD%E8%BD%AF%E4%BB%B6%EF%BC%88%E5%8C%85%E5%90%ABffmpeg%EF%BC%89@1.0.2.16-19.8M-brightgreen.svg)](http://downloads.zhangbudademao.com/software/112/AutoLiveRecorder_1_0_2_16_full.zip)
[![PassedDownloads](https://img.shields.io/badge/%E8%BF%87%E5%BE%80%E7%89%88%E6%9C%AC-release-blue.svg)](https://github.com/LeoChen98/AutoLiveRecorder/releases)  

## 开发计划
* 即将加入命令行启动功能，以便使用计划任务的方式启动。
* 预计下版本会使用WPF重构UI。
* 将基于PHP开发服务器拓展插件。

## 已知问题
1. 录制获得的flv文件的最后一个tag不完整，对应的部分可能在播放器中存在无法控制的问题。（可通过转格式的方式解决）
2. 界面很丑，这个。。。后面有空再重做GUI吧。。。
3. 录制预位模式下突然被切到轮播有一定几率引发录制事件，暂未定位到原因。

## 开放源代码许可
### ffmpeg v4.0
<http://ffmpeg.org/>

Copyright (c) 2000-2018 the FFmpeg developers

Licensed under LGPL v2.1 or later
