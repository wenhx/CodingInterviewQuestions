using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using C = System.Console;

namespace JadeRabbit.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    C.WriteLine("请输入指令序列");
                    var commandSequence = C.ReadLine();
                    var result = Runner.Run(commandSequence.ToUpper());
                    C.WriteLine("结果：");
                    C.WriteLine(result);
                }
                catch (Exception ex)
                {
                    C.WriteLine("指令错误！");
                    C.WriteLine(ex.Message);
                }
                C.WriteLine("继续？（输入Y继续，其他退出。）");
                if (!string.Equals("Y", C.ReadLine(), StringComparison.OrdinalIgnoreCase))
                    break;
            }
            C.WriteLine("再见！");
        }
    }
}
