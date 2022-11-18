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
    public class ServerCommand : Command, ICommandExecutableInServerSide, IServerCommandParamGenerator
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
            return "读取HTTP请求HEAD";
        }

        /// <summary>
        /// 参数：设置了DisplayName的属性，都会被视作参数。默认情况下，参数会被视作传入型参数，如需将通过参数将处理结果返回，需要配合IServerCommandParamGenerator的GetGenerateParams方法，将该参数登记为传出参数
        /// 如果希望为这个参数支持公式编辑能力，进行一些预处理的话，需要将FormulaProperty设置为True；设置后，在使用读取该属性的值时，需要配套使用DataContext（ICommandExecutableInServerSide）中的EvaluateFormula方法
        /// </summary>
        [DisplayName("要读取的字段名")]
        public string FieldName { get; set; }


        /// <summary>
        /// 需要传出的参数，在设计器中填写的是参数的名称
        /// 建议通过DisplayName明确告知开发者
        /// </summary>
        [DisplayName("将字段值保存为参数：")]
        public string ParamaterName4FieldValue { get; set; }

        /// <summary>
        /// 命令执行逻辑
        /// </summary>
        /// <param name="dataContext">用来操作的上下文，包含HTTP请求的上下文、数据上下文等</param>
        /// <returns></returns>
        public ExecuteResult Execute(IServerCommandExecuteContext dataContext)
        {
            try
            {
                var value = string.Empty;

                // dataContext基于HTTP请求的上下文，Context属性的类型为Microsoft.AspNetCore.Http.HttpContext
                // 使用时，需要通过NuGet下载Microsoft.AspNetCore.Http.Abstractions组件
                if (dataContext.Context.Request.Headers.ContainsKey(this.FieldName))
                {
                    // 获取HTTP HEAD的字段               
                    value = dataContext.Context.Request.Headers[this.FieldName].ToString();
                }

                // 将结果写入dataContext的变量列表，即可实现数据的返回
                // 为了避免出错，建议先检查当前服务端命令的参数列表中是否有同名参数，如果没有需要创建，否则更新这个参数的值即可
                if (dataContext.Parameters.ContainsKey(ParamaterName4FieldValue))
                {
                    dataContext.Parameters[ParamaterName4FieldValue] = value;
                }
                else
                {
                    dataContext.Parameters.Add(ParamaterName4FieldValue, value);
                }

                // 大多数服务端命令插件不会内置Return行为，该命令执行过后，服务端命令还需要继续执行。
                // 否则可以通过设置返回对象的errCode、Message，然后设置AllowWriteResultToResponse为True，终止这个服务端命令并返回。
                return new ExecuteResult();
            }
            catch (Exception ex)
            {
                // 对于敏感度较低的功能插件来说，通常会将异常记录到日志，然后正常返回
                dataContext.Log.AppendLine("【从HTTP请求HEAD中读取Field】插件的Execute方法发生异常：\r\n" + ex.ToString());

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
            yield return new GenerateNormalParam()
            {
                ParamName = ParamaterName4FieldValue,
            };
        }
    }
}
