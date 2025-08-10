using NeoDEEX.ServiceModel.Services.Biz;
using System.Text;

namespace bizlib1;

// 간단한 예제 비즈 로직 클래스
[FoxBizClass("Test")]
public class SimpleBizLogicClass
{
    // 간단한 비즈 로직 메서드
    [FoxBizMethod("Echo")]
    public string Echo(string message)
    {
        return "Echo: " + message;
    }

    // 매개변수 덤프 메서드
    [FoxBizMethod("DumpParameters")]
    public string DumpParameters(IDictionary<string, object?> parameters)
    {
        if (parameters.Count < 1)
        {
            return "NO Parameters";
        }
        var result = new StringBuilder();
        result.Append("Parameters Dump= ");
        foreach (var kvp in parameters)
        {
            result.Append($"[{kvp.Key}]:{kvp.Value}  ");
        }
        return result.ToString();
    }
}
