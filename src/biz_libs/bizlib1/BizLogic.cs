using NeoDEEX.ServiceModel.Services.Biz;
using NeoDEEX.Transactions;
using System.Data;

namespace bizlib1;

//
// 트랜잭션을 위한 비즈 로직
//
// NOTE: SupportJsonType 속성을 true 로 지정하면 Javascript 에서 호출하는 JSON 으로부터 비즈 메서드의 매개변수로
//       형변환이 더 유연하게 처리됩니다. 예를 들어 JSON 실수형은 double 타입이 사용되지만 decimal 형 매개변수로 형변환이
//       자동으로 처리됩니다.
//       SupportJsonType 속성은 FoxBizClassAttribute 혹은 FoxBizMethodAttribute 에서 지정할 수 있습니다.
//       FoxBizClassAttribute 에서 지정하면 해당 클래스의 모든 메서드에 적용되며 FoxBizMethodAttribute 에서 지정하면
//       클래스 수준의 설정을 오버라이드하며 해당 메서드에만 적용됩니다.
[FoxBizClass("BizLogic", SupportJsonType = true)]
public class BizLogic : FoxBizBase
{
    [FoxBizMethod]
    [FoxAutoComplete]
    [FoxTransaction(FoxTransactionOption.Supported)]    // 단순 조회이므로 불필요한 트랜잭션을 사용하지 않음.
    public DataSet GetProducts(IDictionary<string, object?> parameters)
    {
        // 비즈 로직 메서드의 매개변수를 IDictionary<string, object?> 타입으로 사용하여
        // 가변 매개변수로 처리 가능.
        string? product_name;
        if (parameters.Count == 0)
        {
            product_name = null;
        }
        else if (parameters.Count == 1 && parameters.ContainsKey("product_name") == true)
        {
            product_name = parameters["product_name"] as string;
        }
        else
        {
            throw new ArgumentException("유효하지 않은 매개변수입니다. 매개변수를 제공하지 않거나 product_name 매개변수만을 제공해야 합니다.");
        }
        using DacLogic dac = new();
        IDacLogic itf = dac.CreateExecution<IDacLogic>();
        return itf.GetProducts(product_name);
    }

    [FoxBizMethod]
    [FoxAutoComplete]
    public int InsertProduct(string product_name, int category_id, decimal unit_price)
    {
        using DacLogic dac = new();
        IDacLogic itf = dac.CreateExecution<IDacLogic>();
        // 주: 트랜잭션 적용을 예제를 위해 product_id 를 구하고(SELECT MAX) 삽입을 수행합니다.
        //     실제에서는 2회의 DB 호출보다는 하나의 SQL 문장을 사용하여 처리하거나 저장 프로시저를 사용하는 것이 좋습니다.
        //     더욱이 이러한 코드는 동시성 문제를 유발할 수 있으므로 AUTO_INCREMENT 컬럼을 사용하거나 잠금 기법을 사용해야 합니다.
        int product_id = itf.GetMaxProductId();
        itf.InsertProduct(product_id, product_name, category_id, unit_price);
        // 삽입한 product_id 를 반환합니다.
        return product_id;
    }

    [FoxBizMethod]
    [FoxAutoComplete]
    public int DeleteProduct(int product_id)
    {
        using DacLogic dac = new();
        IDacLogic itf = dac.CreateExecution<IDacLogic>();
        // 삭제한 레코드 수를 반환합니다.
        return itf.DeleteProduct(product_id);
    }
}
