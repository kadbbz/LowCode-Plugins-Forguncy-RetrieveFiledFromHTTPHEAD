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
    public class WriteFieldToHTTPHeadCommand : Command, ICommandExecutableInServerSide, IServerCommandParamGenerator
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
            if (null == FieldName) {
                return "写入HTTP响应标头";
            } else {
                return "写入HTTP响应标头：" + FieldName.ToString();
            }
            
        }

        [DisplayName("要写入的标头："), FormulaProperty(true)]
        public object FieldName { get; set; }
        
        [DisplayName("值："), FormulaProperty(true)]
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
                string key = dataContext.EvaluateFormulaAsync(this.FieldName).Result.ToString();
                string value = dataContext.EvaluateFormulaAsync(this.ParamaterName4FieldValue).Result.ToString();

                if (dataContext.Context.Response.Headers.ContainsKey(key))
                {
                    dataContext.Context.Response.Headers[key] = value;
                }
                else
                {
                    dataContext.Context.Response.Headers.Add(key, value);
                }
                return new ExecuteResult();
            }
            catch (Exception exception)
            {
                dataContext.Log.AppendLine("【写入HTTP响应标头】的Execute方法发生异常：\r\n" + exception.ToString(), Array.Empty<string>());
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
