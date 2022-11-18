using GrapeCity.Forguncy.Commands;
using GrapeCity.Forguncy.Plugin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RetreiveFieldFormHTTPHead
{
    /// <summary>
    /// 插件的类。
    /// Command：插件的基类，适用于所有类型插件
    /// ICommandExecutableInServerSide：服务端命令专用的接口
    /// IServerCommandParamGenerator：需要将返回值写入变量的插件专用的接口
    /// Icon：设置在设计器中使用的图标，需要将其打包到Resources
    /// </summary>
    [Icon("pack://application:,,,/RetreiveFieldFormHTTPHead;component/Resources/Icon.png")]
    public class ServerCommand2 : Command, ICommandExecutableInServerSide, IServerCommandParamGenerator
    {
        /// <summary>
        /// 插件类型：设置为服务端命令插件
        /// </summary>
        /// <returns>插件类型枚举</returns>
        public override CommandScope GetCommandScope()
        {
            return CommandScope.ServerSide;
        }

        /// <summary>
        /// 在设计器中展示的插件名称
        /// </summary>
        /// <returns>易读的字符串</returns>
        public override string ToString()
        {
            return "获取处理本请求的服务器名";
        }

        /// <summary>
        /// 需要传出的参数，在设计器中填写的是参数的名称
        /// 建议通过DisplayName明确告知开发者
        /// </summary>
        [DisplayName("将机器名保存为参数：")]
        public string ParamaterName4HostName { get; set; }

        /// <summary>
        /// 需要传出的参数，在设计器中填写的是参数的名称
        /// 建议通过DisplayName明确告知开发者
        /// </summary>
        [DisplayName("将IPv4地址保存为参数：")]
        public string ParamaterName4Ip { get; set; }


        /// <summary>
        /// 命令执行逻辑
        /// </summary>
        /// <param name="dataContext">用来操作的上下文，包含HTTP请求的上下文、数据上下文等</param>
        /// <returns></returns>
        public ExecuteResult Execute(IServerCommandExecuteContext dataContext)
        {
            try
            {
                dataContext.Parameters[ParamaterName4HostName] = Environment.MachineName;
                var remoteIp = dataContext.Context.Connection.LocalIpAddress;
                var remotePort = dataContext.Context.Connection.LocalPort;

                dataContext.Parameters[ParamaterName4Ip] = (null != remoteIp)? remoteIp.MapToIPv4().ToString():"N/A";

                // 大多数服务端命令插件不会内置Return行为，该命令执行过后，服务端命令还需要继续执行。
                // 否则可以通过设置返回对象的errCode、Message，然后设置AllowWriteResultToResponse为True，终止这个服务端命令并返回。
                return new ExecuteResult();
            }
            catch (Exception ex)
            {
                // 对于敏感度较低的功能插件来说，通常会将异常记录到日志，然后正常返回
                dataContext.Log.AppendLine("【读取服务器信息】插件的Execute方法发生异常：\r\n" + ex.ToString());

                // 对于高度敏感的插件，记录日志后，依然需要将异常向上抛出
                //return new ExecuteResult()
                //{
                //    ErrCode = 1,
                //    Message = e.Message,
                //    AllowWriteResultToResponse = true
                //};

                return new ExecuteResult();
            }
        }

        /// <summary>
        /// 登记用来返回处理结果的参数名
        /// 这个值是在运行时确定的，推荐使用yield来处理
        /// </summary>
        /// <returns></returns>
        public IEnumerable<GenerateParam> GetGenerateParams()
        {
            List<GenerateParam> result = new List<GenerateParam>
            {
                new GenerateNormalParam()
                {
                    ParamName = ParamaterName4HostName,
                },

                new GenerateNormalParam()
                {
                    ParamName = ParamaterName4Ip,
                }
            };

            return result;
        }
    }
}
