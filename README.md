﻿# AutoLiveRecorder
本程序是直播录播软件，原理为扒流，通过极小的占用完成录播工作，无需打开直播页面也无需留守。目前只支持B站直播。

#### 当前版本：
1.0.2.16 Release。

#### 下载地址：http://update.zhangbudademao.com/112/AutoLiveRecorder.exe

代码中FLV前缀的类均为FLV文件处理，但这个版本没有用到。原因是扒流得到的flv文件最后一个Tag不完整无法自动合并。

#### 已知问题：

1. 录制获得的flv文件的最后一个tag不完整，对应的部分可能在播放器中存在无法控制的问题。（可通过转格式的方式解决）

2. 界面很丑，这个。。。后面有空再重做GUI吧。。。

3. 录制预位模式下突然被切到轮播有一定几率引发录制事件，暂未定位到原因。
