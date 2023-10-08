<img src="https://github.com/liruoyu2008/HJ212Interpreter/blob/main/media/logo.png?raw=true" width="550" alt="HJ212 Interpreter" />

<h1>
   <a href="https://afdian.net/a/Roooyu">
    <img height="40" alt="Buy me a coffee" src="https://github.com/liruoyu2008/HJ212Interpreter/blob/main/media/coffee.svg?raw=true" />
   </a>
  <a href="https://raw.githubusercontent.com/liruoyu2008/HJ212Intepreter/main/LICENSE">
    <img src= "https://img.shields.io/badge/license-MIT-blue.svg" alt="MIT">
  </a>
</h1>




> HJ212 协议解析器

  1. [HJ212协议是什么?](#HJ212协议是什么) 
  2. [HJ212Interpreter是什么?](#HJ212Interpreter是什么)
  3. [使用方法](#我该如何使用它)
  4. [举例](#看几个简单的例子)
  5. [通过Nuget获取](#在Nuget中获取它)
 6. [Tips](#tips)
 7. [参与和贡献](#参与和贡献)


## HJ212协议是什么?

HJ212协议全称为《污染物在线监控（监测）系统数据传输标准》，是中国环保行业的数据通信协议。该协议有两个历史版本：HJ/T 212-2005和HJ 212-2017，后者已经替代前者成为新的当前标准。数据终端、采集终端、环保仪等终端设备把采集好的数据发送到环保平台都会使用这个协议。该协议实现了各种终端和平台之间的完美对接。只要符合该协议标准的设备和平台，都可以互联互通。

## HJ212Interpreter是什么?

HJ212Interpreter是一个库，它**将来自HJ212协议终端的ASCII字符串转换为对象**。

众所周知，拆字符串（Substring、Split什么的）是一件相当无聊的事情，所以，让HJ212Interpreter来为你做这些吧!

**[如果你是个谨慎的人，或者一个好奇宝宝，可以看看他们的文档。](https://github.com/liruoyu2008/HJ212Interpreter/blob/main/doc/污染物在线监控（监测）系统数据传输标准.pdf)**

## 我该如何使用它?

很简单，你给它一个**字符串**，它给你一个**对象**，就这样！（当然，反过来也行）

举个栗子，如果你是环境数据监控（监测）中心，你收到了一个通讯包：
``` c#
string message = "##0162QN=20231008123340461;ST=22;CN=2011;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&DataTime=20231008123340;w01008-SampleTime=20231008123340,w01008-Rtd=123.456&&F441\r\n";
```
这是**2011号命令**数据包，它表示数采仪向你**上传污染物实时数据**，你只需要这样：

```c#
var obj = Message.Parse(SourceType.Slave, message);
```

你就得到了一个消息对象！它将含有2011号命令中的各个**基础字段**，以及**CP指令字段**。

另外，若是注意到`Flag`的值，会发现这个消息是需要回复的。因此，反过来，你可以构造**请求响应命令**，即**9011号命令**，然后将他转换为ASCII通讯包，像这样:

```c#
            var obj9011 = new Message()
            {
                // SourceType.Master指示该消息的发送端为数据监控中心（主机端），SystemType.AIR_QUALITY_MONITORING指示系统编码为22（空气质量监测）
                // 最后的false指示该消息不需要接收端回复
                Command = new Command(SourceType.Master, SystemType.AIR_QUALITY_MONITORING, 9011, "123456", "010000A8900016F000169DC0", false)
                {
                    CP = new CP9011()
                    {
                        QnRtn = RequestResult.READY
                    }
                }
            };
			//str9011 = "##0094QN=20231008125115215;ST=22;CN=9011;PW=123456;MN=010000A8900016F000169DC0;Flag=4;CP=&&QnRtn=1&&CC01\r\n"
            var str9011 = obj9011.ToString();
```

当然，如果你熟悉了该协议的话，构造请求响应命令还可以这样进行：

```c#
            var obj9011 = new Message(message.Command.CreateRequestResponse());
```

这样你就不必重复填写源命令中繁琐的基础字段了。

## 看几个简单的例子：

收到数采仪开机时间:

``` c#
            // string msg2081 = "##0137QN=20231008130452757;ST=22;CN=2081;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&DataTime=20231008130452;RestartTime=20231007130452&&3AC0\r\n";
            Message obj2081 = Message.Parse(SourceType.Slave, msg2081);
            Command reqResp2081 = obj2081.Command.CreateRequestResponse();
            if (reqResp2081 != null)
            {
                Message resp2081Msg = new Message(reqResp2081);
                // 回复响应...
            }
            CP2081 cp = obj2081.Command.CP as CP2081;
            DateTime? time = cp.RestartTime;
            // do something...
```

取污染物实时数据：
``` c#
            Message obj2011 = new Message()
            {
                Command = new Command(SourceType.Master, SystemType.AIR_QUALITY_MONITORING, 2011, "123456", "010000A8900016F000169DC0", true)
                {
                    CP = new CP2011_Request()
                }
            };
            // do something...
```

收到污染物实时数据：

```c#
        // string msg2011 = "##0162QN=20231008132300648;ST=22;CN=2011;PW=123456;MN=010000A8900016F000169DC0;Flag=5;CP=&&DataTime=20231008132300;w01008-SampleTime=20231008132300,w01008-Rtd=123.456&&2800\r\n";
        Message obj2011_2 = Message.Parse(SourceType.Slave, msg2011);
        string resp2011Msg = obj2011_2.Command.CreateRequestResponse()?.ToMessageString();
        if (resp2011Msg != null)
        {
            // 回复响应...
        }
        CP2011_Upload cp2011 = obj2011_2.Command.CP as CP2011_Upload;
        string polid_0 = cp2011.SubCP.Keys.FirstOrDefault();
        SubCP_Rtd value0 = cp2011.SubCP[polid_0] as SubCP_Rtd;
        var polid_0_time = value0.SampleTime;
        var polid_0_Rtd = value0.Rtd;
        // do something...
```

## 在[Nuget](https://www.nuget.org/packages/HJ212Interpreter)中获取它！

```
Install-Package HJ212Interpreter
```

### Tips

> 假设，当你收到一条ASCII报文，报文内包含这样一个浮点参数**Rtd=1.230**，你将它解析为Message对象后，重新将该对象序列化为ASCII报文，那么，新的报文内，这个参数会变成**Rtd=1.23**，你懂的~
>
> 当然，大部分情况下，经过回转的消息与源消息是一致的，意外几乎只会出现解析数值参数时（字符串参数我么会原样保留）。所以，让我们祈祷源消息不会出现类似**1.230**或**0123**这样的数值吧！

> 好消息是，Message对象是内部一致的，即从字符串转换为对象后，如果因为以上问题出现了长度缩减，则Message对象的消息长度和CRC校验码会是新的对应值，而不是源消息内的值（应该总是重视实际逻辑值而不是字面值，对吗？）

> 在使用Command.UnPack()进行多个分包命令的解压缩时，得到的完整命令对应的Flag.D、PNUM、PNO参数将总是与自身对齐（而不是保持源ASCII报文中的字段）。

### 参与和贡献

这个项目的初衷就是希望将类似我这样的Coder们从低级的劳动中解脱出来，提高业务效率，

也能省出更多的时间和精力学习新的东西，或是纯粹的休息与充电。

如果你喜欢这个项目，可以来我的[Github](https://github.com/liruoyu2008/HJ212Interpreter)看看，我会认真处理每条debug和改良建议。

当然，如果你愿意更进一步，来[爱发电](https://afdian.net/a/Roooyu)看看吧，这将是对我最大的认可和鼓励！
