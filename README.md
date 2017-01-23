# HiTCP_unity

haven't finished 



概述:
-------------
微软提供了很多接口测试当前系统/网络适配器支持哪种ip版本:


       Debug.Log(Socket.OSSupportsIPv4);//.net平台过高       
       Debug.Log(Socket.OSSupportsIPv6);//.net平台过高       
       Debug.Log(Socket.SupportsIPv4);       
       Debug.Log(Socket.SupportsIPv6);//微软标记过时api
       
我现在使用的Unity(5.3.4.f1)中mono使用的.net仍然是2.0.50727.1433(Environment.Version),第一和第二条按照msdn说明都是基于现有.net平台(.net4.5+),在unity中执行中肯定会异常,但是在调用的时候发现第一条异常,第二条执行正常,仔细查找mono兼容api发现:
[![](http://note.youdao.com/yws/public/resource/e5a82e19c36d60bd66f6b5ec40c50ae7/xmlnote/0D0F8D6AD2D34D118FA7E5F32BFB847D/7725)](http://note.youdao.com/yws/public/resource/e5a82e19c36d60bd66f6b5ec40c50ae7/xmlnote/0D0F8D6AD2D34D118FA7E5F32BFB847D/7725)

unity对第二第三第四都提供支持,唯独不支持第一条.第四条被标记成过时api,下面只说明第二第三条.
> **Tip:** 关于stackoverfollow中有人测试说第二条在android上测试异常看来是谬传了.
[![](http://note.youdao.com/yws/public/resource/e5a82e19c36d60bd66f6b5ec40c50ae7/xmlnote/529EB007D0564FFFB17530569CA1EB83/7727)](http://note.youdao.com/yws/public/resource/e5a82e19c36d60bd66f6b5ec40c50ae7/xmlnote/529EB007D0564FFFB17530569CA1EB83/7727)

按照接口声明,第二条和第三条在unity中正常使用,并非在android上抛出异常.
再说在unity中支持ipv6,官方说明:
[![](http://note.youdao.com/yws/public/resource/e5a82e19c36d60bd66f6b5ec40c50ae7/xmlnote/DD03AB53B6A34D99A703BB5219D16ADC/7730)](http://note.youdao.com/yws/public/resource/e5a82e19c36d60bd66f6b5ec40c50ae7/xmlnote/DD03AB53B6A34D99A703BB5219D16ADC/7730)

说的很明确,推荐域名,然后通过addressfamily选择合适的ipv4或ipv6,下面就通过tcpclient具体处理ipv6支持.

            if (Socket.OSSupportsIPv6)
                client = new TcpClient(AddressFamily.InterNetworkV6);
            else
                client = new TcpClient(AddressFamily.InterNetwork);




***********
**未完待续**
support:hiramtan@live.com
