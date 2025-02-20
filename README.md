# DesktopDNS

一款桌面型DNS服务，可用于DNS分流、DNS解析等。

# 常见问题

** Linux下如何监听53端口？**

在Linux下，53端口属于特权端口，需要手动赋予程序 CAP_NET_BIND_SERVICE 能力，参考以下命令：

```shell
sudo setcap 'cap_net_bind_service=+ep' /path/to/your_program
```