# 项目迁移至Gitee
https://gitee.com/low-code-dev-lab/lowcode-plugins-forguncy-retrieve-filed-from-http-head

# 活字格低代码开发平台插件：读取HTTP请求HEAD
## 功能简介
本插件提供了读取HTTP请求HEAD中特定字段的能力，通常用于系统集成场景。当第三方系统以HTTP请求的形式调用使用活字格开发的服务端命令时，开发者可以通过该插件将HTTP请求HEAD中的特定字段，如Authorization、From等，读取到变量；后面再根据该变量的值执行权限验证等后续工作。
## 使用方法
在安装有本插件的设计器中，开发者可以在服务端命令里找到【读取HTTP请求HEAD】，其中“要读取的字段名”为输入参数，可传入需要读取的字段，如Authorization等；“将字段值保存为参数”是输出字段，存放该字段的值。如果HEAD中不存在该字段，返回空字符串。
## 更多内容
* [葡萄城市场：插件下载地址](https://marketplace.grapecity.com.cn/ApplicationDetails?productID=SP2203310008&productDetailID=D2203310042&tabName=Tabs_detail)
* [活字格低代码开发平台](https://www.grapecity.com.cn/solutions/huozige)
* [了解低代码知识](https://help.grapecity.com.cn/display/lowcode)
